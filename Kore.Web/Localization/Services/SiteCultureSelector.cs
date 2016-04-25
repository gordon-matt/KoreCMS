using System.Web;
using Kore.Infrastructure;
using Kore.Web.Configuration;

namespace Kore.Web.Localization.Services
{
    public class SiteCultureSelector : ICultureSelector
    {
        public CultureSelectorResult GetCulture(HttpContextBase context)
        {
            string cultureCode = EngineContext.Current.Resolve<KoreSiteSettings>().DefaultLanguage;
            return string.IsNullOrEmpty(cultureCode)
                ? null
                : new CultureSelectorResult { Priority = -5, CultureCode = cultureCode };
        }
    }
}