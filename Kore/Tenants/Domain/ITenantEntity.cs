using Kore.Data;

namespace Kore.Tenants.Domain
{
    public interface ITenantEntity : IEntity
    {
        int? TenantId { get; set; }
    }
}