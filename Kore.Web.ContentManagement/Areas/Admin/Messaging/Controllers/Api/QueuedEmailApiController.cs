using System;
using Kore.Data;
using Kore.Web.ContentManagement.Messaging.Domain;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Messaging.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class QueuedEmailApiController : GenericODataController<QueuedEmail, Guid>
    {
        public QueuedEmailApiController(IRepository<QueuedEmail> repository)
            : base(repository)
        {
        }

        protected override Guid GetId(QueuedEmail entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(QueuedEmail entity)
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