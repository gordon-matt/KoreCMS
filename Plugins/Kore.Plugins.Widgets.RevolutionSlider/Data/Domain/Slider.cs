using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.RevolutionSlider.Data.Domain
{
    public class Slider : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class SliderMap : EntityTypeConfiguration<Slider>, IEntityTypeConfiguration
    {
        public SliderMap()
        {
            ToTable(Constants.Tables.Sliders);
            HasKey(x => x.Id);
            Property(x => x.Name).IsRequired().HasMaxLength(255);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return PluginManager.IsPluginInstalled(Constants.PluginSystemName); }
        }

        #endregion IEntityTypeConfiguration Members
    }
}