using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;

namespace Kore.Plugins.Ecommerce.Simple.Services
{
    public interface IOrderNoteService : IGenericDataService<OrderNote>
    {
    }

    public class OrderNoteService : GenericDataService<OrderNote>, IOrderNoteService
    {
        public OrderNoteService(
            ICacheManager cacheManager,
            IRepository<OrderNote> repository)
            : base(cacheManager, repository)
        {
        }
    }
}