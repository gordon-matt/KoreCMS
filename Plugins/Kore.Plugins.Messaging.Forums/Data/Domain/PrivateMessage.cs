using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Plugins.Messaging.Forums;
using Kore.Web.Plugins;

namespace Kore.Plugins.Messaging.Forums.Data.Domain
{
    public class PrivateMessage : IEntity
    {
        public int Id { get; set; }

        //public int TenantId { get; set; }

        public string FromUserId { get; set; }

        public string ToUserId { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }

        public bool IsRead { get; set; }

        public bool IsDeletedByAuthor { get; set; }

        public bool IsDeletedByRecipient { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class PrivateMessageMap : EntityTypeConfiguration<PrivateMessage>, IEntityTypeConfiguration
    {
        public PrivateMessageMap()
        {
            this.ToTable(Constants.Tables.PrivateMessages);
            this.HasKey(x => x.Id);
            this.Property(x => x.FromUserId).IsRequired().HasMaxLength(128).IsUnicode(true);
            this.Property(x => x.ToUserId).IsRequired().HasMaxLength(128).IsUnicode(true);
            this.Property(x => x.Subject).IsRequired().HasMaxLength(512).IsUnicode(true);
            this.Property(x => x.Text).IsRequired().IsMaxLength().IsUnicode(true);
            this.Property(x => x.IsRead).IsRequired();
            this.Property(x => x.IsDeletedByAuthor).IsRequired();
            this.Property(x => x.IsDeletedByRecipient).IsRequired();
            this.Property(x => x.CreatedOnUtc).IsRequired();
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return PluginManager.IsPluginInstalled(Constants.PluginSystemName); }
        }

        #endregion IEntityTypeConfiguration Members
    }
}