using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Domain
{
    public class BlogPostTag : IEntity
    {
        public Guid PostId { get; set; }

        public int TagId { get; set; }

        public BlogPost Post { get; set; }

        public BlogTag Tag { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { PostId, TagId }; }
        }

        #endregion IEntity Members
    }

    public class PostTagMap : EntityTypeConfiguration<BlogPostTag>, IEntityTypeConfiguration
    {
        public PostTagMap()
        {
            ToTable(CmsConstants.Tables.BlogPostTags);
            HasKey(x => new { x.PostId, x.TagId });

            HasRequired(x => x.Post).WithMany(x => x.Tags).HasForeignKey(x => x.PostId).WillCascadeOnDelete(true);
            HasRequired(x => x.Tag).WithMany(x => x.Posts).HasForeignKey(x => x.TagId).WillCascadeOnDelete(true);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}