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

        public DateTime OrderDateUtc { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

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

    public enum PaymentStatus : byte
    {
        Pending = 0,
        Paid = 1
    }

    public class OrderMap : EntityTypeConfiguration<Order>, IEntityTypeConfiguration
    {
        public OrderMap()
        {
            ToTable(Constants.Tables.Orders);
            HasKey(x => x.Id);
            Property(x => x.OrderDateUtc).IsRequired();
            Property(x => x.PaymentStatus).IsRequired();
        }
    }
}