using Kore.Data;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;
using Kore.Plugins.Ecommerce.Simple.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Ecommerce.Simple.Controllers.Api
{
    public class SimpleCommerceOrderApiController : GenericODataController<SimpleCommerceOrder, int>
    {
        public SimpleCommerceOrderApiController(IOrderService service)
            : base(service)
        {
        }

        protected override int GetId(SimpleCommerceOrder entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(SimpleCommerceOrder entity)
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