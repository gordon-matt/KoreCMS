using Kore.ComponentModel;
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

        [LocalizedDisplayName(LocalizableStrings.PayPalSettings.ProductionUrl)]
        public string ProductionUrl { get; set; }

        [LocalizedDisplayName(LocalizableStrings.PayPalSettings.SandboxUrl)]
        public string SandboxUrl { get; set; }

        [LocalizedDisplayName(LocalizableStrings.PayPalSettings.UseSandboxMode)]
        public bool UseSandboxMode { get; set; }

        [LocalizedDisplayName(LocalizableStrings.PayPalSettings.Merchant)]
        public string Merchant { get; set; }

        [LocalizedDisplayName(LocalizableStrings.PayPalSettings.CurrencyCode)]
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