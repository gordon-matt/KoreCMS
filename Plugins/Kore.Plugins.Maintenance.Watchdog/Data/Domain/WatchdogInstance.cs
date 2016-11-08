using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Tenants.Domain;
using Kore.Web.Plugins;

namespace Kore.Plugins.Maintenance.Watchdog.Data.Domain
{
    [DataContract]
    public class WatchdogInstance : ITenantEntity
    {
        [DataMember]
        public int Id { get; set; }

        [IgnoreDataMember]
        public int? TenantId { get; set; }

        [DataMember]
        public string Url { get; set; }

        [DataMember]
        public string Password { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class WatchdogInstanceMap : EntityTypeConfiguration<WatchdogInstance>, IEntityTypeConfiguration
    {
        public WatchdogInstanceMap()
        {
            ToTable(Constants.Tables.WatchdogInstances);
            HasKey(x => x.Id);
            Property(x => x.Url).IsRequired().HasMaxLength(255).IsUnicode(true);
            Property(x => x.Password).IsRequired().HasMaxLength(255).IsUnicode(true);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return PluginManager.IsPluginInstalled(Constants.PluginSystemName); }
        }

        #endregion IEntityTypeConfiguration Members
    }
}