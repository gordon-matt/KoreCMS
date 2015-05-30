using System.Collections.Generic;

namespace Kore.Web.ContentManagement.Areas.Admin.Messaging
{
    public interface IMessageTemplatesProvider : IMessageTokensProvider
    {
        IEnumerable<MessageTemplate> GetTemplates();
    }
}