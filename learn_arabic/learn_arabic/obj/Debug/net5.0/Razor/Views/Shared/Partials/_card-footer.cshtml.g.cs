#pragma checksum "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_card-footer.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2820b54d0d063a53f2bb51db92ceb3d4895ac8da"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Partials__card_footer), @"mvc.1.0.view", @"/Views/Shared/Partials/_card-footer.cshtml")]
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
#line 1 "C:\websites\learn_arabic\learn_arabic\Views\_ViewImports.cshtml"
using learn_arabic;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\websites\learn_arabic\learn_arabic\Views\_ViewImports.cshtml"
using learn_arabic.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\websites\learn_arabic\learn_arabic\Views\_ViewImports.cshtml"
using learn_arabic.Classes;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\websites\learn_arabic\learn_arabic\Views\_ViewImports.cshtml"
using static learn_arabic.Classes.HelperClass;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2820b54d0d063a53f2bb51db92ceb3d4895ac8da", @"/Views/Shared/Partials/_card-footer.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"afeaf25389d701414678b005532aeac63da68425", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_Partials__card_footer : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_card-footer.cshtml"
  
    int currentPage = ViewBag.CurrentPage  ?? 1;
    int TotalPage = ViewBag.TotalPage ?? 1;
    var next = (currentPage == TotalPage) ? "disabled" : "";
    var prev = (currentPage == 1) ? "disabled" : "";

#line default
#line hidden
#nullable disable
            WriteLiteral("    <!-- Card footer -->\r\n<div class=\"card-footer py-4\">\r\n    <nav aria-label=\"...\">\r\n        <ul class=\"pagination justify-content-end mb-0\">\r\n            <li");
            BeginWriteAttribute("class", " class=\"", 377, "\"", 400, 2);
            WriteAttributeValue("", 385, "page-item", 385, 9, true);
#nullable restore
#line 11 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_card-footer.cshtml"
WriteAttributeValue(" ", 394, prev, 395, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                <a class=\"page-link\"");
            BeginWriteAttribute("href", " href=\"", 440, "\"", 493, 3);
#nullable restore
#line 12 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_card-footer.cshtml"
WriteAttributeValue("", 447, Context.Request.Path.Value, 447, 27, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 474, "?p=", 474, 3, true);
#nullable restore
#line 12 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_card-footer.cshtml"
WriteAttributeValue("", 477, currentPage-1, 477, 16, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" tabindex=\"-1\">\r\n                    <i class=\"fas fa-angle-left\"></i>\r\n                    <span class=\"sr-only\">Previous</span>\r\n                </a>\r\n            </li>\r\n\r\n");
#nullable restore
#line 18 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_card-footer.cshtml"
             for (int j = 1; TotalPage >= j; j++)
            {
                var _class = (j == currentPage )? "active" : "";

#line default
#line hidden
#nullable disable
            WriteLiteral("                <li");
            BeginWriteAttribute("class", " class=\"", 819, "\"", 844, 2);
            WriteAttributeValue("", 827, "page-item", 827, 9, true);
#nullable restore
#line 21 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_card-footer.cshtml"
WriteAttributeValue(" ", 836, _class, 837, 7, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                    <a class=\"page-link\"");
            BeginWriteAttribute("href", " href=\"", 888, "\"", 927, 3);
#nullable restore
#line 22 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_card-footer.cshtml"
WriteAttributeValue("", 895, Context.Request.Path.Value, 895, 27, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 922, "?p=", 922, 3, true);
#nullable restore
#line 22 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_card-footer.cshtml"
WriteAttributeValue("", 925, j, 925, 2, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                        ");
#nullable restore
#line 23 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_card-footer.cshtml"
                   Write(j);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 24 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_card-footer.cshtml"
                         if (j == currentPage)
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <span class=\"sr-only\">(current)</span>\r\n");
#nullable restore
#line 27 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_card-footer.cshtml"
                        }

#line default
#line hidden
#nullable disable
            WriteLiteral("                    </a>\r\n                </li>\r\n");
#nullable restore
#line 30 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_card-footer.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("            <li");
            BeginWriteAttribute("class", " class=\"", 1208, "\"", 1231, 2);
            WriteAttributeValue("", 1216, "page-item", 1216, 9, true);
#nullable restore
#line 31 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_card-footer.cshtml"
WriteAttributeValue(" ", 1225, next, 1226, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                <a class=\"page-link\"");
            BeginWriteAttribute("href", " href=\"", 1271, "\"", 1324, 3);
#nullable restore
#line 32 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_card-footer.cshtml"
WriteAttributeValue("", 1278, Context.Request.Path.Value, 1278, 27, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 1305, "?p=", 1305, 3, true);
#nullable restore
#line 32 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_card-footer.cshtml"
WriteAttributeValue("", 1308, currentPage+1, 1308, 16, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                    <i class=\"fas fa-angle-right\"></i>\r\n                    <span class=\"sr-only\">Next</span>\r\n                </a>\r\n            </li>\r\n        </ul>\r\n    </nav>\r\n</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
