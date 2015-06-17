using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Microsoft.AspNet.Identity.EntityFramework;

namespace KoreCMS.Data.Domain
{
    public class Permission : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members

        public virtual ICollection<IdentityRole> Roles { get; set; }
    }

    public class PermissionMap : EntityTypeConfiguration<Permission>, IEntityTypeConfiguration
    {
        public PermissionMap()
        {
            ToTable(Constants.Tables.Permissions);
            HasKey(x => x.Id);
            Property(x => x.Name).HasMaxLength(50).IsRequired();
            Property(x => x.Category).HasMaxLength(50).IsRequired();
            Property(x => x.Description).HasMaxLength(128).IsRequired();
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