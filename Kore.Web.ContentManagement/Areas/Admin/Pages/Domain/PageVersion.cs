using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data.EntityFramework;
using Kore.Tenants.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Domain
{
    public class PageVersion : ITenantEntity
    {
        public Guid Id { get; set; }

        public int? TenantId { get; set; }

        public Guid PageId { get; set; }

        public string CultureCode { get; set; }

        public DateTime DateCreatedUtc { get; set; }

        public DateTime DateModifiedUtc { get; set; }

        public VersionStatus Status { get; set; }

        public string Title { get; set; }

        public string Slug { get; set; }

        public string Fields { get; set; }

        public bool ShowOnMenus { get; set; }

        public virtual Page Page { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public enum VersionStatus : byte
    {
        Draft = 0,
        Published = 1,
        Archived = 2
    }

    public class PageVersionMap : EntityTypeConfiguration<PageVersion>, IEntityTypeConfiguration
    {
        public PageVersionMap()
        {
            ToTable(CmsConstants.Tables.PageVersions);
            HasKey(x => x.Id);
            Property(x => x.PageId).IsRequired();
            Property(x => x.CultureCode).HasMaxLength(10).IsUnicode(false);
            Property(x => x.DateCreatedUtc).IsRequired();
            Property(x => x.DateModifiedUtc).IsRequired();
            Property(x => x.Status).IsRequired();
            Property(x => x.Title).IsRequired().HasMaxLength(255).IsUnicode(true);
            Property(x => x.Slug).IsRequired().HasMaxLength(255).IsUnicode(true);
            Property(x => x.Fields).IsMaxLength().IsUnicode(true);
            Property(x => x.ShowOnMenus).IsRequired();

            HasRequired(x => x.Page).WithMany(x => x.Versions).HasForeignKey(x => x.PageId);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}