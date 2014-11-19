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

        public DateTime ArchivedDate { get; set; }

        public string Title { get; set; }

        public string Slug { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }

        public bool IsEnabled { get; set; }

        public string BodyContent { get; set; }

        public string CssClass { get; set; }

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
            Property(x => x.Title).HasMaxLength(255).IsRequired();
            Property(x => x.Slug).HasMaxLength(255).IsRequired();
            Property(x => x.CssClass).HasMaxLength(255);
            Property(x => x.MetaKeywords).HasMaxLength(255);
            Property(x => x.MetaDescription).HasMaxLength(255);
            Property(x => x.CultureCode).HasMaxLength(10);
            Property(x => x.ArchivedDate).IsRequired();
        }
    }
}