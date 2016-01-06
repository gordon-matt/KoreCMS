using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Plugins.Messaging.Forums;
using Kore.Web.Plugins;

namespace Kore.Plugins.Messaging.Forums.Data.Domain
{
    public class ForumTopic : IEntity
    {
        public int Id { get; set; }

        public int ForumId { get; set; }

        public string UserId { get; set; }

        public ForumTopicType TopicType { get; set; }

        public string Subject { get; set; }

        public int NumPosts { get; set; }

        public int Views { get; set; }

        public int LastPostId { get; set; }

        public string LastPostUserId { get; set; }

        public DateTime? LastPostTime { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime UpdatedOnUtc { get; set; }

        public virtual Forum Forum { get; set; }

        public int NumReplies
        {
            get { return NumPosts > 0 ? (NumPosts - 1) : 0; }
        }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class ForumTopicMap : EntityTypeConfiguration<ForumTopic>, IEntityTypeConfiguration
    {
        public ForumTopicMap()
        {
            this.ToTable(Constants.Tables.Topics);
            this.HasKey(x => x.Id);
            this.Property(x => x.ForumId).IsRequired();
            this.Property(x => x.UserId).IsRequired().HasMaxLength(128);
            this.Property(x => x.TopicType).IsRequired();
            this.Property(x => x.Subject).IsRequired().HasMaxLength(512);
            this.Property(x => x.NumPosts).IsRequired();
            this.Property(x => x.Views).IsRequired();
            this.Property(x => x.LastPostId).IsRequired();
            this.Property(x => x.LastPostUserId).HasMaxLength(128);
            this.Property(x => x.CreatedOnUtc).IsRequired();
            this.Property(x => x.UpdatedOnUtc).IsRequired();
            this.HasRequired(x => x.Forum).WithMany().HasForeignKey(x => x.ForumId);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return PluginManager.IsPluginInstalled(Constants.PluginSystemName); }
        }

        #endregion IEntityTypeConfiguration Members
    }
}