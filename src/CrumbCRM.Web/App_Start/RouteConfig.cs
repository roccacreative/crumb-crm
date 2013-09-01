using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LowercaseRoutesMVC4;

namespace CrumbCRM.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRouteLowercase(
                name: "Leads",
                url: "Leads/{id}/{order}",
                defaults: new { controller = "Lead", action = "Index", id = UrlParameter.Optional, order = UrlParameter.Optional }
            );

            routes.MapRouteLowercase(
                name: "Sales",
                url: "Sales/{id}/{order}",
                defaults: new { controller = "Sale", action = "Index", id = UrlParameter.Optional, order = UrlParameter.Optional }
            );

            routes.MapRouteLowercase(
                name: "OrderingDesc",
                url: "{controller}/desc",
                defaults: new { controller = "Contacts", action = "Index", order = UrlParameter.Optional }
            );

            routes.MapRouteLowercase(
                name: "OrderingAsc",
                url: "{controller}/asc",
                defaults: new { controller = "Contacts", action = "Index", order = UrlParameter.Optional }
            );

            routes.MapRouteLowercase(
                name: "Order",
                url: "{controller}/{action}/{id}/{order}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, order = UrlParameter.Optional }
            );

            routes.MapRouteLowercase(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}