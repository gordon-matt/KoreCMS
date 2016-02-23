using System.Data.Entity.ModelConfiguration;
using Kore.Data.EntityFramework;

namespace Kore.Logging.Domain
{
    public class LogEntryMap : EntityTypeConfiguration<LogEntry>, IEntityTypeConfiguration
    {
        public LogEntryMap()
        {
            ToTable("Kore_Log");
            HasKey(m => m.Id);
            Property(m => m.EventDateTime).IsRequired();
            Property(m => m.EventLevel).IsRequired().HasMaxLength(5).IsUnicode(false);
            Property(m => m.UserName).IsRequired().HasMaxLength(128).IsUnicode(true);
            Property(m => m.MachineName).IsRequired().HasMaxLength(255).IsUnicode(false);
            Property(m => m.EventMessage).IsRequired().IsMaxLength().IsUnicode(true);
            Property(m => m.ErrorSource).IsRequired().HasMaxLength(255).IsUnicode(true);
            Property(m => m.ErrorClass).HasMaxLength(512).IsUnicode(false);
            Property(m => m.ErrorMethod).HasMaxLength(255).IsUnicode(false);
            Property(m => m.ErrorMessage).IsMaxLength().IsUnicode(true);
            Property(m => m.InnerErrorMessage).IsMaxLength().IsUnicode(true);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}