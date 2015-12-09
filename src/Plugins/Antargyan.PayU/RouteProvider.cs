using SmartStore.Web.Framework.Mvc.Routes;
using System.Web.Mvc;
using System.Web.Routing;

namespace Antargyan.PayU
{
    public class RouteProvider : IRouteProvider
    {
        public int Priority
        {
            get
            {
                return 0;
            }
        }

        public void RegisterRoutes(RouteCollection routes)
        {
            RouteCollectionExtensions.MapRoute(routes, "Antargyan.PayU", "Plugins/Antargyan.PayU/{action}", (object)new
            {
                controller = "PayU",
                action = "Index"
            }, new string[1]{
                "Antargyan.PayU.Controllers"
            }).DataTokens["area"] = (object)"Antargyan.PayU";
        }
    }
}
