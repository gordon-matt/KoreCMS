using System.Collections.Generic;
using Kore.Localization;

namespace Kore.Indexing.Lucene.Infrastructure
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
                        { LocalizableStrings.LuceneSearchBlock.RenderAsBootstrapNavbarForm, "Render as Bootstrap Navbar Form" },
                        { LocalizableStrings.UnexpectedIndexType, "Unexpected index type" },
                    }
                }
            };
        }

        #endregion IDefaultLocalizableStringsProvider Members
    }
}