using System.Collections.Generic;
namespace Kore.Plugins.Ecommerce.Simple.Models
{
    public class PayPalModel
    {
        public bool UseSandboxMode { get; set; }

        public string ActionUrl { get; set; }

        public string Merchant { get; set; }

        //// TODO: Might need to change this to OrderId
        //public int ProductId { get; set; }

        //public string ProductName { get; set; }

        public string CurrencyCode { get; set; }

        //public float Amount { get; set; }

        public IEnumerable<ShoppingCartItem> Items { get; set; }

        public string PaymentCompletedUrl { get; set; }

        public string PaymentCancelledUrl { get; set; }

        public string PaymentNotificationUrl { get; set; }
    }
}