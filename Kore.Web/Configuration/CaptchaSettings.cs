using System.ComponentModel.DataAnnotations;
using Kore.ComponentModel;

namespace Kore.Web.Configuration
{
    public class CaptchaSettings : ISettings
    {
        #region ISettings Members

        [ScaffoldColumn(false)]
        public string Name
        {
            get { return "Captcha Settings"; }
        }

        public string EditorTemplatePath
        {
            get { return "Kore.Web.Views.Shared.EditorTemplates.CaptchaSettings"; }
        }

        #endregion ISettings Members

        [Required]
        [LocalizedDisplayName(KoreWebLocalizableStrings.CaptchaSettings.PublicKey)]
        public string PublicKey { get; set; }

        [Required]
        [LocalizedDisplayName(KoreWebLocalizableStrings.CaptchaSettings.PrivateKey)]
        public string PrivateKey { get; set; }
    }
}