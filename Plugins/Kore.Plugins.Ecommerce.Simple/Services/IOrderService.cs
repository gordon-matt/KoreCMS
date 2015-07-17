using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;

namespace Kore.Plugins.Ecommerce.Simple.Services
{
    public interface IOrderService : IGenericDataService<SimpleCommerceOrder>
    {
    }

    public class OrderService : GenericDataService<SimpleCommerceOrder>, IOrderService
    {
        public OrderService(
            ICacheManager cacheManager,
            IRepository<SimpleCommerceOrder> repository)
            : base(cacheManager, repository)
        {
        }
    }
}