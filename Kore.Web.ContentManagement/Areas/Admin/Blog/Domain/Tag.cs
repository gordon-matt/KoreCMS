using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Domain
{
    public class Tag : IEntity
    {
        private ICollection<PostTag> posts;

        public int Id { get; set; }

        public string Name { get; set; }

        public string UrlSlug { get; set; }

        public ICollection<PostTag> Posts
        {
            get { return posts ?? (posts = new HashSet<PostTag>()); }
            set { posts = value; }
        }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class TagMap : EntityTypeConfiguration<Tag>, IEntityTypeConfiguration
    {
        public TagMap()
        {
            ToTable(CmsConstants.Tables.BlogTags);
            HasKey(x => x.Id);
            Property(x => x.Name).HasMaxLength(255).IsRequired();
            Property(x => x.UrlSlug).HasMaxLength(255).IsRequired();
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}