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

        public AddressModel BillingAddress { get; set; }

        public AddressModel ShippingAddress { get; set; }

        public bool ShippingAddressIsSameAsBillingAddress { get; set; }
    }
}