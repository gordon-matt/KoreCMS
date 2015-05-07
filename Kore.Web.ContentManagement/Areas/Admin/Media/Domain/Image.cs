using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Web.ContentManagement.Areas.Admin.Media.Models;

namespace Kore.Web.ContentManagement.Areas.Admin.Media.Domain
{
    public class Image : IEntity, IKoreImage
    {
        public Guid Id { get; set; }

        public Guid EntityTypeId { get; set; }

        public string EntityId { get; set; }

        public string Url { get; set; }

        public string ThumbnailUrl { get; set; }

        public string Caption { get; set; }

        public int SortOrder { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class MediaPartMap : EntityTypeConfiguration<Image>, IEntityTypeConfiguration
    {
        public MediaPartMap()
        {
            ToTable("Kore_MediaParts");
            HasKey(x => x.Id);
            Property(x => x.EntityTypeId).IsRequired();
            Property(x => x.EntityId).IsRequired().HasMaxLength(255);
            Property(x => x.Url).IsRequired().HasMaxLength(2048);
            Property(x => x.ThumbnailUrl).HasMaxLength(2048);
            Property(x => x.Caption).HasMaxLength(255);
            Property(x => x.SortOrder).IsRequired();
        }
    }
}