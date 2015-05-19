using Kore.Data;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Ecommerce.Simple.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class CategoryApiController : GenericODataController<Category, int>
    {
        public CategoryApiController(IRepository<Category> repository)
            : base(repository)
        {
        }

        protected override int GetId(Category entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Category entity)
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