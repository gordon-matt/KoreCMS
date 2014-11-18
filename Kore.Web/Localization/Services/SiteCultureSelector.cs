//using System.Web;
//using Kore.Web.Configuration;

//namespace Kore.Web.Localization.Services
//{
//    public class SiteCultureSelector : ICultureSelector
//    {
//        private readonly SiteSettings siteSettings;

//        public SiteCultureSelector(SiteSettings siteSettings)
//        {
//            this.siteSettings = siteSettings;
//        }

//        public CultureSelectorResult GetCulture(HttpContextBase context)
//        {
//            var cultureCode = siteSettings.DefaultLanguage;
//            return string.IsNullOrEmpty(cultureCode)
//                ? null
//                : new CultureSelectorResult { Priority = -5, CultureCode = cultureCode };
//        }
//    }
//}