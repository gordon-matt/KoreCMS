using System.Collections.Generic;
using Kore.Localization;

namespace Kore.Plugins.Messaging.Forums.Infrastructure
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
                    { LocalizableStrings.Forums, "Forums" },
                };
            }
        }

        #endregion ILanguagePack Members
    }
}