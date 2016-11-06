using System.Collections.Generic;
using System.Linq;
using Kore.Web.ContentManagement.Areas.Admin.Messaging;

namespace Kore.Web.Identity.Messaging
{
    public class AccountMessageTemplateTokensProvider : IMessageTemplateTokensProvider
    {
        #region IMessageTemplateTokensProvider Members

        public IEnumerable<string> GetTokens(string templateName)
        {
            switch (templateName)
            {
                case AccountMessageTemplates.Account_Confirmed:
                    return new[]
                    {
                        "[UserName]",
                        "[Email]"
                    };

                case AccountMessageTemplates.Account_PasswordReset:
                    return new[]
                    {
                        "[UserName]",
                        "[Email]"
                    };

                case AccountMessageTemplates.Account_ProfileChanged:
                    return Enumerable.Empty<string>();

                case AccountMessageTemplates.Account_Registered:
                    return new[]
                    {
                        "[UserName]",
                        "[Email]",
                        "[ConfirmationToken]"
                    };

                default: return Enumerable.Empty<string>();
            }
        }

        #endregion IMessageTemplateTokensProvider Members
    }
}