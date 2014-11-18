using System.Linq;
using System.Web;
using System.Web.Routing;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;

namespace Kore.Web.ContentManagement.Routing
{
    public class CmsRouteConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var pageService = EngineContext.Current.Resolve<IPageService>();

            if (values[parameterName] != null)
            {
                var permalink = values[parameterName].ToString();
                return pageService.Repository.Table.Any(x => x.Slug == permalink);
            }
            return false;
        }
    }
}