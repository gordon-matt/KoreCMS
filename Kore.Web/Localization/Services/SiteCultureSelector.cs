﻿//using System.Web;
//using Kore.Web.Configuration;

//namespace Kore.Web.Localization.Services
//{
//    public class SiteCultureSelector : ICultureSelector
//    {
//        private readonly KoreSiteSettings siteSettings;

//        public SiteCultureSelector(KoreSiteSettings siteSettings)
//        {
//            this.siteSettings = siteSettings;
//        }

//        public CultureSelectorResult GetCulture(HttpContextBase context)
//        {
//            string cultureCode = siteSettings.DefaultLanguage;
//            return string.IsNullOrEmpty(cultureCode)
//                ? null
//                : new CultureSelectorResult { Priority = -5, CultureCode = cultureCode };
//        }
//    }
//}