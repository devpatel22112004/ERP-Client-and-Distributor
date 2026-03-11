using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCMarketing
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Custom route for ClientAccess
            routes.MapRoute(
                name: "ClientAccess",
                url: "ClientAccess/{token}",
                defaults: new { controller = "ClientAccess", action = "Index", extra = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DeliveryAccess",
                url: "DeliveryAccess/{action}",
                defaults: new { controller = "DeliveryAccess", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
