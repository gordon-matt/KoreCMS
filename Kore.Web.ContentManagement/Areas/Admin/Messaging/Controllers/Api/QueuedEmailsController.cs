using System;
using System.Web.Http;
using System.Web.Http.Cors;
using Kore.Data;
using Kore.Web.ContentManagement.Messaging.Domain;
using Kore.Web.Http.OData;

namespace Kore.Web.ContentManagement.Areas.Admin.Messaging.Controllers.Api
{
    [Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class QueuedEmailsController : GenericODataController<QueuedEmail, Guid>
    {
        public QueuedEmailsController(IRepository<QueuedEmail> repository)
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
    }
}