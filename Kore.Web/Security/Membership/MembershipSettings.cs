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

        [LocalizedDisplayName(KoreWebLocalizableStrings.MembershipSettings.GeneratedPasswordLength)]
        public byte GeneratedPasswordLength { get; set; }

        [LocalizedDisplayName(KoreWebLocalizableStrings.MembershipSettings.GeneratedPasswordNumberOfNonAlphanumericChars)]
        public byte GeneratedPasswordNumberOfNonAlphanumericChars { get; set; }

        #region ISettings Members

        public string Name
        {
            get { return "Membership Settings"; }
        }

        public string EditorTemplatePath
        {
            get { return "Kore.Web.Views.Shared.EditorTemplates.MembershipSettings"; }
        }

        #endregion ISettings Members
    }
}