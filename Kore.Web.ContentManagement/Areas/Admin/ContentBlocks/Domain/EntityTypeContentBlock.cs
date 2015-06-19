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
            Property(x => x.EntityType).HasMaxLength(512).HasColumnType("varchar").IsRequired();
            Property(x => x.EntityId).HasMaxLength(50).HasColumnType("varchar").IsRequired();
            Property(x => x.BlockName).HasMaxLength(255).IsRequired();
            Property(x => x.BlockType).HasMaxLength(1024).HasColumnType("varchar").IsRequired();
            Property(x => x.Title).HasMaxLength(255).IsRequired();
            Property(x => x.ZoneId).IsRequired();
            Property(x => x.Order).IsRequired();
            Property(x => x.IsEnabled).IsRequired();
            Property(x => x.BlockValues).IsMaxLength().IsRequired();
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}