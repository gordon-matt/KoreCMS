using System.Linq;

namespace Kore.Plugins.Ecommerce.Simple.Models
{
    public class CheckoutModel
    {
        public CheckoutModel()
        {
            Cart = new CartModel();
            BillingAddress = new AddressModel();
            ShippingAddress = new AddressModel();
            ShippingAddressIsSameAsBillingAddress = true;
        }

        public CartModel Cart { get; set; }

        public float SubTotal { get; set; }

        public float ShippingTotal { get; set; }

        public float TaxTotal { get; set; }

        public float GrandTotal { get; set; }

        public AddressModel BillingAddress { get; set; }

        public AddressModel ShippingAddress { get; set; }

        public bool ShippingAddressIsSameAsBillingAddress { get; set; }
    }
}