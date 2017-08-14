using System.Web.Mvc;
using System.Web.Routing;

namespace HideMessage
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Home",
                url: "Home/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

			routes.MapRoute(
				name: "Login",
				url: "Login/{action}/{id}",
				defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "Detail",
				url: "Detail/{id}",
				defaults: new { controller = "Detail", action = "Index", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "Default",
				url: "",
				defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
			);
        }
    }
}
