using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Web.Plugins;

namespace Kore.Plugins.Ecommerce.Simple.Data.Domain
{
    public class Product : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public int CategoryId { get; set; }

        public float Price { get; set; }

        public float Tax { get; set; }

        public float ShippingCost { get; set; }

        public string MainImageUrl { get; set; }

        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }

        public virtual Category Category { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class ProductMap : EntityTypeConfiguration<Product>, IEntityTypeConfiguration
    {
        public ProductMap()
        {
            if (!PluginManager.IsPluginInstalled(Constants.PluginSystemName))
            {
                return;
            }

            ToTable(Constants.Tables.Products);
            HasKey(x => x.Id);
            Property(x => x.Name).HasMaxLength(255).IsRequired();
            Property(x => x.Slug).HasMaxLength(255).IsRequired();
            Property(x => x.Price).IsRequired();
            Property(x => x.Tax).IsRequired();
            Property(x => x.ShippingCost).IsRequired();
            Property(x => x.MainImageUrl).HasMaxLength(255);
            Property(x => x.ShortDescription).IsMaxLength().IsRequired();
            Property(x => x.FullDescription).IsMaxLength().IsRequired();
            Property(x => x.MetaKeywords).HasMaxLength(255);
            Property(x => x.MetaDescription).HasMaxLength(255);
            HasRequired(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId);
        }
    }
}