namespace Kore.Plugins.Ecommerce.Simple.Data.Domain
{
    public enum OrderStatus : byte
    {
        Pending = 0,
        Processing = 1,
        Completed = 2,
        Cancelled = 3
    }

    public enum PaymentStatus : byte
    {
        Pending = 0,
        Paid = 1,
        Authorized = 2,
        Voided = 3,
        Refunded = 4
    }
}