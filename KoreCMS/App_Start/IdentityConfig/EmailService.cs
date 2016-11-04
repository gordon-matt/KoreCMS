using System.Threading.Tasks;
using Kore.Infrastructure;
using Kore.Net.Mail;
using Microsoft.AspNet.Identity;

namespace KoreCMS
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            var emailSender = EngineContext.Current.Resolve<IEmailSender>();
            emailSender.Send(message.Subject, message.Body, message.Destination);
            return Task.FromResult(0);
        }
    }
}