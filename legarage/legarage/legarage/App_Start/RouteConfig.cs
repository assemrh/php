using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace legarage
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            //Custom Route
            routes.MapRoute(
            name: "Products", //RouteName
            url: "Products/{action}/{id}", //Route URL Pattern

            // Controller and Action Method for Above Defined URL Pattern
            defaults: new
            {
                Controller = "Parts",
                action = "PartDetails",
                id = UrlParameter.Optional
            });
            routes.MapRoute(
                name: "Details",
                url: "Prodects/Details/{id}",
                defaults: new { controller = "Parts", action = "PartDetails", id = UrlParameter.Optional }
            );
            //routes.MapRoute(
            //                name: "Details",
            //                url: "Prodects/Details/{id}",
            //                defaults: new { controller = "Parts", action = "PartDetails", id = UrlParameter.Optional }
            //            );
            //routes.MapRoute(
            //                name: "Details",
            //                url: "Prodects/Details/{id}",
            //                defaults: new { controller = "Parts", action = "PartDetails", id = UrlParameter.Optional }
            //            );
            //routes.MapRoute(
            //                name: "Details",
            //                url: "Prodects/Details/{id}",
            //                defaults: new { controller = "Parts", action = "PartDetails", id = UrlParameter.Optional }
            //            );
            //routes.MapRoute(
            //                name: "Details",
            //                url: "Prodects/Details/{id}",
            //                defaults: new { controller = "Parts", action = "PartDetails", id = UrlParameter.Optional }
            //            );
            //routes.MapRoute(
            //                name: "Details",
            //                url: "Prodects/Details/{id}",
            //                defaults: new { controller = "Parts", action = "PartDetails", id = UrlParameter.Optional }
            //            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Index",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           );


        }
    }
}
