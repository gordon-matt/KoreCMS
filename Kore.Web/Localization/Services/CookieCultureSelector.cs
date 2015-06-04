using System.Web;

namespace Kore.Web.Localization.Services
{
    public class CookieCultureSelector : ICultureSelector
    {
        public CultureSelectorResult GetCulture(HttpContextBase context)
        {
            var cookie = context.Request.Cookies["CurrentCulture"];
            if (cookie != null)
            {
                // we allow null or empty value (so we can work with invariant culture for editing pages, etc)
                return new CultureSelectorResult { Priority = -4, CultureCode = cookie.Value };
            }
            return null;
        }
    }
}