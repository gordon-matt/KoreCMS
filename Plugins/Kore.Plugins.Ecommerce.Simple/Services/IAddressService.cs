using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;

namespace Kore.Plugins.Ecommerce.Simple.Services
{
    public interface IAddressService : IGenericDataService<Address>
    {
    }

    public class AddressService : GenericDataService<Address>, IAddressService
    {
        public AddressService(ICacheManager cacheManager, IRepository<Address> repository)
            : base(cacheManager, repository)
        {
        }
    }
}