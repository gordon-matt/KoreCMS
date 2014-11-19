using System.Web.Routing;

namespace Kore.Web.Mvc.Routing
{
    public interface IHasRequestContext
    {
        RequestContext RequestContext { get; }
    }
}