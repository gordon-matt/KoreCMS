using System;
using Kore.Web.ContentManagement.Areas.Admin.Messaging.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Messaging.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Messaging.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class QueuedEmailApiController : GenericTenantODataController<QueuedEmail, Guid>
    {
        public QueuedEmailApiController(IQueuedEmailService service)
            : base(service)
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
            get { return CmsPermissions.QueuedEmailsRead; }
        }

        protected override Permission WritePermission
        {
            get { return CmsPermissions.QueuedEmailsWrite; }
        }
    }
}