using System.Data.Entity.ModelConfiguration;
using Kore.Data.EntityFramework;

namespace Kore.Localization.Domain
{
    public class LocalizableStringMap : EntityTypeConfiguration<LocalizableString>, IEntityTypeConfiguration
    {
        public LocalizableStringMap()
        {
            ToTable("Kore_LocalizableStrings");
            HasKey(m => m.Id);
            Property(m => m.CultureCode).HasMaxLength(10);
            Property(m => m.TextKey).IsRequired();
        }
    }
}