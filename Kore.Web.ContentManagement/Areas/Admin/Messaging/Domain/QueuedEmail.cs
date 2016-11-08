using System;
using System.Data.Entity.ModelConfiguration;
using System.Net.Mail;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Tenants.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Messaging.Domain
{
    public class QueuedEmail : ITenantEntity, IMailMessage
    {
        public Guid Id { get; set; }

        public int? TenantId { get; set; }

        /// <summary>
        /// Gets or sets the priority
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the From Address property
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        /// Gets or sets the FromName property
        /// </summary>
        public string FromName { get; set; }

        /// <summary>
        /// Gets or sets the To property
        /// </summary>
        public string ToAddress { get; set; }

        /// <summary>
        /// Gets or sets the ToName property
        /// </summary>
        public string ToName { get; set; }

        /// <summary>
        /// Gets or sets the subject
        /// </summary>
        public string Subject { get; set; }

        public string MailMessage { get; set; }

        /// <summary>
        /// Gets or sets the date and time of item creation in UTC
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the send tries
        /// </summary>
        public int SentTries { get; set; }

        /// <summary>
        /// Gets or sets the sent date and time
        /// </summary>
        public DateTime? SentOnUtc { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members

        public MailMessage GetMailMessage()
        {
            var wrap = MailMessageWrapper.Create(MailMessage);
            return wrap.ToMailMessage();
        }
    }

    public class QueuedEmailMap : EntityTypeConfiguration<QueuedEmail>, IEntityTypeConfiguration
    {
        public QueuedEmailMap()
        {
            ToTable(CmsConstants.Tables.QueuedEmails);
            HasKey(x => x.Id);
            Property(x => x.Priority).IsRequired();
            Property(x => x.FromAddress).HasMaxLength(255).IsUnicode(true);
            Property(x => x.FromName).HasMaxLength(255).IsUnicode(true);
            Property(x => x.ToAddress).IsRequired().HasMaxLength(255).IsUnicode(true);
            Property(x => x.ToName).HasMaxLength(255).IsUnicode(true);
            Property(x => x.Subject).HasMaxLength(255).IsUnicode(true);
            Property(x => x.MailMessage).IsRequired().IsMaxLength().IsUnicode(true);
            Property(x => x.CreatedOnUtc).IsRequired();
            Property(x => x.SentTries).IsRequired();
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}