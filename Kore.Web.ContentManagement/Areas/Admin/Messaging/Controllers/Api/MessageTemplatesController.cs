using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.OData;
using Kore.Data;
using Kore.Web.ContentManagement.Messaging.Domain;
using Kore.Web.Http.OData;

namespace Kore.Web.ContentManagement.Areas.Admin.Messaging.Controllers.Api
{
    [Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class MessageTemplatesController : GenericODataController<MessageTemplate, Guid>
    {
        private readonly Lazy<IEnumerable<IMessageTemplateTokensProvider>> tokensProviders;

        public MessageTemplatesController(
            IRepository<MessageTemplate> repository,
            Lazy<IEnumerable<IMessageTemplateTokensProvider>> tokensProviders)
            : base(repository)
        {
            this.tokensProviders = tokensProviders;
        }

        protected override Guid GetId(MessageTemplate entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(MessageTemplate entity)
        {
            entity.Id = Guid.NewGuid();
        }

        [HttpPost]
        public virtual IEnumerable<string> GetTokens(ODataActionParameters parameters)
        {
            string templateName = (string)parameters["templateName"];

            return tokensProviders.Value
                .SelectMany(x => x.GetTokens(templateName))
                .Distinct()
                .OrderBy(x => x)
                .ToList();
        }
    }
}