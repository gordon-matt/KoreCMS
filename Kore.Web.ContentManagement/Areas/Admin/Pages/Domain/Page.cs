using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Localization;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Domain
{
    //TODO: Add TemplatePath as a property for .cshtml files. If not specified, use default...
    //  this will be the only (and correct) way to insert widgets as well...; we'll
    //  specify zones in the template
    public class Page : ILocalizableEntity<Guid>, IEntity
    {
        public Guid Id { get; set; }

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

    public class PageMap : EntityTypeConfiguration<Page>, IEntityTypeConfiguration
    {
        public PageMap()
        {
            ToTable("Kore_Pages");
            HasKey(x => x.Id);
            Property(x => x.Title).HasMaxLength(255).IsRequired();
            Property(x => x.Slug).HasMaxLength(255).IsRequired();
            Property(x => x.CssClass).HasMaxLength(255);
            Property(x => x.MetaKeywords).HasMaxLength(255);
            Property(x => x.MetaDescription).HasMaxLength(255);
            Property(x => x.CultureCode).HasMaxLength(10);
        }
    }
}