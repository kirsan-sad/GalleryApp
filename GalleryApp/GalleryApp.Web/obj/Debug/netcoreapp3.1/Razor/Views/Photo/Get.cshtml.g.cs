#pragma checksum "C:\Users\Анон\source\repos\kirsan-sad\GalleryApp\GalleryApp\GalleryApp.Web\Views\Photo\Get.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "45e47657a295f08976df433188a9ab61b94d0f8e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Photo_Get), @"mvc.1.0.view", @"/Views/Photo/Get.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"45e47657a295f08976df433188a9ab61b94d0f8e", @"/Views/Photo/Get.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"09fcce4225639dd4a393cecd20f7e23631dc077d", @"/Views/_ViewImports.cshtml")]
    public class Views_Photo_Get : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<GalleryApp.Domain.Models.Photo>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\Анон\source\repos\kirsan-sad\GalleryApp\GalleryApp\GalleryApp.Web\Views\Photo\Get.cshtml"
  
    ViewData["Title"] = "Details";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 7 "C:\Users\Анон\source\repos\kirsan-sad\GalleryApp\GalleryApp\GalleryApp.Web\Views\Photo\Get.cshtml"
 foreach (var f in Model)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <p><img style=\"width:200px;height:200px\"");
            BeginWriteAttribute("src", " src=\"", 173, "\"", 194, 2);
            WriteAttributeValue("", 179, "/Images/", 179, 8, true);
#nullable restore
#line 9 "C:\Users\Анон\source\repos\kirsan-sad\GalleryApp\GalleryApp\GalleryApp.Web\Views\Photo\Get.cshtml"
WriteAttributeValue("", 187, f.Name, 187, 7, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("></p>\r\n");
#nullable restore
#line 10 "C:\Users\Анон\source\repos\kirsan-sad\GalleryApp\GalleryApp\GalleryApp.Web\Views\Photo\Get.cshtml"
}

#line default
#line hidden
#nullable disable
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
