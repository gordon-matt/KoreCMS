using System;
using System.Collections.Generic;
using System.Linq;
using Kore.Infrastructure;
using Kore.Tenants.Services;
using Kore.Web.ContentManagement.Areas.Admin.Messaging.Services;

namespace Kore.Web.ContentManagement.Areas.Admin.Messaging
{
    public class MessageTemplatesUpdater : IStartupTask
    {
        #region IStartupTask Members

        public void Execute()
        {
            var tenantService = EngineContext.Current.Resolve<ITenantService>();
            IEnumerable<int> tenantIds = null;

            using (var connection = tenantService.OpenConnection())
            {
                tenantIds = connection.Query().Select(x => x.Id).ToList();
            }

            var providers = EngineContext.Current.ResolveAll<IMessageTemplatesProvider>();
            var templateService = EngineContext.Current.Resolve<IMessageTemplateService>();

            var templates = templateService.Find();

            var toInsert = new List<Domain.MessageTemplate>();
            foreach (var provider in providers)
            {
                foreach (var template in provider.GetTemplates())
                {
                    foreach (int tenantId in tenantIds)
                    {
                        if (templates.FirstOrDefault(x => x.TenantId == tenantId && x.Name == template.Name && x.OwnerId == template.OwnerId) == null)
                        {
                            var newTemplate = new Domain.MessageTemplate
                            {
                                Id = Guid.NewGuid(),
                                TenantId = tenantId,
                                Name = template.Name,
                                OwnerId = template.OwnerId,
                                Subject = template.Subject,
                                Body = template.Body ?? "< Empty >",
                                Enabled = true
                            };
                            toInsert.Add(newTemplate);
                        }
                    }
                }
            }
            if (toInsert.Any())
            {
                templateService.Insert(toInsert);
            }
        }

        public int Order => 9999;

        #endregion IStartupTask Members
    }
}