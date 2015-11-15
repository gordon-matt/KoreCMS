using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Tenants.Domain;

namespace Kore.Tenants.Services
{
    public interface ITenantService : IGenericDataService<Tenant>
    {
    }

    public class TenantService : GenericDataService<Tenant>, ITenantService
    {
        public TenantService(ICacheManager cacheManager, IRepository<Tenant> repository)
            : base(cacheManager, repository)
        {
        }
    }
}