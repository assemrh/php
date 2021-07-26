using learn_arabic.Classes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Threading.Tasks;

namespace learn_arabic
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddAuthentication("CookieAuth").AddCookie("CookieAuth","display name", config =>
            {
                config.Cookie.Name = "Cookie-" + Ciphering.GetMD5HashData("authCookie");
                
                config.LoginPath = "/user/login";
                //config.AccessDeniedPath= "/user/login";
                
                config.SlidingExpiration = true;
                config.ExpireTimeSpan = TimeSpan.FromHours(8);
                config.Events.OnRedirectToLogin = ctx =>
                {
                    if (ctx.Request.Headers.ContainsKey("Postman-Token") &&
                        ctx.Response.StatusCode == (int)HttpStatusCode.OK)
                    {
                        ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        ctx.Response.Redirect("/user/Unauthorized");
                    }
                    else
                    {
                        ctx.Response.Redirect(ctx.RedirectUri);
                    }
                    return Task.FromResult(0);
                };

            });

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });//You can set Time  
            //services.AddRazorPages();
            services.AddControllersWithViews();
            services.AddMvc()
                  .AddRazorOptions(option => {
                      option.ViewLocationFormats.Add("/Views/{1}/Partials/{0}.cshtml");
                      option.ViewLocationFormats.Add("/Views/Shared/Partials/{0}.cshtml");
                      option.ViewLocationFormats.Add("/Views/Shared/Layouts/{0}.cshtml");
                      option.ViewLocationFormats.Add("/Views/ControlPanel/{1}/{0}.cshtml");
                      //option.AreaViewLocationFormats.Add("/Views/ControlPanel/{1}/{0}.cshtml");
                  });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"); 
                //endpoints.MapControllerRoute(
                //    name: "cp",
                //    pattern: "cp/{controller=controlpanel}/{action=Index}/{id?}"); //controlpanel
            });
        }
    }
}
