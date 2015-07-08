using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Domain
{
    public class PostTag : IEntity
    {
        public Guid PostId { get; set; }

        public int TagId { get; set; }

        public Post Post { get; set; }

        public Tag Tag { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { PostId, TagId }; }
        }

        #endregion IEntity Members
    }

    public class PostTagMap : EntityTypeConfiguration<PostTag>, IEntityTypeConfiguration
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