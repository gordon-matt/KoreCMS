using Kore.ComponentModel;
using Kore.Web.Configuration;

namespace Kore.Web.Security
{
    public class SecuritySettings : ISettings
    {
        public SecuritySettings()
        {
            EnableXsrfProtectionForAdmin = true;
            EnableXsrfProtectionForFrontend = true;
        }

        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Security.EnableXsrfProtectionForAdmin)]
        public bool EnableXsrfProtectionForAdmin { get; set; }

        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Security.EnableXsrfProtectionForFrontend)]
        public bool EnableXsrfProtectionForFrontend { get; set; }

        #region ISettings Members

        public string Name => "Security Settings";

        public bool IsTenantRestricted => true;

        public string EditorTemplatePath => "Kore.Web.Views.Shared.EditorTemplates.SecuritySettings";

        #endregion ISettings Members
    }
}