using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;

namespace Kore.Web.ContentManagement.Areas.Admin.Menus.Domain
{
    public class MenuItem : IEntity
    {
        public Guid Id { get; set; }

        public Guid MenuId { get; set; }

        public string Text { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public string CssClass { get; set; }

        public int Position { get; set; }

        public Guid? ParentId { get; set; }

        public bool Enabled { get; set; }

        public bool IsExternalUrl { get; set; }

        public Guid? RefId { get; set; }

        //public virtual Menu Menu { get; set; }

        //public virtual MenuItem Parent { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members

        public override string ToString()
        {
            return Text;
        }
    }

    public class MenuItemMap : EntityTypeConfiguration<MenuItem>, IEntityTypeConfiguration
    {
        public MenuItemMap()
        {
            ToTable(CmsConstants.Tables.MenuItems);
            HasKey(x => x.Id);
            Property(x => x.MenuId).IsRequired();
            Property(x => x.Text).IsRequired().HasMaxLength(255);
            Property(x => x.Description).HasMaxLength(255);
            Property(x => x.Url).IsRequired().HasMaxLength(255);
            Property(x => x.CssClass).HasMaxLength(128);
            Property(x => x.Position).IsRequired();
            Property(x => x.Enabled).IsRequired();
            Property(x => x.IsExternalUrl).IsRequired();
            //HasRequired(x => x.Menu).WithMany().HasForeignKey(x => x.MenuId);
            //HasOptional(x => x.Parent).WithMany().HasForeignKey(x => x.ParentId);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}