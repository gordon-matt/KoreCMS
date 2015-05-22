using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;

namespace Kore.Plugins.Ecommerce.Simple.Services
{
    public interface IProductImageService : IGenericDataService<ProductImage>
    {
    }

    public class ProductImageService : GenericDataService<ProductImage>, IProductImageService
    {
        public ProductImageService(
            ICacheManager cacheManager,
            IRepository<ProductImage> repository)
            : base(cacheManager, repository)
        {
        }
    }
}