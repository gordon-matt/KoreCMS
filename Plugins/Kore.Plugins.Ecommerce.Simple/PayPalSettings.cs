using System;
using System.ComponentModel.DataAnnotations;
using Kore.Web.Configuration;

namespace Kore.Plugins.Ecommerce.Simple
{
    public class PayPalSettings : ISettings
    {
        public PayPalSettings()
        {
            ProductionUrl = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            SandboxUrl = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            UseSandboxMode = true;
            CurrencyCode = "USD";
        }

        [Display(Name = "Production URL")]
        public string ProductionUrl { get; set; }

        [Display(Name = "Sandbox URL")]
        public string SandboxUrl { get; set; }

        [Display(Name = "Use Sandbox Mode")]
        public bool UseSandboxMode { get; set; }

        public string Merchant { get; set; }

        [Display(Name = "Currency Code")]
        public string CurrencyCode { get; set; }

        #region ISettings Members

        public string Name
        {
            get { return "Simple Commerce: PayPal Settings"; }
        }

        public string EditorTemplatePath
        {
            get { return "/Plugins/Plugins.Ecommerce.Simple/Views/Shared/EditorTemplates/PayPalSettings.cshtml"; }
        }

        #endregion ISettings Members
    }
}