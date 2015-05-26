using System;
using Kore.Data;
using Kore.Logging.Domain;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Areas.Admin.Log.Controllers.Api
{
    public class LogApiController : GenericODataController<LogEntry, Guid>
    {
        public LogApiController(IRepository<LogEntry> repository)
            : base(repository)
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
    }
}