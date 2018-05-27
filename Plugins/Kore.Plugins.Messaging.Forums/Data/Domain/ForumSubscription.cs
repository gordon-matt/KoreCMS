using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Web.Plugins;

namespace Kore.Plugins.Messaging.Forums.Data.Domain
{
    public class ForumSubscription : IEntity
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int ForumId { get; set; }

        public int TopicId { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class ForumSubscriptionMap : EntityTypeConfiguration<ForumSubscription>, IEntityTypeConfiguration
    {
        public ForumSubscriptionMap()
        {
            this.ToTable(Constants.Tables.Subscriptions);
            this.HasKey(x => x.Id);
            this.Property(x => x.UserId).IsRequired().HasMaxLength(128).IsUnicode(true);
            this.Property(x => x.ForumId).IsRequired();
            this.Property(x => x.TopicId).IsRequired();
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return PluginManager.IsPluginInstalled(Constants.PluginSystemName); }
        }

        #endregion IEntityTypeConfiguration Members
    }
}