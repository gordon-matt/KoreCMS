using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;

namespace Kore.Plugins.Ecommerce.Simple.Services
{
    public interface IProductService : IGenericDataService<SimpleCommerceProduct>
    {
    }

    public class ProductService : GenericDataService<SimpleCommerceProduct>, IProductService
    {
        public ProductService(
            ICacheManager cacheManager,
            IRepository<SimpleCommerceProduct> repository)
            : base(cacheManager, repository)
        {
        }
    }
}