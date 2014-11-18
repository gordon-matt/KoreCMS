//using System;
//using Kore.Composition;
//using Kore.Localization.Domain;
//using Kore.Web.Configuration;
//using Kore.Web.Events;

//namespace Kore.Web.Localization
//{
//    public class LanguageContentHandler : IContentHandler<Language>
//    {
//        private readonly SiteSettings siteSettings;

//        public LanguageContentHandler(SiteSettings siteSettings)
//        {
//            this.siteSettings = siteSettings;
//        }

//        #region Implementation of IContentHandler<Language>

//        public void Creating(CreateContentContext<Language> context)
//        {
//        }

//        public void Created(CreateContentContext<Language> context)
//        {
//        }

//        public void Updating(UpdateContentContext<Language> context)
//        {
//        }

//        public void Updated(UpdateContentContext<Language> context)
//        {
//        }

//        public void Removing(RemoveContentContext<Language> context)
//        {
//            if (context.ContentItem.CultureCode == siteSettings.DefaultLanguage)
//            {
//                throw new NotSupportedException("Cannot delete default language.");
//            }
//        }

//        public void Removed(RemoveContentContext<Language> context)
//        {
//        }

//        #endregion Implementation of IContentHandler<Language>
//    }
//}