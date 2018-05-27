using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Web.Plugins;

namespace Kore.Plugins.Messaging.Forums.Data.Domain
{
    public class ForumPost : IEntity
    {
        public int Id { get; set; }

        public int TopicId { get; set; }

        public string UserId { get; set; }

        public string Text { get; set; }

        public string IPAddress { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime UpdatedOnUtc { get; set; }

        public virtual ForumTopic ForumTopic { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class ForumPostMap : EntityTypeConfiguration<ForumPost>, IEntityTypeConfiguration
    {
        public ForumPostMap()
        {
            this.ToTable(Constants.Tables.Posts);
            this.HasKey(x => x.Id);
            this.Property(x => x.TopicId).IsRequired();
            this.Property(x => x.UserId).IsRequired().HasMaxLength(128).IsUnicode(true);
            this.Property(x => x.Text).IsRequired().IsMaxLength().IsUnicode(true);
            this.Property(x => x.IPAddress).HasMaxLength(45).IsUnicode(false);
            this.Property(x => x.CreatedOnUtc).IsRequired();
            this.Property(x => x.UpdatedOnUtc).IsRequired();

            this.HasRequired(x => x.ForumTopic).WithMany().HasForeignKey(x => x.TopicId);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return PluginManager.IsPluginInstalled(Constants.PluginSystemName); }
        }

        #endregion IEntityTypeConfiguration Members
    }
}