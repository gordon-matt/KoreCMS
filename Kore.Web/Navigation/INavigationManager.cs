using System.Collections.Generic;
using System.Web.Routing;

namespace Kore.Web.Navigation
{
    public interface INavigationManager
    {
        IEnumerable<MenuItem> BuildMenu();

        string GetUrl(string menuItemUrl, RouteValueDictionary routeValueDictionary);
    }
}