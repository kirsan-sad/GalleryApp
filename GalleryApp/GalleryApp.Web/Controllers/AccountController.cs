using GalleryApp.Domain.Interfaces;
using GalleryApp.Domain.Models;
using GalleryApp.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GalleryApp.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _repository;

        public AccountController(IUserRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                if (Url.IsLocalUrl(ViewBag.ReturnUrl))
                    return Redirect(ViewBag.ReturnUrl);

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (ModelState.IsValid)
            {
                User user = await _repository.LoginAsync(model);

                if (user != null)
                {
                    await Authenticate(model.Login);

                    //return RedirectToAction("Index", "Home");
                    //return Redirect(Request.UrlReferrer.ToString());

                    if (Url.IsLocalUrl(ViewBag.ReturnUrl))
                        return Redirect(ViewBag.ReturnUrl);
                }

                ModelState.AddModelError("", "Incorrect login and (or) password");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User model)
        {
            if (ModelState.IsValid)
            {
                User user = await _repository.RegisterAsync(model);
                if (user != null)
                {
                    await Authenticate(model.Login);

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "User with this login already exists");
            }

            return View(model);
        }

        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
