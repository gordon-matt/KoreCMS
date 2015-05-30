using System.Collections.Generic;
using Kore.Localization;

namespace Kore.Plugins.Widgets.Google.Infrastructure
{
    public class LanguagePackInvariant : ILanguagePack
    {
        #region ILanguagePack Members

        public string CultureCode
        {
            get { return null; }
        }

        public IDictionary<string, string> LocalizedStrings
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { LocalizableStrings.ContentBlocks.AdSenseBlock.AdClient, "Ad Client" },
                    { LocalizableStrings.ContentBlocks.AdSenseBlock.AdSlot, "Ad Slot" },
                    { LocalizableStrings.ContentBlocks.AdSenseBlock.EnableLazyLoadAd, "Lazy Load" },
                    { LocalizableStrings.ContentBlocks.AdSenseBlock.Height, "Height" },
                    { LocalizableStrings.ContentBlocks.AdSenseBlock.Width, "Width" },
                    { LocalizableStrings.ContentBlocks.AnalyticsBlock.AccountNumber, "Account Number" },
                    { LocalizableStrings.ContentBlocks.AnalyticsBlock.DomainName, "Domain Name" },
                    { LocalizableStrings.ContentBlocks.MapBlock.Height, "Height" },
                    { LocalizableStrings.ContentBlocks.MapBlock.Latitude, "Latitude" },
                    { LocalizableStrings.ContentBlocks.MapBlock.Longitude, "Longitude" },
                    { LocalizableStrings.ContentBlocks.MapBlock.Zoom, "Zoom" },
                    { LocalizableStrings.Google, "Google" },
                };
            }
        }

        #endregion ILanguagePack Members
    }
}