using System.Collections.Generic;
using Kore.Localization;

namespace Kore.Web.Common.Infrastructure
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
                    { LocalizableStrings.Regions.Cities, "Cities" },
                    { LocalizableStrings.Regions.States, "States" },
                    { LocalizableStrings.Regions.Title, "Regions" },
                    { LocalizableStrings.Regions.Model.CountryCode, "Country Code" },
                    { LocalizableStrings.Regions.Model.HasStates, "Has States" },
                    { LocalizableStrings.Regions.Model.Name, "Name" },
                    { LocalizableStrings.Regions.Model.RegionType, "Region Type" },
                    { LocalizableStrings.Regions.Model.StateCode, "State Code" },
                };
            }
        }

        #endregion ILanguagePack Members
    }
}