using System.Net.Mail;

namespace Kore.Web.ContentManagement.Messaging
{
    public interface IMailMessage
    {
        MailMessage GetMailMessage();
    }
}