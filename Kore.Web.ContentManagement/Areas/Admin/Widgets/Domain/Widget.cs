using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Localization;

namespace Kore.Web.ContentManagement.Areas.Admin.Widgets.Domain
{
    public class Widget : ILocalizableEntity<Guid>, IEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public int Order { get; set; }

        public bool IsEnabled { get; set; }

        public string WidgetName { get; set; }

        public string WidgetType { get; set; }

        public Guid ZoneId { get; set; }

        public string DisplayCondition { get; set; }

        public string WidgetValues { get; set; }

        public Guid? PageId { get; set; }

        public string CultureCode { get; set; }

        public Guid? RefId { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class WidgetMap : EntityTypeConfiguration<Widget>, IEntityTypeConfiguration
    {
        public WidgetMap()
        {
            ToTable("Kore_Widgets");
            HasKey(x => x.Id);
            Property(x => x.Title).HasMaxLength(255).IsRequired();
            Property(x => x.WidgetName).HasMaxLength(255).IsRequired();
            Property(x => x.WidgetType).HasMaxLength(1024).IsRequired();
            Property(x => x.CultureCode).HasMaxLength(10);
            Property(x => x.IsEnabled).IsRequired();
        }
    }
}