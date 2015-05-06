using System.Collections.Generic;
using Kore.Localization;

namespace Kore.Plugins.Ecommerce.Simple.Infrastructure
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
                    }
                }
            };
        }

        #endregion IDefaultLocalizableStringsProvider Members
    }
}