using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Domain
{
    public class EntityTypeContentBlock : IEntity
    {
        public Guid Id { get; set; }

        public string EntityType { get; set; }

        public string EntityId { get; set; }

        public string BlockName { get; set; }

        public string BlockType { get; set; }

        public string Title { get; set; }

        public Guid ZoneId { get; set; }

        public int Order { get; set; }

        public bool IsEnabled { get; set; }

        public string BlockValues { get; set; }

        public string CustomTemplatePath { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class EntityTypeContentBlockMap : EntityTypeConfiguration<EntityTypeContentBlock>, IEntityTypeConfiguration
    {
        public EntityTypeContentBlockMap()
        {
            ToTable(CmsConstants.Tables.EntityTypeContentBlocks);
            HasKey(x => x.Id);
            Property(x => x.EntityType).IsRequired().HasMaxLength(512).IsUnicode(false);
            Property(x => x.EntityId).IsRequired().HasMaxLength(50).IsUnicode(false);
            Property(x => x.BlockName).IsRequired().HasMaxLength(255).IsUnicode(true);
            Property(x => x.BlockType).IsRequired().HasMaxLength(1024).IsUnicode(false);
            Property(x => x.Title).IsRequired().HasMaxLength(255).IsUnicode(true);
            Property(x => x.ZoneId).IsRequired();
            Property(x => x.Order).IsRequired();
            Property(x => x.IsEnabled).IsRequired();
            Property(x => x.BlockValues).IsMaxLength().IsUnicode(true);
            Property(x => x.CustomTemplatePath).HasMaxLength(255).IsUnicode(true);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}