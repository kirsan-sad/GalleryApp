#pragma checksum "E:\work\source\repos2020\GalleryApp\GalleryApp\GalleryApp.Web\Views\Admin\Photos.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e12c2384e09368533f38285899971df8d7fcf6db"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Admin_Photos), @"mvc.1.0.view", @"/Views/Admin/Photos.cshtml")]
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
#line 1 "E:\work\source\repos2020\GalleryApp\GalleryApp\GalleryApp.Web\Views\_ViewImports.cshtml"
using GalleryApp.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\work\source\repos2020\GalleryApp\GalleryApp\GalleryApp.Web\Views\_ViewImports.cshtml"
using GalleryApp.Web.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e12c2384e09368533f38285899971df8d7fcf6db", @"/Views/Admin/Photos.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"09fcce4225639dd4a393cecd20f7e23631dc077d", @"/Views/_ViewImports.cshtml")]
    public class Views_Admin_Photos : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<GalleryApp.Domain.Models.Photo>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "E:\work\source\repos2020\GalleryApp\GalleryApp\GalleryApp.Web\Views\Admin\Photos.cshtml"
   ViewData["Title"] = "Index"; 

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"table-transp\">\r\n    ");
#nullable restore
#line 6 "E:\work\source\repos2020\GalleryApp\GalleryApp\GalleryApp.Web\Views\Admin\Photos.cshtml"
Write(await Html.PartialAsync("_ViewAllPhotos", Model));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</div>\r\n\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n");
#nullable restore
#line 10 "E:\work\source\repos2020\GalleryApp\GalleryApp\GalleryApp.Web\Views\Admin\Photos.cshtml"
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<GalleryApp.Domain.Models.Photo>> Html { get; private set; }
    }
}
#pragma warning restore 1591
