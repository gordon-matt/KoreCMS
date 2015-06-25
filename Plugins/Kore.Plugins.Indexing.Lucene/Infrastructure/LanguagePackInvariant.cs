using System.Collections.Generic;
using Kore.Localization;

namespace Kore.Plugins.Indexing.Lucene.Infrastructure
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
                    { LocalizableStrings.LuceneSearchBlock.CustomTemplatePath, "Custom Template Path" },
                    { LocalizableStrings.LuceneSearchBlock.Style, "Style" },
                    { LocalizableStrings.UnexpectedIndexType, "Unexpected index type" },
                };
            }
        }

        #endregion ILanguagePack Members
    }
}