using System.Net.Mail;

namespace Kore.Web.ContentManagement.Areas.Admin.Messaging
{
    public interface IMailMessage
    {
        MailMessage GetMailMessage();
    }
}