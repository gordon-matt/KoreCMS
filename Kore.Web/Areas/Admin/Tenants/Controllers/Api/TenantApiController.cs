using System.Threading.Tasks;
using System.Web.Http;
using Kore.Security.Membership;
using Kore.Tenants.Domain;
using Kore.Tenants.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;
using System.Linq;
using System.IO;
using Kore.IO;
using System.Web.Hosting;

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

            //TOOD: Create tenant media folder:
            var mediaFolder = new DirectoryInfo(HostingEnvironment.MapPath("~/Media/Uploads/Tenant_" + tenantId));
            if (!mediaFolder.Exists)
            {
                mediaFolder.Create();
            }

            return result;
        }

        public override async Task<IHttpActionResult> Delete(int key)
        {
            var result = await base.Delete(key);

            //TODO: Remove everything associated with the tenant.

            // TODO: Add some checkbox on admin page... only delete files if user checks that box.
            //var mediaFolder = new DirectoryInfo(HostingEnvironment.MapPath("~/Media/Uploads/Tenant_" + key));
            //if (mediaFolder.Exists)
            //{
            //    mediaFolder.Delete();
            //}

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