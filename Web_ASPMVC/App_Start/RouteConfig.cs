using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web_ASPMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Product Category",
               url: "san-pham/{metatitle}-{cateid}",
               defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
               namespaces: new[] { "Web_ASPMVC.Controllers" }
           );
            routes.MapRoute(
              name: "Product Detail",
              url: "Chi-tiet/{metatitle}-{cateid}",
              defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
              namespaces: new[] { "Web_ASPMVC.Controllers" }
          );
            routes.MapRoute(
           name: "About",
           url: "Ve-chung-toi",
           defaults: new { controller = "About", action = "Index", id = UrlParameter.Optional },
           namespaces: new[] { "Web_ASPMVC.Controllers" }
       );
            routes.MapRoute(
         name: "Services",
         url: "Dich-vu",
         defaults: new { controller = "Services", action = "Index", id = UrlParameter.Optional },
         namespaces: new[] { "Web_ASPMVC.Controllers" }
     );
            routes.MapRoute(
         name: "Cart",
         url: "gio-hang",
         defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
         namespaces: new[] { "Web_ASPMVC.Controllers" }
     );
            routes.MapRoute(
            name: "Add Cart",
            url: "them-gio-hang",
            defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional },
            namespaces: new[] { "Web_ASPMVC.Controllers" }
        );
            //router mặc định
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Web_ASPMVC.Controllers" }
            );
        }
    }
}
