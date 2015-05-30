using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Domain
{
    public class BlogEntry : IEntity
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public DateTime DateCreatedUtc { get; set; }

        public string Headline { get; set; }

        public string Slug { get; set; }

        public string TeaserImageUrl { get; set; }

        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

        public bool UseExternalLink { get; set; }

        public string ExternalLink { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class BlogEntryMap : EntityTypeConfiguration<BlogEntry>, IEntityTypeConfiguration
    {
        public BlogEntryMap()
        {
            ToTable(CmsConstants.Tables.Blog);
            HasKey(x => x.Id);
            Property(x => x.UserId).HasMaxLength(255).IsRequired();
            Property(x => x.DateCreatedUtc).IsRequired();
            Property(x => x.Headline).HasMaxLength(128).IsRequired();
            Property(x => x.Slug).HasMaxLength(128).IsRequired();
            Property(x => x.TeaserImageUrl).HasMaxLength(255);
            Property(x => x.ShortDescription).IsMaxLength().IsRequired();
            Property(x => x.FullDescription).IsMaxLength().IsRequired();
            Property(x => x.UseExternalLink).IsRequired();
            Property(x => x.ExternalLink).HasMaxLength(255).IsRequired();
            Property(x => x.MetaKeywords).HasMaxLength(255);
            Property(x => x.MetaDescription).HasMaxLength(255);
        }
    }
}