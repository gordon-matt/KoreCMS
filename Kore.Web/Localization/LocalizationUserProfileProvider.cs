using System.Collections.Generic;
using Kore.ComponentModel;
using Kore.Infrastructure;
using Kore.Security.Membership;
using Kore.Threading;
using Kore.Web.Security.Membership;

namespace Kore.Web.Localization
{
    public class LocalizationUserProfileProvider : IUserProfileProvider
    {
        public class Fields
        {
            public const string PreferredLanguage = "PreferredLanguage";
        }

        [LocalizedDisplayName(KoreWebLocalizableStrings.UserProfile.Localization.PreferredLanguage)]
        public string PreferredLanguage { get; set; }

        #region IUserProfileProvider Members

        public string Name
        {
            get { return "Localization"; }
        }

        public string DisplayTemplatePath
        {
            get { return "Kore.Web.Views.Shared.DisplayTemplates.LocalizationUserProfileProvider"; }
        }

        public string EditorTemplatePath
        {
            get { return "Kore.Web.Views.Shared.EditorTemplates.LocalizationUserProfileProvider"; }
        }

        public int Order
        {
            get { return 9999; }
        }

        public IEnumerable<string> GetFieldNames()
        {
            return new[]
            {
                Fields.PreferredLanguage
            };
        }

        public void PopulateFields(string userId)
        {
            var membershipService = EngineContext.Current.Resolve<IMembershipService>();
            PreferredLanguage = AsyncHelper.RunSync(() => membershipService.GetProfileEntry(userId, Fields.PreferredLanguage));
        }

        #endregion IUserProfileProvider Members
    }
}