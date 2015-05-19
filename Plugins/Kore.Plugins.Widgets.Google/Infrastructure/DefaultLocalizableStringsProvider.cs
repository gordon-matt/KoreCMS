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
                        { LocalizableStrings.AdSenseBlock.AdClient, "Ad Client" },
                        { LocalizableStrings.AdSenseBlock.AdSlot, "Ad Slot" },
                        { LocalizableStrings.AdSenseBlock.EnableLazyLoadAd, "Lazy Load" },
                        { LocalizableStrings.AdSenseBlock.Height, "Height" },
                        { LocalizableStrings.AdSenseBlock.Width, "Width" },
                        { LocalizableStrings.ConfirmGenerateFile, "Are you sure you want to generate a new XML sitemap file? Warning: This will replace the existing one." },
                        { LocalizableStrings.GenerateFile, "Generate File" },
                        { LocalizableStrings.GenerateFileError, "Error when generating XML sitemap file." },
                        { LocalizableStrings.GenerateFileSuccess, "Successfully generated XML sitemap file." },
                        { LocalizableStrings.Google, "Google" },
                        { LocalizableStrings.XMLSitemap, "XML Sitemap" }
                    }
                }
            };
        }

        #endregion IDefaultLocalizableStringsProvider Members
    }
}