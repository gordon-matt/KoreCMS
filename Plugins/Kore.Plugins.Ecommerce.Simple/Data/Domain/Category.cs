using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;

namespace Kore.Plugins.Ecommerce.Simple.Data.Domain
{
    public class Category : IEntity
    {
        public int Id { get; set; }

        public int ParentId { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

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
        }
    }
}