using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;

namespace Kore.Plugins.Ecommerce.Simple.Services
{
    public interface IOrderLineService : IGenericDataService<SimpleCommerceOrderLine>
    {
    }

    public class OrderLineService : GenericDataService<SimpleCommerceOrderLine>, IOrderLineService
    {
        public OrderLineService(ICacheManager cacheManager, IRepository<SimpleCommerceOrderLine> repository)
            : base(cacheManager, repository)
        {
        }
    }
}