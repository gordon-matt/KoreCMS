using System.Collections.Generic;

namespace Kore.Localization
{
    public interface ILanguagePack
    {
        /// <summary>
        /// Leave NULL for default (invariant) culture
        /// </summary>
        string CultureCode { get; }

        IDictionary<string, string> LocalizedStrings { get; }
    }
}