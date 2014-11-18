using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Web.ContentManagement.Areas.Admin.Media.Models;

namespace Kore.Web.ContentManagement.Areas.Admin.Media.Domain
{
    public class MediaPart : IEntity, IMediaPart
    {
        public Guid Id { get; set; }

        public string Caption { get; set; }

        public string Url { get; set; }

        public int SortOrder { get; set; }

        public Guid MediaPartTypeId { get; set; }

        public int ParentId { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class MediaPartMap : EntityTypeConfiguration<MediaPart>, IEntityTypeConfiguration
    {
        public MediaPartMap()
        {
            ToTable("Kore_MediaParts");
            HasKey(x => x.Id);
            Property(x => x.Caption).HasMaxLength(255);
            Property(x => x.Url).IsRequired().HasMaxLength(2048);
            Property(x => x.SortOrder).IsRequired();
            Property(x => x.MediaPartTypeId).IsRequired();
            Property(x => x.ParentId).IsRequired();
        }
    }
}