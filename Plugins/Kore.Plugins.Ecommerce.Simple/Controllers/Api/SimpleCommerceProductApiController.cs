using Kore.Data;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;
using Kore.Plugins.Ecommerce.Simple.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Ecommerce.Simple.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class SimpleCommerceProductApiController : GenericODataController<SimpleCommerceProduct, int>
    {
        public SimpleCommerceProductApiController(IProductService service)
            : base(service)
        {
        }

        protected override int GetId(SimpleCommerceProduct entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(SimpleCommerceProduct entity)
        {
        }

        protected override Permission ReadPermission
        {
            get { return SimpleCommercePermissions.ReadProducts; }
        }

        protected override Permission WritePermission
        {
            get { return SimpleCommercePermissions.WriteProducts; }
        }
    }
}