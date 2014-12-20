using System.Collections.Generic;

namespace Kore.Web.ContentManagement.Areas.Admin.Messaging
{
    public interface IMessageTemplateTokensProvider
    {
        IEnumerable<string> GetTokens(string templateName);
    }
}