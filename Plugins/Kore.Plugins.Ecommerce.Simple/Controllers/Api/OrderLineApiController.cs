using Kore.Plugins.Ecommerce.Simple.Data.Domain;
using Kore.Plugins.Ecommerce.Simple.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Ecommerce.Simple.Controllers.Api
{
    public class OrderLineApiController : GenericODataController<OrderLine, int>
    {
        public OrderLineApiController(IOrderLineService service)
            : base(service)
        {
        }

        protected override int GetId(OrderLine entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(OrderLine entity)
        {
        }

        protected override Permission ReadPermission
        {
            get { return SimpleCommercePermissions.ReadOrders; }
        }

        protected override Permission WritePermission
        {
            get { return StandardPermissions.FullAccess; }
        }
    }
}