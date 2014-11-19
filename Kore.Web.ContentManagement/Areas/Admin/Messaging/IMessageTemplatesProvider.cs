using System.Collections.Generic;

namespace Kore.Web.ContentManagement.Messaging
{
    public interface IMessageTemplatesProvider : IMessageTokensProvider
    {
        IEnumerable<MessageTemplate> GetTemplates();
    }
}