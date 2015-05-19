using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;

namespace Kore.Plugins.Ecommerce.Simple.Data.Domain
{
    public class OrderNote : IEntity
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public string Text { get; set; }

        public bool DisplayToCustomer { get; set; }

        public DateTime DateCreatedUtc { get; set; }

        public virtual Order Order { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class OrderNoteMap : EntityTypeConfiguration<OrderNote>, IEntityTypeConfiguration
    {
        public OrderNoteMap()
        {
            ToTable(Constants.Tables.OrderNotes);
            HasKey(x => x.Id);
            Property(x => x.OrderId).IsRequired();
            Property(x => x.Text).IsRequired().IsMaxLength();
            Property(x => x.DisplayToCustomer).IsRequired();
            Property(x => x.DateCreatedUtc).IsRequired();
            HasRequired(x => x.Order).WithMany(x => x.Notes).HasForeignKey(x => x.OrderId);
        }
    }
}