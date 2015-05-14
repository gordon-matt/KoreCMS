namespace Kore.Plugins.Ecommerce.Simple.Data.Domain
{
    public enum OrderStatus : byte
    {
        Pending = 0,
        Processing = 1,
        Completed = 2,
        Cancelled = 3
    }
}