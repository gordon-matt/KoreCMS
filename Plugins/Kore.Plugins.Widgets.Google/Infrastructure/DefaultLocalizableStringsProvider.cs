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
                        { LocalizableStrings.SitemapModel.ChangeFrequency, "Change Frequency" },
                        { LocalizableStrings.SitemapModel.Id, "ID" },
                        { LocalizableStrings.SitemapModel.Location, "Location" },
                        { LocalizableStrings.SitemapModel.Priority, "Priority" },
                        { LocalizableStrings.SitemapModel.ChangeFrequencies.Always, "Always" },
                        { LocalizableStrings.SitemapModel.ChangeFrequencies.Daily, "Daily" },
                        { LocalizableStrings.SitemapModel.ChangeFrequencies.Hourly, "Hourly" },
                        { LocalizableStrings.SitemapModel.ChangeFrequencies.Monthly, "Monthly" },
                        { LocalizableStrings.SitemapModel.ChangeFrequencies.Never, "Never" },
                        { LocalizableStrings.SitemapModel.ChangeFrequencies.Weekly, "Weekly" },
                        { LocalizableStrings.SitemapModel.ChangeFrequencies.Yearly, "Yearly" },
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