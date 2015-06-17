using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;

namespace Kore.Web.Common.Areas.Admin.Regions.Domain
{
    public class RegionSettings : IEntity
    {
        public int Id { get; set; }

        public int RegionId { get; set; }

        public string SettingsId { get; set; }

        public string Fields { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class RegionSettingsMap : EntityTypeConfiguration<RegionSettings>, IEntityTypeConfiguration
    {
        public RegionSettingsMap()
        {
            ToTable(Constants.Tables.RegionSettings);
            HasKey(m => m.Id);
            Property(m => m.RegionId).IsRequired();
            Property(m => m.SettingsId).IsRequired().HasMaxLength(255);
            Property(m => m.Fields).IsRequired().IsMaxLength();
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return true; }
        }

        #endregion IEntityTypeConfiguration Members
    }
}