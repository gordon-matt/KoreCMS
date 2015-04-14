using System.Collections.Generic;
using Kore.Localization;

namespace Kore.Plugins.Widgets.Google.Infrastructure
{
    public class DefaultLocalizableStringsProvider : IDefaultLocalizableStringsProvider
    {
        #region IDefaultLocalizableStringsProvider Members

        public ICollection<Translation> GetTranslations()
        {
            return new[]
            {
                new Translation
                {
                    CultureCode = null,
                    LocalizedStrings = new Dictionary<string, string>
                    {
                        { LocalizableStrings.Google, "Google" },
                        { LocalizableStrings.XMLSitemap, "XML Sitemap" }
                    }
                }
            };
        }

        #endregion IDefaultLocalizableStringsProvider Members
    }
}