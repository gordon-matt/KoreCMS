using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;

namespace Kore.Plugins.Ecommerce.Simple.Data.Domain
{
    public class Order : IEntity
    {
        private ICollection<OrderLine> lines;

        public int Id { get; set; }

        public string UserId { get; set; }

        public int BillingAddressId { get; set; }

        public int ShippingAddressId { get; set; }

        public float OrderTotal { get; set; }

        public string IPAddress { get; set; }

        public DateTime OrderDateUtc { get; set; }

        public OrderStatus Status { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public virtual Address BillingAddress { get; set; }

        public virtual Address ShippingAddress { get; set; }

        public virtual ICollection<OrderLine> Lines
        {
            get { return lines ?? (lines = new List<OrderLine>()); }
            set { lines = value; }
        }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class OrderMap : EntityTypeConfiguration<Order>, IEntityTypeConfiguration
    {
        public OrderMap()
        {
            ToTable(Constants.Tables.Orders);
            HasKey(x => x.Id);
            Property(x => x.BillingAddressId).IsRequired();
            Property(x => x.ShippingAddressId).IsRequired();
            Property(x => x.OrderTotal).IsRequired();
            Property(x => x.OrderDateUtc).IsRequired();
            Property(x => x.Status).IsRequired();
            Property(x => x.PaymentStatus).IsRequired();
            HasRequired(x => x.BillingAddress).WithMany().HasForeignKey(x => x.BillingAddressId).WillCascadeOnDelete(false);
            HasRequired(x => x.ShippingAddress).WithMany().HasForeignKey(x => x.ShippingAddressId).WillCascadeOnDelete(false);
        }
    }
}