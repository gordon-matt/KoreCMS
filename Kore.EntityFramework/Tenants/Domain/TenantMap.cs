using System.Data.Entity.ModelConfiguration;
using Kore.Data.EntityFramework;
using Kore.Tenants.Domain;

namespace Kore.EntityFramework.Tenants.Domain
{
    public class TenantMap : EntityTypeConfiguration<Tenant>, IEntityTypeConfiguration
    {
        public TenantMap()
        {
            ToTable("Kore_Tenants");
            HasKey(x => x.Id);
            Property(x => x.Name).IsRequired().HasMaxLength(255).IsUnicode(true);
            Property(x => x.Url).IsRequired().HasMaxLength(255).IsUnicode(true);
            //Property(x => x.SecureUrl).HasMaxLength(255).IsUnicode(true);
            Property(x => x.Hosts).HasMaxLength(1024).IsUnicode(true);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}