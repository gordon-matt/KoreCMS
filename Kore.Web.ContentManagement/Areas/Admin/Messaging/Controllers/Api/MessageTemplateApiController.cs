using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Kore.Data;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;
using Microsoft.AspNet.OData;
using MessageTemplateEntity = Kore.Web.ContentManagement.Areas.Admin.Messaging.Domain.MessageTemplate;

namespace Kore.Web.ContentManagement.Areas.Admin.Messaging.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class MessageTemplateApiController : GenericTenantODataController<MessageTemplateEntity, Guid>
    {
        private readonly Lazy<IEnumerable<IMessageTemplateTokensProvider>> tokensProviders;

        public MessageTemplateApiController(
            IRepository<MessageTemplateEntity> repository,
            Lazy<IEnumerable<IMessageTemplateTokensProvider>> tokensProviders)
            : base(repository)
        {
            this.tokensProviders = tokensProviders;
        }

        protected override Guid GetId(MessageTemplateEntity entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(MessageTemplateEntity entity)
        {
            entity.Id = Guid.NewGuid();
        }

        [HttpPost]
        public virtual IEnumerable<string> GetTokens(ODataActionParameters parameters)
        {
            if (!CheckPermission(ReadPermission))
            {
                return Enumerable.Empty<string>();
            }

            string templateName = (string)parameters["templateName"];

            return tokensProviders.Value
                .SelectMany(x => x.GetTokens(templateName))
                .Distinct()
                .OrderBy(x => x)
                .ToList();
        }

        protected override Permission ReadPermission
        {
            get { return CmsPermissions.MessageTemplatesRead; }
        }

        protected override Permission WritePermission
        {
            get { return CmsPermissions.MessageTemplatesWrite; }
        }
    }
}