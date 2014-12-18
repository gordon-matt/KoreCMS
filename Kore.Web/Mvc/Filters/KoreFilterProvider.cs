using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kore.Infrastructure;

namespace Kore.Web.Mvc.Filters
{
    public class KoreFilterProvider : System.Web.Mvc.IFilterProvider
    {
        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filterProviders = EngineContext.Current.ResolveAll<IFilterProvider>();
            return filterProviders.Select(x => new Filter(x, FilterScope.Action, null));
        }
    }
}