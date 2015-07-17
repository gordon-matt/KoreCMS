using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;

namespace Kore.Plugins.Ecommerce.Simple.Services
{
    public interface IAddressService : IGenericDataService<SimpleCommerceAddress>
    {
    }

    public class AddressService : GenericDataService<SimpleCommerceAddress>, IAddressService
    {
        public AddressService(ICacheManager cacheManager, IRepository<SimpleCommerceAddress> repository)
            : base(cacheManager, repository)
        {
        }
    }
}