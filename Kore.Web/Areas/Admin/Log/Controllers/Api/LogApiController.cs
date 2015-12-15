using System;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.OData;
using System.Web.Http.Results;
using Kore.Data;
using Kore.Data.Services;
using Kore.Logging.Domain;
using Kore.Web.Areas.Admin.Log.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Areas.Admin.Log.Controllers.Api
{
    public class LogApiController : GenericODataController<LogEntry, Guid>
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
            get { return StandardPermissions.FullAccess; }
        }

        protected override Permission WritePermission
        {
            get { return StandardPermissions.FullAccess; }
        }

        [HttpPost]
        public virtual IHttpActionResult Clear(ODataActionParameters parameters)
        {
            if (!CheckPermission(WritePermission))
            {
                return Unauthorized();
            }

            Service.DeleteAll();

            return Ok();
        }
    }
}