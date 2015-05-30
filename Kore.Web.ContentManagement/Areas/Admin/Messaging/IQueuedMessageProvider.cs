using System.Collections.Generic;

namespace Kore.Web.ContentManagement.Areas.Admin.Messaging
{
    public interface IQueuedMessageProvider
    {
        IEnumerable<IMailMessage> GetQueuedEmails(int maxSendTries, int maxMessageItems);

        void OnSendSuccess(IMailMessage mailMessage);

        void OnSendError(IMailMessage mailMessage);
    }
}