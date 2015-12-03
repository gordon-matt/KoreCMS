using System;
using System.Collections.Generic;
using System.Linq;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.Messaging.Services;

namespace Kore.Web.ContentManagement.Areas.Admin.Messaging
{
    public class MessageTemplatesUpdater : IStartupTask
    {
        #region IStartupTask Members

        public void Execute()
        {
            var providers = EngineContext.Current.Resolve<IEnumerable<IMessageTemplatesProvider>>();
            var templateService = EngineContext.Current.Resolve<IMessageTemplateService>();

            var templates = templateService.Find();
            foreach (var provider in providers)
            {
                foreach (var template in provider.GetTemplates())
                {
                    if (templates.FirstOrDefault(x => x.Name == template.Name && x.OwnerId == template.OwnerId) == null)
                    {
                        var newTemplate = new Domain.MessageTemplate
                        {
                            Id = Guid.NewGuid(),
                            Name = template.Name,
                            OwnerId = template.OwnerId,
                            Subject = template.Subject,
                            Body = template.Body ?? "< Empty >",
                            Enabled = true
                        };
                        templateService.Insert(newTemplate);
                    }
                }
            }
        }

        public int Order
        {
            get { return 9999; }
        }

        #endregion IStartupTask Members
    }
}