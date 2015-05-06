using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;

namespace Kore.Web.ContentManagement.Areas.Admin.Media.Domain
{
    public class ImageEntity : IEntity
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

    public class MediaPartTypeMap : EntityTypeConfiguration<ImageEntity>, IEntityTypeConfiguration
    {
        public MediaPartTypeMap()
        {
            ToTable("Kore_MediaPartTypes");
            HasKey(x => x.Id);
            Property(x => x.Type).IsRequired().HasMaxLength(2048);
        }
    }
}