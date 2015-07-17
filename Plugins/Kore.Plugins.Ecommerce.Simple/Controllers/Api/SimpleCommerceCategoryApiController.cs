using Kore.Data;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;
using Kore.Plugins.Ecommerce.Simple.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Ecommerce.Simple.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class SimpleCommerceCategoryApiController : GenericODataController<SimpleCommerceCategory, int>
    {
        public SimpleCommerceCategoryApiController(ICategoryService service)
            : base(service)
        {
        }

        protected override int GetId(SimpleCommerceCategory entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(SimpleCommerceCategory entity)
        {
        }

        protected override Permission ReadPermission
        {
            get { return SimpleCommercePermissions.ReadCategories; }
        }

        protected override Permission WritePermission
        {
            get { return SimpleCommercePermissions.WriteCategories; }
        }
    }
}