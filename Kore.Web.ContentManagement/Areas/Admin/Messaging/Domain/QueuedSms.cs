using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;

namespace Kore.Web.ContentManagement.Messaging.Domain
{
    public class QueuedSms : IEntity
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the priority
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the From Number property
        /// </summary>
        public string FromNumber { get; set; }

        /// <summary>
        /// Gets or sets the To Number property
        /// </summary>
        public string ToNumber { get; set; }

        /// <summary>
        /// Gets or sets the Message property
        /// </summary>
        public string Message { get; set; }

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
    }

    public class QueuedSmsMap : EntityTypeConfiguration<QueuedSms>, IEntityTypeConfiguration
    {
        public QueuedSmsMap()
        {
            ToTable("Kore_QueuedSMS");
            HasKey(x => x.Id);
            Property(x => x.Priority).IsRequired();
            Property(x => x.FromNumber).HasMaxLength(30);
            Property(x => x.ToNumber).HasMaxLength(30).IsRequired();
            Property(x => x.Message).IsRequired();
            Property(x => x.CreatedOnUtc).IsRequired();
            Property(x => x.SentTries).IsRequired();
        }
    }
}