using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;

namespace Kore.Plugins.Ecommerce.Simple.Services
{
    public interface IOrderNoteService : IGenericDataService<SimpleCommerceOrderNote>
    {
    }

    public class OrderNoteService : GenericDataService<SimpleCommerceOrderNote>, IOrderNoteService
    {
        public OrderNoteService(
            ICacheManager cacheManager,
            IRepository<SimpleCommerceOrderNote> repository)
            : base(cacheManager, repository)
        {
        }
    }
}