using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Tenants.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Domain
{
    public class BlogCategory : ITenantEntity
    {
        private ICollection<BlogPost> posts;

        public int Id { get; set; }

        public int? TenantId { get; set; }

        public string Name { get; set; }

        public string UrlSlug { get; set; }

        public ICollection<BlogPost> Posts
        {
            get { return posts ?? (posts = new HashSet<BlogPost>()); }
            set { posts = value; }
        }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class CategoryMap : EntityTypeConfiguration<BlogCategory>, IEntityTypeConfiguration
    {
        public CategoryMap()
        {
            ToTable(CmsConstants.Tables.BlogCategories);
            HasKey(x => x.Id);
            Property(x => x.Name).IsRequired().HasMaxLength(255).IsUnicode(true);
            Property(x => x.UrlSlug).IsRequired().HasMaxLength(255).IsUnicode(true);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}
