using System.Data.Entity.ModelConfiguration;
using Kore.Data.EntityFramework;

namespace Kore.Configuration.Domain
{
    public class SettingMap : EntityTypeConfiguration<Setting>, IEntityTypeConfiguration
    {
        public SettingMap()
        {
            ToTable("Kore_Settings");
            HasKey(s => s.Id);
            Property(s => s.Name).IsRequired().HasMaxLength(255);
            Property(s => s.Type).IsRequired().HasMaxLength(255);
            Property(s => s.Value).IsMaxLength();
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}