//using System.Web;
//using Kore.Composition;
//using Kore.Infrastructure;
//using Kore.Localization;
//using Kore.Web.Configuration;

//namespace Kore.Web.Localization
//{
//    public class DefaultWebCultureManager : DefaultCultureManager, IWebCultureManager
//    {
//        #region IWebCultureManager Members

//        public virtual string GetCurrentCulture(HttpContextBase requestContext)
//        {
//            var languageService = EngineContext.Current.Resolve<ILanguageService>();

//            var cookie = requestContext.Request.Cookies["CurrentCulture"];
//            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
//            {
//                var language = languageService.GetLanguage(cookie.Value);
//                if (language != null)
//                {
//                    return language.CultureCode;
//                }
//            }

//            //TODO
//            //var siteSettings = EngineContext.Current.Resolve<SiteSettings>();
//            //if (!string.IsNullOrEmpty(siteSettings.DefaultLanguage))
//            //{
//            //    var language = languageService.GetLanguage(siteSettings.DefaultLanguage);
//            //    if (language != null)
//            //    {
//            //        return language.CultureCode;
//            //    }
//            //}

//            return "en-US";
//        }

//        #endregion IWebCultureManager Members
//    }
//}