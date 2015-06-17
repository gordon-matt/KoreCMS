using System.Data.Entity.ModelConfiguration;
using Kore.Data.EntityFramework;
using Kore.Tasks.Domain;

namespace Kore.ScheduledTasks.Domain
{
    public class ScheduledTaskMap : EntityTypeConfiguration<ScheduledTask>, IEntityTypeConfiguration
    {
        public ScheduledTaskMap()
        {
            ToTable("Kore_ScheduledTasks");
            HasKey(s => s.Id);
            Property(s => s.Name).IsRequired().HasMaxLength(255);
            Property(s => s.Type).IsRequired().HasMaxLength(255);
            Property(s => s.Seconds).IsRequired();
            Property(s => s.Enabled).IsRequired();
            Property(s => s.StopOnError).IsRequired();
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}