using System.Web.Mvc;
using System.Web.Routing;
using Kore.Web.Mvc.Routing;

namespace Kore.Web.ContentManagement
{
    public class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            // register CMS pages route
            routes.MapRoute(
                name: "CmsRoute",
                url: "{*slug}",
                defaults: new { controller = "PageContent", action = "Index", area = Constants.Areas.Pages, slug = string.Empty }
                //constraints: new { slug = new CmsRouteConstraint() }
            );
        }

        public int Priority
        {
            get
            {
                // make sure CMS pages route gets done last
                return int.MaxValue;
            }
        }
    }
}