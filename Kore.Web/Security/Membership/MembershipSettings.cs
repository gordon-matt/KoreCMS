using System.ComponentModel.DataAnnotations;
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

        [Display(Name = "Length")]
        public byte GeneratedPasswordLength { get; set; }

        [Display(Name = "Number of Non-Alphanumeric Characters")]
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

        #endregion
    }
}