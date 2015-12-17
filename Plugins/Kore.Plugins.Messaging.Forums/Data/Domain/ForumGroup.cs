using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Plugins.Messaging.Forums;
using Kore.Web.Plugins;

namespace Kore.Plugins.Messaging.Forums.Data.Domain
{
    public class ForumGroup : IEntity
    {
        private ICollection<Forum> forums;

        public int Id { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime UpdatedOnUtc { get; set; }

        public virtual ICollection<Forum> Forums
        {
            get { return forums ?? (forums = new List<Forum>()); }
            protected set { forums = value; }
        }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class ForumGroupMap : EntityTypeConfiguration<ForumGroup>, IEntityTypeConfiguration
    {
        public ForumGroupMap()
        {
            this.ToTable(Constants.Tables.Groups);
            this.HasKey(x => x.Id);
            this.Property(x => x.Name).IsRequired().HasMaxLength(255);
            this.Property(x => x.DisplayOrder).IsRequired();
            this.Property(x => x.CreatedOnUtc).IsRequired();
            this.Property(x => x.UpdatedOnUtc).IsRequired();
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return PluginManager.IsPluginInstalled(Constants.PluginSystemName); }
        }

        #endregion IEntityTypeConfiguration Members
    }
}