using System.Web;

//using Kore.DI;

namespace Kore.Web.Localization.Services
{
    public interface ICultureSelector //: IDependency
    {
        CultureSelectorResult GetCulture(HttpContextBase context);
    }
}