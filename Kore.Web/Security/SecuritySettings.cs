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

        public string Name
        {
            get { return "Security Settings"; }
        }

        public bool IsTenantRestricted
        {
            get { return true; }
        }

        public string EditorTemplatePath
        {
            get { return "Kore.Web.Views.Shared.EditorTemplates.SecuritySettings"; }
        }

        #endregion ISettings Members
    }
}