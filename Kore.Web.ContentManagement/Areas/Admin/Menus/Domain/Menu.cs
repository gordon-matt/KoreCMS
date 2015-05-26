using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;

namespace Kore.Web.ContentManagement.Areas.Admin.Menus.Domain
{
    public class Menu : IEntity
    {
        public Guid Id { get; set; }

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
            Property(x => x.Name).HasMaxLength(255).IsRequired();
            Property(x => x.UrlFilter).HasMaxLength(255);
        }
    }
}