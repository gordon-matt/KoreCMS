using System.Data.Entity.ModelConfiguration;
using Kore.Data.EntityFramework;

namespace Kore.Localization.Domain
{
    public class LanguageMap : EntityTypeConfiguration<Kore.Localization.Domain.Language>, IEntityTypeConfiguration
    {
        public LanguageMap()
        {
            ToTable("Kore_Languages");
            HasKey(m => m.Id);
            Property(m => m.Name).HasMaxLength(255).IsRequired();
            Property(m => m.CultureCode).HasMaxLength(10).IsRequired();
            Property(m => m.IsRTL).IsRequired();
            Property(m => m.IsEnabled).IsRequired();
            Property(m => m.SortOrder).IsRequired();
        }
    }
}