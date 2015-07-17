using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;

namespace Kore.Plugins.Ecommerce.Simple.Services
{
    public interface IProductImageService : IGenericDataService<SimpleCommerceProductImage>
    {
    }

    public class ProductImageService : GenericDataService<SimpleCommerceProductImage>, IProductImageService
    {
        public ProductImageService(
            ICacheManager cacheManager,
            IRepository<SimpleCommerceProductImage> repository)
            : base(cacheManager, repository)
        {
        }
    }
}