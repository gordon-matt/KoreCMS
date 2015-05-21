namespace Kore.Plugins.Ecommerce.Simple.Models
{
    public class CreateOrderModel
    {
        public AddressModel BillingAddress { get; set; }

        public AddressModel ShippingAddress { get; set; }
    }
}