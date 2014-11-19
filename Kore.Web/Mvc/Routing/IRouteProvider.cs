using System.Web.Routing;

namespace Kore.Web.Mvc.Routing
{
    public interface IRouteProvider
    {
        void RegisterRoutes(RouteCollection routes);

        int Priority { get; }
    }
}