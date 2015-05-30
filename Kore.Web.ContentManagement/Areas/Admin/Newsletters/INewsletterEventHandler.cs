using System.Collections.Generic;
using Kore.Security.Membership;
using Kore.Web.ContentManagement.Areas.Admin.Messaging;
using Kore.Web.ContentManagement.Areas.Admin.Messaging.Services;
using Kore.Web.Events;

namespace Kore.Web.ContentManagement.Areas.Admin.Newsletters
{
    public interface INewsletterEventHandler : IEventHandler
    {
        void Subscribed(KoreUser user);

        void Unsubscribed(KoreUser user);
    }

    public class NewsletterEventHandler : INewsletterEventHandler
    {
        private readonly IMessageService messageService;

        public NewsletterEventHandler(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        #region INewsletterEventHandler Members

        public void Subscribed(KoreUser user)
        {
            var tokens = new List<Token>
            {
                new Token("[UserName]", user.UserName),
                new Token("[Email]", user.Email)
            };
            messageService.SendEmailMessage(NewsletterMessageTemplates.Newsletter_Subscribed, tokens, user.Email);
        }

        public void Unsubscribed(KoreUser user)
        {
            var tokens = new List<Token>
            {
                new Token("[UserName]", user.UserName),
                new Token("[Email]", user.Email)
            };
            messageService.SendEmailMessage(NewsletterMessageTemplates.Newsletter_Unsubscribed, tokens, user.Email);
        }

        #endregion INewsletterEventHandler Members
    }
}