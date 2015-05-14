namespace Kore.Plugins.Ecommerce.Simple.Models
{
    public class PayPalModel
    {
        public string ActionUrl { get; set; }

        public string Command { get; set; }

        public string Business { get; set; }

        public string NoShipping { get; set; }

        public string RedirectUrl { get; set; }

        public string CancelUrl { get; set; }

        public string NotifyUrl { get; set; }

        public string CurrencyCode { get; set; }

        public string ProductName { get; set; }

        public float Amount { get; set; }
    }
}