#pragma checksum "C:\websites\learn_arabic\learn_arabic\Views\ControlPanel\Groups\Group_Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "489351e3ff93b2bc4bcd9f8d87f413d5fd1a7cab"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_ControlPanel_Groups_Group_Details), @"mvc.1.0.view", @"/Views/ControlPanel/Groups/Group_Details.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"489351e3ff93b2bc4bcd9f8d87f413d5fd1a7cab", @"/Views/ControlPanel/Groups/Group_Details.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"afeaf25389d701414678b005532aeac63da68425", @"/Views/_ViewImports.cshtml")]
    public class Views_ControlPanel_Groups_Group_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<learn_arabic.Models.GroupModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\websites\learn_arabic\learn_arabic\Views\ControlPanel\Groups\Group_Details.cshtml"
  
    Layout = "_ControlPanel_Layout";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 6 "C:\websites\learn_arabic\learn_arabic\Views\ControlPanel\Groups\Group_Details.cshtml"
Write(await Html.PartialAsync("_header", true));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"

<!-- Page content -->
<div class=""container-fluid mt--6"">
    <div class=""row justify-content-center"">
        <div class=""col-6 p-5 ml-5 mr-5"">
            <div class=""card"">
                <!-- Card header -->
                <div class=""card-header border-0"">
                    <h3 class=""mb-0"">");
#nullable restore
#line 15 "C:\websites\learn_arabic\learn_arabic\Views\ControlPanel\Groups\Group_Details.cshtml"
                                Write(ViewBag.TableTitle);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</h3>
                </div>
                <div class=""table-responsive p-3"">
                    <div class=""align-items-center add-form w-100"" role=""dialog"" data-previous=""/Admin/Categories/"">
                        <div class=""form-group"">
                            <label class=""form-control-label"">");
#nullable restore
#line 20 "C:\websites\learn_arabic\learn_arabic\Views\ControlPanel\Groups\Group_Details.cshtml"
                                                         Write(Resources.General.ArabicName);

#line default
#line hidden
#nullable disable
            WriteLiteral(":</label>\r\n                            <input class=\"form-control border-0 bg-transparent\" type=\"text\"");
            BeginWriteAttribute("value", " value=\"", 902, "\"", 929, 1);
#nullable restore
#line 21 "C:\websites\learn_arabic\learn_arabic\Views\ControlPanel\Groups\Group_Details.cshtml"
WriteAttributeValue("", 910, Model?.Arabic_Name, 910, 19, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" placeholder=\"---------------\" readonly>\r\n                        </div>\r\n                        <div class=\"form-group\">\r\n                            <label for=\"example-text-input\" class=\"form-control-label\">");
#nullable restore
#line 24 "C:\websites\learn_arabic\learn_arabic\Views\ControlPanel\Groups\Group_Details.cshtml"
                                                                                  Write(Resources.General.EnglishName);

#line default
#line hidden
#nullable disable
            WriteLiteral(":</label>\r\n                            <input class=\"form-control border-0 bg-transparent\" type=\"text\"");
            BeginWriteAttribute("value", " value=\"", 1273, "\"", 1301, 1);
#nullable restore
#line 25 "C:\websites\learn_arabic\learn_arabic\Views\ControlPanel\Groups\Group_Details.cshtml"
WriteAttributeValue("", 1281, Model?.English_Name, 1281, 20, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" placeholder=\"---------------\" readonly>\r\n                        </div>\r\n                        <div class=\"form-group\">\r\n                            <label for=\"example-text-input\" class=\"form-control-label\">");
#nullable restore
#line 28 "C:\websites\learn_arabic\learn_arabic\Views\ControlPanel\Groups\Group_Details.cshtml"
                                                                                  Write(Resources.General.TurkishName);

#line default
#line hidden
#nullable disable
            WriteLiteral(":</label>\r\n                            <input class=\"form-control border-0 bg-transparent\" type=\"text\"");
            BeginWriteAttribute("value", " value=\"", 1645, "\"", 1673, 1);
#nullable restore
#line 29 "C:\websites\learn_arabic\learn_arabic\Views\ControlPanel\Groups\Group_Details.cshtml"
WriteAttributeValue("", 1653, Model?.Turkish_Name, 1653, 20, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" placeholder=\"---------------\" readonly>\r\n                        </div>\r\n                        <div class=\"form-group\">\r\n                            <label for=\"example-text-input\" class=\"form-control-label\">");
#nullable restore
#line 32 "C:\websites\learn_arabic\learn_arabic\Views\ControlPanel\Groups\Group_Details.cshtml"
                                                                                  Write(Resources.General.RussianName);

#line default
#line hidden
#nullable disable
            WriteLiteral(":</label>\r\n                            <input class=\"form-control border-0 bg-transparent\" type=\"text\"");
            BeginWriteAttribute("value", " value=\"", 2017, "\"", 2045, 1);
#nullable restore
#line 33 "C:\websites\learn_arabic\learn_arabic\Views\ControlPanel\Groups\Group_Details.cshtml"
WriteAttributeValue("", 2025, Model?.Russian_Name, 2025, 20, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" placeholder=\"---------------\" readonly>\r\n                        </div>\r\n                        <div class=\"form-group\">\r\n                            <label for=\"example-text-input\" class=\"form-control-label\">");
#nullable restore
#line 36 "C:\websites\learn_arabic\learn_arabic\Views\ControlPanel\Groups\Group_Details.cshtml"
                                                                                  Write(Resources.General.Prev_Name);

#line default
#line hidden
#nullable disable
            WriteLiteral(":</label>\r\n                            <input class=\"form-control border-0 bg-transparent\" type=\"text\"");
            BeginWriteAttribute("value", " value=\"", 2387, "\"", 2412, 1);
#nullable restore
#line 37 "C:\websites\learn_arabic\learn_arabic\Views\ControlPanel\Groups\Group_Details.cshtml"
WriteAttributeValue("", 2395, Model?.Prev_Name, 2395, 17, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" placeholder=\"---------------\" readonly>\r\n                        </div>\r\n                        <div class=\"form-group\">\r\n                            <label for=\"example-text-input\" class=\"form-control-label\">");
#nullable restore
#line 40 "C:\websites\learn_arabic\learn_arabic\Views\ControlPanel\Groups\Group_Details.cshtml"
                                                                                  Write(Resources.General.Category_Name);

#line default
#line hidden
#nullable disable
            WriteLiteral(":</label>\r\n                            <input class=\"form-control border-0 bg-transparent\" type=\"text\"");
            BeginWriteAttribute("value", " value=\"", 2758, "\"", 2787, 1);
#nullable restore
#line 41 "C:\websites\learn_arabic\learn_arabic\Views\ControlPanel\Groups\Group_Details.cshtml"
WriteAttributeValue("", 2766, Model?.Category_Name, 2766, 21, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" placeholder=\"---------------\" readonly>\r\n                        </div>\r\n                        <div class=\"form-group\">\r\n                            <label for=\"example-text-input\" class=\"form-control-label\">");
#nullable restore
#line 44 "C:\websites\learn_arabic\learn_arabic\Views\ControlPanel\Groups\Group_Details.cshtml"
                                                                                  Write(Resources.General.Start_Time);

#line default
#line hidden
#nullable disable
            WriteLiteral(":</label>\r\n                            <input class=\"form-control border-0 bg-transparent\" type=\"time\"");
            BeginWriteAttribute("value", " value=\"", 3130, "\"", 3156, 1);
#nullable restore
#line 45 "C:\websites\learn_arabic\learn_arabic\Views\ControlPanel\Groups\Group_Details.cshtml"
WriteAttributeValue("", 3138, Model?.Start_Time, 3138, 18, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" placeholder=\"---------------\" readonly>\r\n                        </div>\r\n                        <div class=\"form-group\">\r\n                            <label for=\"example-text-input\" class=\"form-control-label\">");
#nullable restore
#line 48 "C:\websites\learn_arabic\learn_arabic\Views\ControlPanel\Groups\Group_Details.cshtml"
                                                                                  Write(Resources.General.End_Time);

#line default
#line hidden
#nullable disable
            WriteLiteral(":</label>\r\n                            <input class=\"form-control border-0 bg-transparent\" type=\"time\"");
            BeginWriteAttribute("value", " value=\"", 3497, "\"", 3521, 1);
#nullable restore
#line 49 "C:\websites\learn_arabic\learn_arabic\Views\ControlPanel\Groups\Group_Details.cshtml"
WriteAttributeValue("", 3505, Model?.End_Time, 3505, 16, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" placeholder=\"---------------\" readonly>\r\n                        </div>\r\n                        <div class=\"text-center\">\r\n                            <a");
            BeginWriteAttribute("href", " href=\"", 3677, "\"", 3702, 1);
#nullable restore
#line 52 "C:\websites\learn_arabic\learn_arabic\Views\ControlPanel\Groups\Group_Details.cshtml"
WriteAttributeValue("", 3684, ViewBag.ReturnUrl, 3684, 18, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"btn btn-primary ml-auto\">");
#nullable restore
#line 52 "C:\websites\learn_arabic\learn_arabic\Views\ControlPanel\Groups\Group_Details.cshtml"
                                                                                    Write(Resources.General.Close);

#line default
#line hidden
#nullable disable
            WriteLiteral("</a>\r\n                        </div>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<learn_arabic.Models.GroupModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
