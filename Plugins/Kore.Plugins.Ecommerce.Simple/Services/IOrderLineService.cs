using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;

namespace Kore.Plugins.Ecommerce.Simple.Services
{
    public interface IOrderLineService : IGenericDataService<OrderLine>
    {
    }

    public class OrderLineService : GenericDataService<OrderLine>, IOrderLineService
    {
        public OrderLineService(ICacheManager cacheManager, IRepository<OrderLine> repository)
            : base(cacheManager, repository)
        {
        }
    }
}