using System.Net.Mail;
using System.Web.Mvc;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks
{
    public interface IFormBlockProcessor
    {
        void Process(FormCollection formCollection, MailMessage mailMessage);
    }
}