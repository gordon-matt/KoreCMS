using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;

namespace Kore.Plugins.Ecommerce.Simple.Services
{
    public interface ICategoryService : IGenericDataService<SimpleCommerceCategory>
    {
    }

    public class CategoryService : GenericDataService<SimpleCommerceCategory>, ICategoryService
    {
        public CategoryService(ICacheManager cacheManager, IRepository<SimpleCommerceCategory> repository)
            : base(cacheManager, repository)
        {
        }
    }
}