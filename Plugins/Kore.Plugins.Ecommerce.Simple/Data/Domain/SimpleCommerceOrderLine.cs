using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Web.Plugins;

namespace Kore.Plugins.Ecommerce.Simple.Data.Domain
{
    public class SimpleCommerceOrderLine : IEntity
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public float UnitPrice { get; set; }

        public short Quantity { get; set; }

        public virtual SimpleCommerceOrder Order { get; set; }

        public virtual SimpleCommerceProduct Product { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class OrderLineMap : EntityTypeConfiguration<SimpleCommerceOrderLine>, IEntityTypeConfiguration
    {
        public OrderLineMap()
        {
            ToTable(Constants.Tables.OrderLines);
            HasKey(x => x.Id);
            Property(x => x.OrderId).IsRequired();
            Property(x => x.ProductId).IsRequired();
            Property(x => x.UnitPrice).IsRequired();
            Property(x => x.Quantity).IsRequired();
            HasRequired(x => x.Order).WithMany(x => x.Lines).HasForeignKey(x => x.OrderId);
            HasRequired(x => x.Product).WithMany().HasForeignKey(x => x.ProductId);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return PluginManager.IsPluginInstalled(Constants.PluginSystemName); }
        }

        #endregion IEntityTypeConfiguration Members
    }
}