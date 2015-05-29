using Kore.ComponentModel;
using Kore.Web.Configuration;

namespace Kore.Plugins.Ecommerce.Simple
{
    public class PayPalSettings : ISettings
    {
        public PayPalSettings()
        {
            ProductionUrl = "https://www.paypal.com/us/cgi-bin/webscr";
            SandboxUrl = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            UseSandboxMode = true;
            CurrencyCode = "USD";
        }

        [LocalizedDisplayName(LocalizableStrings.Settings.PayPal.ProductionUrl)]
        public string ProductionUrl { get; set; }

        [LocalizedDisplayName(LocalizableStrings.Settings.PayPal.SandboxUrl)]
        public string SandboxUrl { get; set; }

        [LocalizedDisplayName(LocalizableStrings.Settings.PayPal.UseSandboxMode)]
        public bool UseSandboxMode { get; set; }

        [LocalizedDisplayName(LocalizableStrings.Settings.PayPal.Merchant)]
        public string Merchant { get; set; }

        [LocalizedDisplayName(LocalizableStrings.Settings.PayPal.CurrencyCode)]
        public string CurrencyCode { get; set; }

        [LocalizedDisplayName(LocalizableStrings.Settings.PayPal.PdtToken)]
        public string PdtToken { get; set; }

        #region ISettings Members

        public string Name
        {
            get { return "Simple Commerce: PayPal Settings"; }
        }

        public string EditorTemplatePath
        {
            get { return "/Plugins/Ecommerce.Simple/Views/Shared/EditorTemplates/PayPalSettings.cshtml"; }
        }

        #endregion ISettings Members
    }
}