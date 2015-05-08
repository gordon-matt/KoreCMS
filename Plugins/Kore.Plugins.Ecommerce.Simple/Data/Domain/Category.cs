using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;

namespace Kore.Plugins.Ecommerce.Simple.Data.Domain
{
    public class Category : IEntity
    {
        private ICollection<Product> products;
        private ICollection<Category> subCategories;

        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string ImageUrl { get; set; }

        public virtual Category Parent { get; set; }

        public virtual ICollection<Product> Products
        {
            get { return products ?? (products = new List<Product>()); }
            set { products = value; }
        }

        public virtual ICollection<Category> SubCategories
        {
            get { return subCategories ?? (subCategories = new List<Category>()); }
            set { subCategories = value; }
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
            ToTable("Kore_Plugins_SimpleCommerce_Categories");
            HasKey(x => x.Id);
            Property(x => x.Name).HasMaxLength(255).IsRequired();
            Property(x => x.Slug).HasMaxLength(255).IsRequired();
            Property(x => x.ImageUrl).HasMaxLength(255);
            HasOptional(x => x.Parent).WithMany(x => x.SubCategories).HasForeignKey(x => x.ParentId);
        }
    }
}