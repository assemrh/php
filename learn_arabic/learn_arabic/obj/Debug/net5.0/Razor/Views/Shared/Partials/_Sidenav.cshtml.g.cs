#pragma checksum "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_Sidenav.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4b28a0a29fb7914147b75cd6c7f65326cc6ba1eb"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Partials__Sidenav), @"mvc.1.0.view", @"/Views/Shared/Partials/_Sidenav.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4b28a0a29fb7914147b75cd6c7f65326cc6ba1eb", @"/Views/Shared/Partials/_Sidenav.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"afeaf25389d701414678b005532aeac63da68425", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_Partials__Sidenav : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/img/svg/quran.svg"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("navbar-brand-img"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("alt", new global::Microsoft.AspNetCore.Html.HtmlString("..."), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"
<nav class=""sidenav navbar navbar-vertical  fixed-left  navbar-expand-xs navbar-light bg-default"" id=""sidenav-main"">
    <div class=""scrollbar-inner"">
        <!-- Brand -->
        <div class=""sidenav-header  align-items-center"">
            <a class=""navbar-brand"" href=""/admin"">
                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "4b28a0a29fb7914147b75cd6c7f65326cc6ba1eb4846", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
            </a>
            
        </div>
        <hr class=""mt-0 bg-white"">

        <div class=""navbar-inner"">
            <!-- Collapse -->
            <div class=""collapse navbar-collapse"" id=""sidenav-collapse-main"">
                <!-- Nav items -->
                <ul class=""navbar-nav nav nav-sidebar flex-column"" data-widget=""treeview"" role=""menu"" data-accordion=""false"">
                    <li class=""nav-item "">
                        <a class=""nav-link text-white"" href=""#"">
                            <i class=""fas fa-building""></i>
                            <span class=""nav-link-text"">Company</span>
                            <i class=""right fas fa-angle-left""></i>

                        </a>
                        <ul class=""nav nav-treeview"">
                            <li class=""nav-item"">
                                <a href=""/admin/Company/"" class=""nav-link text-white align-middle"">
                                    <i class=""far fa-circle nav-icon""></i>
");
            WriteLiteral(@"                                    <p>Company</p>
                                </a>
                            </li>
                            <li class=""nav-item"">
                                <a href=""/admin/Company/types"" class=""nav-link text-white align-middle"">
                                    <i class=""far fa-circle nav-icon""></i>
                                    <p>Company types</p>
                                </a>
                            </li>
                        </ul>
                    </li>
                    <li class=""nav-item "">
                        <a class=""nav-link text-white"" href=""#"">
                            <i class=""fas fa-users""></i>
                            <span class=""nav-link-text"">");
#nullable restore
#line 43 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_Sidenav.cshtml"
                                                   Write(Resources.General.Users);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</span>
                            <i class=""right fas fa-angle-left""></i>

                        </a>
                        <ul class=""nav nav-treeview"">
                            <li class=""nav-item"">
                                <a href=""/admin/user/"" class=""nav-link text-white align-middle"">
                                    <i class=""far fa-circle nav-icon""></i>
                                    <p>");
#nullable restore
#line 51 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_Sidenav.cshtml"
                                  Write(Resources.General.Users);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</p>
                                </a>
                            </li>

                        </ul>
                    </li>
                    <li class=""nav-item "">
                        <a class=""nav-link text-white"" href=""#"">
                            <i class=""ni ni-building""></i>
                            <span class=""nav-link-text"">");
#nullable restore
#line 60 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_Sidenav.cshtml"
                                                   Write(Resources.General.Countries);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</span>
                            <i class=""right fas fa-angle-left""></i>
                        </a>
                        <ul class=""nav nav-treeview"">
                            <li class=""nav-item"">
                                <a href=""/admin/Countries/"" class=""nav-link text-white align-middle"">
                                    <i class=""far fa-circle nav-icon""></i>
                                    <p>");
#nullable restore
#line 67 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_Sidenav.cshtml"
                                  Write(Resources.General.Countries);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</p>
                                </a>
                            </li>

                        </ul>
                    </li>
                    <li class=""nav-item"">
                        <a href=""/admin/categories"" class=""nav-link text-white"">
                            <i class=""bi bi-tags""></i>
                            <span class=""nav-link-text"">");
#nullable restore
#line 76 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_Sidenav.cshtml"
                                                   Write(Resources.General.Categories);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n                        </a>\r\n");
            WriteLiteral(@"                    </li>
                    <li class=""nav-item"">
                        <a href=""/admin/Groups"" class=""nav-link text-white"">
                            <i class=""bi bi-tags""></i>
                            <span class=""nav-link-text"">");
#nullable restore
#line 96 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_Sidenav.cshtml"
                                                   Write(Resources.General.Groups);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</span>
                        </a>
                    </li>
                    <li class=""nav-item"">
                        <a href=""/admin/Answers"" class=""nav-link text-white"">
                            <i class=""bi bi-tags""></i>
                            <span class=""nav-link-text"">");
#nullable restore
#line 102 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_Sidenav.cshtml"
                                                   Write(Resources.General.Answers);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</span>
                        </a>
                    </li>
                    <li class=""nav-item"">
                        <a href=""/admin/Examples"" class=""nav-link text-white"">
                            <i class=""bi bi-tags""></i>
                            <span class=""nav-link-text"">");
#nullable restore
#line 108 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_Sidenav.cshtml"
                                                   Write(Resources.General.Examples);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</span>
                        </a>
                    </li>
                    <li class=""nav-item"">
                        <a href=""/admin/Examps"" class=""nav-link text-white"">
                            <i class=""bi bi-tags""></i>
                            <span class=""nav-link-text"">");
#nullable restore
#line 114 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_Sidenav.cshtml"
                                                   Write(Resources.General.Examps);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</span>
                        </a>
                    </li>
                    <li class=""nav-item"">
                        <a href=""/admin/Helpful_Links"" class=""nav-link text-white"">
                            <i class=""bi bi-tags""></i>
                            <span class=""nav-link-text"">");
#nullable restore
#line 120 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_Sidenav.cshtml"
                                                   Write(Resources.General.Helpful_Links);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</span>
                        </a>
                    </li>
                    <li class=""nav-item"">
                        <a href=""/admin/Lessons"" class=""nav-link text-white"">
                            <i class=""bi bi-tags""></i>
                            <span class=""nav-link-text"">");
#nullable restore
#line 126 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_Sidenav.cshtml"
                                                   Write(Resources.General.Lessons);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</span>
                        </a>
                    </li>
                    <li class=""nav-item"">
                        <a href=""/admin/Questions"" class=""nav-link text-white"">
                            <i class=""bi bi-tags""></i>
                            <span class=""nav-link-text"">");
#nullable restore
#line 132 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_Sidenav.cshtml"
                                                   Write(Resources.General.Questions);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</span>
                        </a>
                    </li>
                    <li class=""nav-item"">
                        <a href=""/admin/Shapes"" class=""nav-link text-white"">
                            <i class=""bi bi-tags""></i>
                            <span class=""nav-link-text"">");
#nullable restore
#line 138 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_Sidenav.cshtml"
                                                   Write(Resources.General.Shapes);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</span>
                        </a>
                    </li>
                    <li class=""nav-item"">
                        <a href=""/admin/Suggestions"" class=""nav-link text-white"">
                            <i class=""bi bi-tags""></i>
                            <span class=""nav-link-text"">");
#nullable restore
#line 144 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_Sidenav.cshtml"
                                                   Write(Resources.General.Suggestions);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</span>
                        </a>
                    </li>
                    <li class=""nav-item"">
                        <a href=""/admin/Tutorials"" class=""nav-link text-white"">
                            <i class=""bi bi-tags""></i>
                            <span class=""nav-link-text"">");
#nullable restore
#line 150 "C:\websites\learn_arabic\learn_arabic\Views\Shared\Partials\_Sidenav.cshtml"
                                                   Write(Resources.General.Tutorials);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</span>
                        </a>
                    </li>
                    <li class=""nav-item"">
                        <a href=""#"" class=""nav-link text-white"">
                            <i class=""ni ni-building""></i>

                            <span class=""nav-link-text"">Dashboard</span>
                            <i class=""right fas fa-angle-left""></i>
                        </a>
                        <ul class=""nav nav-treeview"">
                            <li class=""nav-item"">
                                <a href=""./index.html"" class=""nav-link text-white align-middle"">
                                    <i class=""far fa-circle nav-icon""></i>
                                    <p>Dashboard v1</p>
                                </a>
                            </li>
                            <li class=""nav-item"">
                                <a href=""./index2.html"" class=""nav-link text-white align-middle"">
                                    <i class=""far fa-c");
            WriteLiteral(@"ircle nav-icon""></i>
                                    <p>Dashboard v2</p>
                                </a>
                            </li>
                            <li class=""nav-item"">
                                <a href=""./index3.html"" class=""nav-link text-white align-middle"">
                                    <i class=""far fa-circle nav-icon""></i>
                                    <p>Dashboard v3</p>
                                </a>
                            </li>
                        </ul>
                    </li>
                    <!--
    <li class=""nav-item"">
        <a class=""nav-link active"" href=""examples/dashboard.html"">
            <i class=""ni ni-tv-2 text-primary""></i>
            <span class=""nav-link-text"">Dashboard</span>
        </a>
    </li>
    <li class=""nav-item"">
        <a class=""nav-link"" href=""examples/icons.html"">
            <i class=""ni ni-planet text-orange""></i>
            <span class=""nav-link-text"">Icons</span>
        <");
            WriteLiteral(@"/a>
    </li>
    <li class=""nav-item"">
        <a class=""nav-link"" href=""examples/map.html"">
            <i class=""ni ni-pin-3 text-primary""></i>
            <span class=""nav-link-text"">Google</span>
        </a>
    </li>
    <li class=""nav-item"">
        <a class=""nav-link"" href=""examples/profile.html"">
            <i class=""ni ni-single-02 text-yellow""></i>
            <span class=""nav-link-text"">Profile</span>
        </a>
    </li>
    <li class=""nav-item"">
        <a class=""nav-link"" href=""examples/tables.html"">
            <i class=""ni ni-bullet-list-67 text-default""></i>
            <span class=""nav-link-text"">Tables</span>
        </a>
    </li>
    <li class=""nav-item"">
        <a class=""nav-link"" href=""examples/login.html"">
            <i class=""ni ni-key-25 text-info""></i>
            <span class=""nav-link-text"">Login</span>
        </a>
    </li>
    <li class=""nav-item"">
        <a class=""nav-link"" href=""examples/register.html"">
            <i class=""ni ni-circle-08 ");
            WriteLiteral(@"text-pink""></i>
            <span class=""nav-link-text"">Register</span>
        </a>
    </li>
    <li class=""nav-item"">
        <a class=""nav-link"" href=""examples/upgrade.html"">
            <i class=""ni ni-send text-dark""></i>
            <span class=""nav-link-text"">Upgrade</span>
        </a>
    </li>-->
                </ul>
                <!-- Divider -->
                <hr class=""my-3 bg-white"">
                <!-- Heading -->
                <h6 class=""navbar-heading p-0 text-muted"">
                    <span class=""docs-normal"">Documentation</span>
                </h6>
                <!-- Navigation -->
                <ul class=""navbar-nav mb-md-3"">
                    <!--<li class=""nav-item"">
                    <a class=""nav-link"" href=""https://demos.creative-tim.com/argon-dashboard/docs/getting-started/overview.html"" target=""_blank"">
                        <i class=""ni ni-spaceship""></i>
                        <span class=""nav-link-text"">Getting started</span>
      ");
            WriteLiteral(@"              </a>
                </li>
                <li class=""nav-item"">
                    <a class=""nav-link"" href=""https://demos.creative-tim.com/argon-dashboard/docs/foundation/colors.html"" target=""_blank"">
                        <i class=""ni ni-palette""></i>
                        <span class=""nav-link-text"">Foundation</span>
                    </a>
                </li>
                <li class=""nav-item"">
                    <a class=""nav-link"" href=""https://demos.creative-tim.com/argon-dashboard/docs/components/alerts.html"" target=""_blank"">
                        <i class=""ni ni-ui-04""></i>
                        <span class=""nav-link-text"">Components</span>
                    </a>
                </li>
                <li class=""nav-item"">
                    <a class=""nav-link"" href=""https://demos.creative-tim.com/argon-dashboard/docs/plugins/charts.html"" target=""_blank"">
                        <i class=""ni ni-chart-pie-35""></i>
                        <span class=""na");
            WriteLiteral(@"v-link-text"">Plugins</span>
                    </a>
                </li>
                <li class=""nav-item"">
                    <a class=""nav-link active active-pro"" href=""examples/upgrade.html"">
                        <i class=""ni ni-send text-dark""></i>
                        <span class=""nav-link-text"">Upgrade to PRO</span>
                    </a>
                </li>-->
                </ul>
            </div>
        </div>
    </div>
</nav>");
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