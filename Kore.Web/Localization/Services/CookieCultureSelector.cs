using System.Web;

namespace Kore.Web.Localization.Services
{
    public class CookieCultureSelector : ICultureSelector
    {
        public CultureSelectorResult GetCulture(HttpContextBase context)
        {
            var cookie = context.Request.Cookies["CurrentCulture"];
            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                return new CultureSelectorResult { Priority = -4, CultureCode = cookie.Value };
            }
            return null;
        }
    }
}