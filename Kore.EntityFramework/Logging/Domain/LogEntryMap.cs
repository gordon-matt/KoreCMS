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
            Property(m => m.EventLevel).IsRequired().HasMaxLength(5);
            Property(m => m.UserName).IsRequired().HasMaxLength(255);
            Property(m => m.MachineName).IsRequired().HasMaxLength(255);
            Property(m => m.EventMessage).IsRequired().IsMaxLength();
            Property(m => m.ErrorSource).IsRequired().HasMaxLength(255);
            Property(m => m.ErrorClass).HasMaxLength(512);
            Property(m => m.ErrorMethod).HasMaxLength(255);
            Property(m => m.ErrorMessage).IsMaxLength();
            Property(m => m.InnerErrorMessage).IsMaxLength();
        }
    }
}