using Kore.ComponentModel;
using Kore.Web.Configuration;

namespace Kore.Web.Security.Membership
{
    public class MembershipSettings : ISettings
    {
        public MembershipSettings()
        {
            GeneratedPasswordLength = 7;
            GeneratedPasswordNumberOfNonAlphanumericChars = 3;
        }

        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Membership.GeneratedPasswordLength)]
        public byte GeneratedPasswordLength { get; set; }

        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Membership.GeneratedPasswordNumberOfNonAlphanumericChars)]
        public byte GeneratedPasswordNumberOfNonAlphanumericChars { get; set; }

        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Membership.DisallowUnconfirmedUserLogin)]
        public bool DisallowUnconfirmedUserLogin { get; set; }

        #region ISettings Members

        public string Name => "Membership Settings";

        public bool IsTenantRestricted => false;

        public string EditorTemplatePath => "Kore.Web.Views.Shared.EditorTemplates.MembershipSettings";

        #endregion ISettings Members
    }
}