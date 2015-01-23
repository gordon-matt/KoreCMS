using System.Collections.Generic;
using System.Linq;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Security.Membership;
using Kore.Web.Collections;
using Kore.Web.Mvc.RoboUI;
using Kore.Web.Security.Membership;

namespace Kore.Web.Localization
{
    public class LocalizationUserProfileProvider : IUserProfileProvider
    {
        public const string PreferredLanguage = "PreferredLanguage";

        #region IUserProfileProvider Members

        public string Category
        {
            get { return "Localization"; }
        }

        public IEnumerable<string> GetFieldNames()
        {
            return new[]
            {
                PreferredLanguage
            };
        }

        public IEnumerable<RoboControlAttribute> GetFields(string userId, bool onlyPublicProperties)
        {
            if (onlyPublicProperties)
            {
                // if there are many properties and some of them are public, then we can return only the public properties,
                //  but for this provider there is ONLY 1 property and it is not public, so we just return empty an result
                return Enumerable.Empty<RoboControlAttribute>();
            }

            var membershipService = EngineContext.Current.Resolve<IMembershipService>();
            var languageManager = EngineContext.Current.Resolve<ILanguageManager>();

            var languages = languageManager.GetActiveLanguages();

            if (languages.Count() < 2)
            {
                return Enumerable.Empty<RoboControlAttribute>();
            }

            string preferredCultureCode = membershipService.GetProfileEntry(userId, PreferredLanguage);
            string selectedValue = null;

            if (!string.IsNullOrEmpty(preferredCultureCode))
            {
                var preferredLanguage = languages.FirstOrDefault(x => x.CultureCode == preferredCultureCode);

                if (preferredLanguage != null)
                {
                    selectedValue = preferredCultureCode;
                }
            }

            var selectList = languages.ToSelectList(value => value.CultureCode, text => text.Name);

            return new RoboControlAttribute[]
            {
                new RoboChoiceAttribute(RoboChoiceType.DropDownList)
                {
                    Name = PreferredLanguage,
                    LabelText = "Preferred Language",
                    SelectListItems = selectList,
                    PropertyType = typeof(string),
                    Value = selectedValue
                }
            };
        }

        #endregion IUserProfileProvider Members
    }
}