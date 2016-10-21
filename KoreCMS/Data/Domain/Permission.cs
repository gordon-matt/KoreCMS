using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Kore.Data.EntityFramework;
using Kore.Tenants.Domain;

namespace KoreCMS.Data.Domain
{
    public class Permission : ITenantEntity
    {
        public int Id { get; set; }

        public int? TenantId { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members

        public virtual ICollection<ApplicationRole> Roles { get; set; }
    }

    public class PermissionMap : EntityTypeConfiguration<Permission>, IEntityTypeConfiguration
    {
        public PermissionMap()
        {
            ToTable(Constants.Tables.Permissions);
            HasKey(x => x.Id);
            Property(x => x.Name).IsRequired().HasMaxLength(50).IsUnicode(true);
            Property(x => x.Category).IsRequired().HasMaxLength(50).IsUnicode(true);
            Property(x => x.Description).IsRequired().HasMaxLength(128).IsUnicode(true);
            HasMany(c => c.Roles).WithMany().Map(m =>
            {
                m.MapLeftKey("PermissionId");
                m.MapRightKey("RoleId");
                m.ToTable("RolePermissions");
            });
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}