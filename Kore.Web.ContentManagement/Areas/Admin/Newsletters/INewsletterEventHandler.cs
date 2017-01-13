using System.Collections.Generic;
using Kore.Security.Membership;
using Kore.Web.ContentManagement.Areas.Admin.Messaging;
using Kore.Web.ContentManagement.Areas.Admin.Messaging.Services;
using Kore.Events;

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
        private readonly IWorkContext workContext;

        public NewsletterEventHandler(IMessageService messageService, IWorkContext workContext)
        {
            this.messageService = messageService;
            this.workContext = workContext;
        }

        #region INewsletterEventHandler Members

        public void Subscribed(KoreUser user)
        {
            var tokens = new List<Token>
            {
                new Token("[UserName]", user.UserName),
                new Token("[Email]", user.Email)
            };
            messageService.SendEmailMessage(workContext.CurrentTenant.Id, NewsletterMessageTemplates.Newsletter_Subscribed, tokens, user.Email);
        }

        public void Unsubscribed(KoreUser user)
        {
            var tokens = new List<Token>
            {
                new Token("[UserName]", user.UserName),
                new Token("[Email]", user.Email)
            };
            messageService.SendEmailMessage(workContext.CurrentTenant.Id, NewsletterMessageTemplates.Newsletter_Unsubscribed, tokens, user.Email);
        }

        #endregion INewsletterEventHandler Members
    }
}