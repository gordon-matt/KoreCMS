using System;
using Kore.Data;
using Kore.Web.ContentManagement.Messaging.Domain;
using Kore.Web.Http.OData;

namespace Kore.Web.ContentManagement.Areas.Admin.Messaging.Controllers.Api
{
    public class MessageTemplatesController : GenericODataController<MessageTemplate, Guid>
    {
        public MessageTemplatesController(IRepository<MessageTemplate> repository)
            : base(repository)
        {
        }

        protected override Guid GetId(MessageTemplate entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(MessageTemplate entity)
        {
            entity.Id = Guid.NewGuid();
        }
    }
}