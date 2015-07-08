using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Domain
{
    public class Post : IEntity
    {
        private ICollection<PostTag> tags;

        public Guid Id { get; set; }

        public string UserId { get; set; }

        public DateTime DateCreatedUtc { get; set; }

        public int CategoryId { get; set; }

        public string Headline { get; set; }

        public string Slug { get; set; }

        public string TeaserImageUrl { get; set; }

        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

        public bool UseExternalLink { get; set; }

        public string ExternalLink { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<PostTag> Tags
        {
            get { return tags ?? (tags = new HashSet<PostTag>()); }
            set { tags = value; }
        }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class PostMap : EntityTypeConfiguration<Post>, IEntityTypeConfiguration
    {
        public PostMap()
        {
            ToTable(CmsConstants.Tables.BlogPosts);
            HasKey(x => x.Id);
            Property(x => x.UserId).HasMaxLength(255).IsRequired();
            Property(x => x.DateCreatedUtc).IsRequired();
            Property(x => x.CategoryId).IsRequired();
            Property(x => x.Headline).HasMaxLength(128).IsRequired();
            Property(x => x.Slug).HasMaxLength(128).IsRequired();
            Property(x => x.TeaserImageUrl).HasMaxLength(255);
            Property(x => x.ShortDescription).IsMaxLength().IsRequired();
            Property(x => x.FullDescription).IsMaxLength();
            Property(x => x.UseExternalLink).IsRequired();
            Property(x => x.ExternalLink).HasMaxLength(255);
            Property(x => x.MetaKeywords).HasMaxLength(255);
            Property(x => x.MetaDescription).HasMaxLength(255);
            HasRequired(x => x.Category).WithMany(x => x.Posts).HasForeignKey(x => x.CategoryId);
            //HasMany(c => c.Tags).WithMany(x => x.Posts).Map(m =>
            //{
            //    m.MapLeftKey("PostId");
            //    m.MapRightKey("TagId");
            //    m.ToTable("Kore_BlogPostTags");
            //});
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}