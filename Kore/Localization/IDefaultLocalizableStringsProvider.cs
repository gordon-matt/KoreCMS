using System.Collections.Generic;

namespace Kore.Localization
{
    public interface IDefaultLocalizableStringsProvider
    {
        ICollection<Translation> GetTranslations();
    }

    public class Translation
    {
        /// <summary>
        /// Leave NULL for default (invariant) culture
        /// </summary>
        public string CultureCode { get; set; }

        public IDictionary<string, string> LocalizedStrings { get; set; }
    }
}