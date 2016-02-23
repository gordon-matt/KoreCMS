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
            Property(m => m.Name).IsRequired().HasMaxLength(255).IsUnicode(true);
            Property(m => m.CultureCode).IsRequired().HasMaxLength(10).IsUnicode(false);
            Property(m => m.IsRTL).IsRequired();
            Property(m => m.IsEnabled).IsRequired();
            Property(m => m.SortOrder).IsRequired();
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}