#pragma checksum "C:\Users\Анон\source\repos\kirsan-sad\GalleryApp\GalleryApp\GalleryApp.Web\Views\Admin\Genres.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b3987be3cad7860f7393de31f80b4904d4537d90"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Admin_Genres), @"mvc.1.0.view", @"/Views/Admin/Genres.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Анон\source\repos\kirsan-sad\GalleryApp\GalleryApp\GalleryApp.Web\Views\_ViewImports.cshtml"
using GalleryApp.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Анон\source\repos\kirsan-sad\GalleryApp\GalleryApp\GalleryApp.Web\Views\_ViewImports.cshtml"
using GalleryApp.Web.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b3987be3cad7860f7393de31f80b4904d4537d90", @"/Views/Admin/Genres.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"09fcce4225639dd4a393cecd20f7e23631dc077d", @"/Views/_ViewImports.cshtml")]
    public class Views_Admin_Genres : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable< GalleryApp.Domain.Models.Genre>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\Анон\source\repos\kirsan-sad\GalleryApp\GalleryApp\GalleryApp.Web\Views\Admin\Genres.cshtml"
   ViewData["Title"] = "Index"; 

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div id=\"view-all\">\r\n    ");
#nullable restore
#line 6 "C:\Users\Анон\source\repos\kirsan-sad\GalleryApp\GalleryApp\GalleryApp.Web\Views\Admin\Genres.cshtml"
Write(await Html.PartialAsync("_ViewAllGenres", Model));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</div>\r\n\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n");
#nullable restore
#line 10 "C:\Users\Анон\source\repos\kirsan-sad\GalleryApp\GalleryApp\GalleryApp.Web\Views\Admin\Genres.cshtml"
      await Html.RenderPartialAsync("_ValidationScriptsPartial");

#line default
#line hidden
#nullable disable
            }
            );
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable< GalleryApp.Domain.Models.Genre>> Html { get; private set; }
    }
}
#pragma warning restore 1591
