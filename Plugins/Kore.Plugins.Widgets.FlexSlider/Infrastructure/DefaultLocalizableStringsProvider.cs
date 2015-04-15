using System.Collections.Generic;
using Kore.Localization;

namespace Kore.Plugins.Widgets.FlexSlider.Infrastructure
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
                        { LocalizableStrings.FlexSlider, "FlexSlider" },
                        { LocalizableStrings.CategoryGallerySliders, "Category Gallery Sliders" },
                        { LocalizableStrings.CollectionGallerySliders, "Collection Gallery Sliders" },
                        { LocalizableStrings.DefaultSliders, "Default Sliders" },
                        { LocalizableStrings.LinkedSliders, "Linked Sliders" }
                    }
                }
            };
        }

        #endregion IDefaultLocalizableStringsProvider Members
    }
}