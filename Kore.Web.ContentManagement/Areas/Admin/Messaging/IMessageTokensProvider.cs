using System.Collections.Generic;

namespace Kore.Web.ContentManagement.Messaging
{
    public interface IMessageTokensProvider
    {
        IEnumerable<string> GetAvailableTokens(string template);

        void GetTokens(string template, IEnumerable<Token> tokens);
    }
}