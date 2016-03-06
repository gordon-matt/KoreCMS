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
            var workContext = EngineContext.Current.Resolve<IWebWorkContext>();
            var pageVersionService = EngineContext.Current.Resolve<IPageVersionService>();

            if (values[parameterName] != null)
            {
                var permalink = values[parameterName].ToString();

                using (var connection = pageVersionService.OpenConnection())
                {
                    return connection.Query(x => x.Slug == permalink).Any();
                }
            }
            return false;
        }
    }
}