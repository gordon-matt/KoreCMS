using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;

namespace Kore.Web.ContentManagement.Areas.Admin.Media.Domain
{
    public class ImageEntityType : IEntity
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class MediaPartTypeMap : EntityTypeConfiguration<ImageEntityType>, IEntityTypeConfiguration
    {
        public MediaPartTypeMap()
        {
            ToTable(CmsConstants.Tables.ImageEntityTypes);
            HasKey(x => x.Id);
            Property(x => x.Type).IsRequired().HasMaxLength(2048);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}