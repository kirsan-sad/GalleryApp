using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalleryApp.Domain.Interfaces;
using GalleryApp.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GalleryApp.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostApiController : ControllerBase
    {
        private readonly IPhotoRepository _repository;

        public PostApiController(IPhotoRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [Produces("application/json")]
        [HttpGet("search")]
        public async Task<ActionResult> Search()
        {
            try
            {
                ICollection<Photo> photoList = await _repository.GetPhotosAsync();
                string term = HttpContext.Request.Query["term"].ToString();
                var photoTitle = photoList.Where(p => p.Title.Contains(term))
                                          .Select(p => p.Title).ToList();

                return Ok(photoTitle);
            }
            catch
            {
                return BadRequest();
            }

        }
    }
}