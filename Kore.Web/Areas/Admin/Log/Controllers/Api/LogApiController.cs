using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using Kore.Logging.Domain;
using Kore.Logging.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Areas.Admin.Log.Controllers.Api
{
    public class LogApiController : GenericTenantODataController<LogEntry, Guid>
    {
        public LogApiController(ILogService service)
            : base(service)
        {
        }

        protected override Guid GetId(LogEntry entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(LogEntry entity)
        {
            entity.Id = Guid.NewGuid();
        }

        protected override Permission ReadPermission
        {
            get { return KoreWebPermissions.LogRead; }
        }

        protected override Permission WritePermission
        {
            get { return StandardPermissions.FullAccess; }
        }

        [HttpPost]
        public virtual async Task<IHttpActionResult> Clear(ODataActionParameters parameters)
        {
            if (!CheckPermission(WritePermission))
            {
                return Unauthorized();
            }

            int tenantId = GetTenantId();
            await Service.DeleteAsync(x => x.TenantId == tenantId);

            return Ok();
        }
    }
}