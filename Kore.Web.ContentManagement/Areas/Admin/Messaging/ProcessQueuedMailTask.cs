using System;
using System.Linq;
using Kore.Infrastructure;
using Kore.Logging;
using Kore.Net.Mail;
using Kore.Tasks;

namespace Kore.Web.ContentManagement.Areas.Admin.Messaging
{
    public class ProcessQueuedMailTask : ITask
    {
        #region ITask Members

        public string Name
        {
            get { return "Process Queued Mail Task"; }
        }

        public int DefaultInterval
        {
            get { return 600; }
        }

        public void Execute()
        {
            int maxTries;
            int messagesPerBatch;

            var smtpSettings = EngineContext.Current.Resolve<SmtpSettings>();
            if (!string.IsNullOrEmpty(smtpSettings.Host))
            {
                maxTries = smtpSettings.MaxTries;
                messagesPerBatch = smtpSettings.MessagesPerBatch;
            }
            else
            {
                maxTries = 3;
                messagesPerBatch = 50;
            }

            var emailSender = EngineContext.Current.Resolve<IEmailSender>();
            var providers = EngineContext.Current.ResolveAll<IQueuedMessageProvider>();

            //var componentContext = EngineContext.Current.Resolve<IComponentContext>();
            //var logger = componentContext.Resolve<ILogger>(new TypedParameter(typeof(Type), typeof(ProcessQueuedMailTask)));
            var logger = LoggingUtilities.Resolve();

            foreach (var provider in providers)
            {
                var queuedEmails = provider.GetQueuedEmails(maxTries, messagesPerBatch);

                if (!queuedEmails.Any())
                {
                    continue;
                }

                foreach (var queuedEmail in queuedEmails)
                {
                    try
                    {
                        var mailMessage = queuedEmail.GetMailMessage();
                        emailSender.Send(mailMessage);
                        provider.OnSendSuccess(queuedEmail);
                    }
                    catch (Exception x)
                    {
                        logger.Error(string.Format("Error sending e-mail. {0}", x.Message), x);
                        provider.OnSendError(queuedEmail);
                    }
                }
            }
        }

        #endregion ITask Members
    }
}