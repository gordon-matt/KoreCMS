using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Tenants.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Domain
{
    public class Page : ITenantEntity
    {
        private ICollection<PageVersion> versions;

        public Guid Id { get; set; }

        public int? TenantId { get; set; }

        public Guid? ParentId { get; set; }

        public Guid PageTypeId { get; set; }

        public string Name { get; set; }

        public bool IsEnabled { get; set; }

        public int Order { get; set; }

        public string AccessRestrictions { get; set; }

        public ICollection<PageVersion> Versions
        {
            get { return versions ?? (versions = new HashSet<PageVersion>()); }
            set { versions = value; }
        }

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
            ToTable(CmsConstants.Tables.Pages);
            HasKey(x => x.Id);
            Property(x => x.PageTypeId).IsRequired();
            Property(x => x.Name).IsRequired().HasMaxLength(255).IsUnicode(true);
            Property(x => x.IsEnabled).IsRequired();
            Property(x => x.Order).IsRequired();
            Property(x => x.AccessRestrictions).HasMaxLength(1024).IsUnicode(false);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}