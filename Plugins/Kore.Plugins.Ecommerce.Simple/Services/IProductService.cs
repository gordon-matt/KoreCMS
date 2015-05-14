using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;

namespace Kore.Plugins.Ecommerce.Simple.Services
{
    public interface IProductService : IGenericDataService<Product>
    {
    }

    public class ProductService : GenericDataService<Product>, IProductService
    {
        public ProductService(ICacheManager cacheManager, IRepository<Product> repository)
            : base(cacheManager, repository)
        {
        }
    }
}