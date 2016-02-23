using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Plugins.Messaging.Forums;
using Kore.Web.Plugins;

namespace Kore.Plugins.Messaging.Forums.Data.Domain
{
    public class Forum : IEntity
    {
        public int Id { get; set; }

        public int ForumGroupId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int NumTopics { get; set; }

        public int NumPosts { get; set; }

        public int LastTopicId { get; set; }

        public int LastPostId { get; set; }

        public string LastPostUserId { get; set; }

        public DateTime? LastPostTime { get; set; }

        public int DisplayOrder { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime UpdatedOnUtc { get; set; }

        public virtual ForumGroup ForumGroup { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class ForumMap : EntityTypeConfiguration<Forum>, IEntityTypeConfiguration
    {
        public ForumMap()
        {
            this.ToTable(Constants.Tables.Forums);
            this.HasKey(x => x.Id);
            this.Property(x => x.ForumGroupId).IsRequired();
            this.Property(x => x.Name).IsRequired().HasMaxLength(255).IsUnicode(true);
            this.Property(x => x.Description).IsMaxLength().IsUnicode(true);
            this.Property(x => x.NumTopics).IsRequired();
            this.Property(x => x.NumPosts).IsRequired();
            this.Property(x => x.LastTopicId).IsRequired();
            this.Property(x => x.LastPostId).IsRequired();
            this.Property(x => x.LastPostUserId).HasMaxLength(128).IsUnicode(true);
            this.Property(x => x.DisplayOrder).IsRequired();
            this.Property(x => x.CreatedOnUtc).IsRequired();
            this.Property(x => x.UpdatedOnUtc).IsRequired();

            this.HasRequired(x => x.ForumGroup).WithMany(x => x.Forums).HasForeignKey(x => x.ForumGroupId);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return PluginManager.IsPluginInstalled(Constants.PluginSystemName); }
        }

        #endregion IEntityTypeConfiguration Members
    }
}