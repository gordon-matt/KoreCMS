using System.Collections.Generic;

namespace Kore.Web.ContentManagement.Areas.Admin.Messaging
{
    public interface IMessageTokensProvider
    {
        IEnumerable<string> GetAvailableTokens(string template);

        void GetTokens(string template, IEnumerable<Token> tokens);
    }
}