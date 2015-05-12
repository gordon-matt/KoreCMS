using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                        { LocalizableStrings.UnexpectedIndexType, "Unexpected index type" },
                    }
                }
            };
        }

        #endregion IDefaultLocalizableStringsProvider Members
    }
}
