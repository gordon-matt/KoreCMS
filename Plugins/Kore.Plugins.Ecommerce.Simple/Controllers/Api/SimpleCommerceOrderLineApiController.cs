using Kore.Plugins.Ecommerce.Simple.Data.Domain;
using Kore.Plugins.Ecommerce.Simple.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Ecommerce.Simple.Controllers.Api
{
    public class SimpleCommerceOrderLineApiController : GenericODataController<SimpleCommerceOrderLine, int>
    {
        public SimpleCommerceOrderLineApiController(IOrderLineService service)
            : base(service)
        {
        }

        protected override int GetId(SimpleCommerceOrderLine entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(SimpleCommerceOrderLine entity)
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