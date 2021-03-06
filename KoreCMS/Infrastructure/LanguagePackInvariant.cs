﻿using System.Collections.Generic;
using Kore.Localization;

namespace KoreCMS.Infrastructure
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
                    { LocalizableStrings.Dashboard.Administration, "Administration" },
                    { LocalizableStrings.Dashboard.Frontend, "Frontend" },
                    { LocalizableStrings.Dashboard.Title, "Dashboard" },
                    { LocalizableStrings.Dashboard.ToggleNavigation, "Toggle Navigation" },
                };
            }
        }

        #endregion ILanguagePack Members
    }
}