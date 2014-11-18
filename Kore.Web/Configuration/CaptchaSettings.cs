using System;
using System.ComponentModel.DataAnnotations;

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

        [Display(Name = "Public Key")]
        [Required]
        public string PublicKey { get; set; }

        [Display(Name = "Private Key")]
        [Required]
        public string PrivateKey { get; set; }
    }
}