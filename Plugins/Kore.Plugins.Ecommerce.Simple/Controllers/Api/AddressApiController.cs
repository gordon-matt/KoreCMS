using Kore.Data;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Ecommerce.Simple.Controllers.Api
{
    public class AddressApiController : GenericODataController<Address, int>
    {
        public AddressApiController(IRepository<Address> repository)
            : base(repository)
        {
        }

        protected override int GetId(Address entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Address entity)
        {
        }

        protected override Permission ReadPermission
        {
            get { return SimpleCommercePermissions.ReadAddresses; }
        }

        protected override Permission WritePermission
        {
            get { return SimpleCommercePermissions.WriteAddresses; }
        }
    }
}