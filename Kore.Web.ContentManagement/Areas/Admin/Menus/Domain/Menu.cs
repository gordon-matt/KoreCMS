using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data.EntityFramework;
using Kore.Tenants.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Menus.Domain
{
    public class Menu : ITenantEntity
    {
        public Guid Id { get; set; }

        public int? TenantId { get; set; }

        public string Name { get; set; }

        public string UrlFilter { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class MenuMap : EntityTypeConfiguration<Menu>, IEntityTypeConfiguration
    {
        public MenuMap()
        {
            ToTable(CmsConstants.Tables.Menus);
            HasKey(x => x.Id);
            Property(x => x.Name).IsRequired().HasMaxLength(255).IsUnicode(true);
            Property(x => x.UrlFilter).HasMaxLength(255).IsUnicode(true);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}