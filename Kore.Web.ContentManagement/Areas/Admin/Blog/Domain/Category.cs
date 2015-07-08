using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kore.Data;
using Kore.Data.EntityFramework;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Domain
{
    public class Category : IEntity
    {
        private ICollection<Post> posts;

        public int Id { get; set; }

        public string Name { get; set; }

        public string UrlSlug { get; set; }

        public ICollection<Post> Posts
        {
            get { return posts ?? (posts = new HashSet<Post>()); }
            set { posts = value; }
        }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class CategoryMap : EntityTypeConfiguration<Category>, IEntityTypeConfiguration
    {
        public CategoryMap()
        {
            ToTable(CmsConstants.Tables.BlogCategories);
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
