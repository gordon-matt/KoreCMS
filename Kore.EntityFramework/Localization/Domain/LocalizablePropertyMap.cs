using System.Data.Entity.ModelConfiguration;
using Kore.Data.EntityFramework;

namespace Kore.Localization.Domain
{
    public class LocalizablePropertyMap : EntityTypeConfiguration<LocalizableProperty>, IEntityTypeConfiguration
    {
        public LocalizablePropertyMap()
        {
            ToTable("Kore_LocalizableProperties");
            HasKey(m => m.Id);
            Property(m => m.CultureCode).HasMaxLength(10).HasColumnType("varchar");
            Property(x => x.EntityType).HasMaxLength(512).HasColumnType("varchar").IsRequired();
            Property(x => x.EntityId).HasMaxLength(50).HasColumnType("varchar").IsRequired();
            Property(m => m.Property).HasMaxLength(128).HasColumnType("varchar").IsRequired();
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}