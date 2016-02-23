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
            Property(m => m.CultureCode).HasMaxLength(10).IsUnicode(false);
            Property(x => x.EntityType).IsRequired().HasMaxLength(512).IsUnicode(false);
            Property(x => x.EntityId).IsRequired().HasMaxLength(50).IsUnicode(false);
            Property(m => m.Property).IsRequired().HasMaxLength(128).IsUnicode(false);
            Property(m => m.Value).IsMaxLength().IsUnicode(true);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}