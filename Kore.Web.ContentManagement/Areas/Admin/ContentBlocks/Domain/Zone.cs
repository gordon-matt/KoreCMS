using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data.EntityFramework;
using Kore.Tenants.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Domain
{
    public class Zone : ITenantEntity
    {
        public Guid Id { get; set; }

        public int? TenantId { get; set; }

        public string Name { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class ZoneMap : EntityTypeConfiguration<Zone>, IEntityTypeConfiguration
    {
        public ZoneMap()
        {
            ToTable(CmsConstants.Tables.Zones);
            HasKey(x => x.Id);
            Property(x => x.Name).IsRequired().HasMaxLength(255).IsUnicode(true);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}