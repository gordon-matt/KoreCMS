using System.Web;

namespace Kore.Web.Localization.Services
{
    public interface ICultureSelector
    {
        CultureSelectorResult GetCulture(HttpContextBase context);
    }
}