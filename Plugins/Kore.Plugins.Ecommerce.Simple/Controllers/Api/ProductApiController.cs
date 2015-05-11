using System.Web.Http;
using Kore.Data;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Ecommerce.Simple.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class ProductApiController : GenericODataController<Product, int>
    {
        public ProductApiController(IRepository<Product> repository)
            : base(repository)
        {
        }

        protected override int GetId(Product entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Product entity)
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