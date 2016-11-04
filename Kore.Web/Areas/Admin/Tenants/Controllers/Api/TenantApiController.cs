using System.Threading.Tasks;
using System.Web.Http;
using Kore.Security.Membership;
using Kore.Tenants.Domain;
using Kore.Tenants.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;
using System.Linq;

namespace Kore.Web.Areas.Admin.Tenants.Controllers.Api
{
    public class TenantApiController : GenericODataController<Tenant, int>
    {
        private readonly IMembershipService membershipService;

        public TenantApiController(
            ITenantService service,
            IMembershipService membershipService)
            : base(service)
        {
            this.membershipService = membershipService;
        }

        public override async Task<IHttpActionResult> Post(Tenant entity)
        {
            var result = await base.Post(entity);
            int tenantId = entity.Id; // EF should have populated the ID in base.Post()
            await membershipService.EnsureAdminRoleForTenant(tenantId);
            return result;
        }

        protected override int GetId(Tenant entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Tenant entity)
        {
        }

        protected override Permission ReadPermission
        {
            get { return StandardPermissions.FullAccess; }
        }

        protected override Permission WritePermission
        {
            get { return StandardPermissions.FullAccess; }
        }
    }
}