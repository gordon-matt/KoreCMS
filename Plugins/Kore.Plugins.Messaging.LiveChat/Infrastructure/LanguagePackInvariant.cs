﻿using System.Collections.Generic;
using Kore.Localization;

namespace Kore.Plugins.Messaging.LiveChat.Infrastructure
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
                    { LocalizableStrings.Agent, "Agent" },
                    { LocalizableStrings.LiveChat, "Live Chat" },
                    { LocalizableStrings.Setup, "Setup" },
                };
            }
        }

        #endregion ILanguagePack Members
    }
}