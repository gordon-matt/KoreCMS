using System;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.FullCalendar.Data.Domain
{
    public class CalendarEvent : IEntity
    {
        public int Id { get; set; }

        public int CalendarId { get; set; }

        public string Name { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public virtual Calendar Calendar { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class CalendarEntryMap : EntityTypeConfiguration<CalendarEvent>, IEntityTypeConfiguration
    {
        public CalendarEntryMap()
        {
            ToTable(Constants.Tables.Events);
            HasKey(x => x.Id);
            Property(x => x.CalendarId).IsRequired();
            Property(x => x.Name).IsRequired().HasMaxLength(255);
            Property(x => x.StartDateTime).IsRequired();
            Property(x => x.EndDateTime).IsRequired();
            HasRequired(x => x.Calendar).WithMany(x => x.Events).HasForeignKey(x => x.CalendarId);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return PluginManager.IsPluginInstalled(Constants.PluginSystemName); }
        }

        #endregion IEntityTypeConfiguration Members
    }
}