using System.Collections.Generic;

namespace Kore.Plugins.Ecommerce.Simple.Models
{
    public class PayPalModel
    {
        public bool PassProductNamesAndTotals { get; set; }

        public string Merchant { get; set; }

        public bool UseSandboxMode { get; set; }

        public string ActionUrl { get; set; }

        public string ReturnUrl { get; set; }

        public string CancelReturnUrl { get; set; }

        public string NotificationUrl { get; set; }

        public string CurrencyCode { get; set; }

        public IEnumerable<ShoppingCartItem> Items { get; set; }

        public int OrderId { get; set; }

        public float OrderTotal { get; set; }

        public float SalesTax { get; set; }

        public float ShippingFee { get; set; }

        public AddressModel BillingAddress { get; set; }
    }
}