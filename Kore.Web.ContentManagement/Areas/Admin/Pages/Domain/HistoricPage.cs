using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Localization;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Domain
{
    public class HistoricPage : ILocalizableEntity<Guid>, IEntity
    {
        public Guid Id { get; set; }

        public Guid PageId { get; set; }

        public Guid? ParentId { get; set; }

        public Guid PageTypeId { get; set; }

        public DateTime ArchivedDate { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string Fields { get; set; }

        public bool IsEnabled { get; set; }

        public DateTime DateCreatedUtc { get; set; }

        public DateTime DateModifiedUtc { get; set; }

        public string CultureCode { get; set; }

        public Guid? RefId { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class HistoricPageMap : EntityTypeConfiguration<HistoricPage>, IEntityTypeConfiguration
    {
        public HistoricPageMap()
        {
            ToTable("Kore_HistoricPages");
            HasKey(x => x.Id);
            Property(x => x.PageId).IsRequired();
            Property(x => x.PageTypeId).IsRequired();
            Property(x => x.ArchivedDate).IsRequired();
            Property(x => x.Name).HasMaxLength(255).IsRequired();
            Property(x => x.Slug).HasMaxLength(255).IsRequired();
            Property(x => x.Fields).IsMaxLength();
            Property(x => x.IsEnabled).IsRequired();
            Property(x => x.DateCreatedUtc).IsRequired();
            Property(x => x.DateModifiedUtc).IsRequired();
            Property(x => x.CultureCode).HasMaxLength(10);
        }
    }
}