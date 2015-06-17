using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Domain
{
    public class PageType : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string LayoutPath { get; set; }

        public string DisplayTemplatePath { get; set; }

        public string EditorTemplatePath { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class PageTypeMap : EntityTypeConfiguration<PageType>, IEntityTypeConfiguration
    {
        public PageTypeMap()
        {
            ToTable(CmsConstants.Tables.PageTypes);
            HasKey(x => x.Id);
            Property(x => x.Name).HasMaxLength(255).IsRequired();
            Property(x => x.LayoutPath).IsRequired().HasMaxLength(255);
            Property(x => x.DisplayTemplatePath).HasMaxLength(255);
            Property(x => x.EditorTemplatePath).HasMaxLength(255);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}