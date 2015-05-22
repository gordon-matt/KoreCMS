using System.Collections.Generic;
using Kore.ComponentModel;
using Kore.Infrastructure;
using Kore.Security.Membership;
using Kore.Web.Security.Membership;

namespace Kore.Web.Mvc.Themes
{
    public class ThemeUserProfileProvider : IUserProfileProvider
    {
        public class Fields
        {
            public const string PreferredTheme = "PreferredTheme";
        }

        [LocalizedDisplayName(KoreWebLocalizableStrings.UserProfile.Theme.PreferredTheme)]
        public string PreferredTheme { get; set; }

        #region IUserProfileProvider Members

        public string Name
        {
            get { return "Theme"; }
        }

        public string DisplayTemplatePath
        {
            get { return "Kore.Web.Views.Shared.DisplayTemplates.ThemeUserProfileProvider"; }
        }

        public string EditorTemplatePath
        {
            get { return "Kore.Web.Views.Shared.EditorTemplates.ThemeUserProfileProvider"; }
        }

        public int Order
        {
            get { return 9999; }
        }

        public IEnumerable<string> GetFieldNames()
        {
            return new[]
            {
                Fields.PreferredTheme
            };
        }

        public void PopulateFields(string userId)
        {
            var membershipService = EngineContext.Current.Resolve<IMembershipService>();
            PreferredTheme = membershipService.GetProfileEntry(userId, Fields.PreferredTheme);
        }

        #endregion IUserProfileProvider Members
    }
}