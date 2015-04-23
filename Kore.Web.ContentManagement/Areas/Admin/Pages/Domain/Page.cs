using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Localization;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Domain
{
    public class Page : ILocalizableEntity<Guid>, IEntity
    {
        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        public Guid PageTypeId { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string Fields { get; set; }

        public bool IsEnabled { get; set; }

        public int Order { get; set; }

        public string AccessRestrictions { get; set; }

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

    public class PageMap : EntityTypeConfiguration<Page>, IEntityTypeConfiguration
    {
        public PageMap()
        {
            ToTable("Kore_Pages");
            HasKey(x => x.Id);
            Property(x => x.PageTypeId).IsRequired();
            Property(x => x.Name).IsRequired().HasMaxLength(255);
            Property(x => x.Slug).IsRequired().HasMaxLength(255);
            Property(x => x.Fields).IsMaxLength();
            Property(x => x.IsEnabled).IsRequired();
            Property(x => x.Order).IsRequired();
            Property(x => x.AccessRestrictions).HasColumnType("varchar").HasMaxLength(1024);
            Property(x => x.DateCreatedUtc).IsRequired();
            Property(x => x.DateModifiedUtc).IsRequired();
            Property(x => x.CultureCode).HasMaxLength(10);
        }
    }
}