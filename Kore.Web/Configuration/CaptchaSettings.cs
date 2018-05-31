using System.ComponentModel.DataAnnotations;
using Kore.ComponentModel;
using Kore.Web.Mvc.Recaptcha;

namespace Kore.Web.Configuration
{
    public class CaptchaSettings : ISettings
    {
        #region ISettings Members

        public string Name => "reCAPTCHA Settings";

        public bool IsTenantRestricted => false;

        public string EditorTemplatePath => "Kore.Web.Views.Shared.EditorTemplates.CaptchaSettings";

        #endregion ISettings Members

        [Required]
        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Captcha.PublicKey)]
        public string PublicKey { get; set; }

        [Required]
        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Captcha.PrivateKey)]
        public string PrivateKey { get; set; }

        [Required]
        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Captcha.Theme)]
        public RecaptchaTheme Theme { get; set; }
    }
}