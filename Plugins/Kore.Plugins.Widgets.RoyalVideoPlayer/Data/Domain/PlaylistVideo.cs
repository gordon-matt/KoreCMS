using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.RoyalVideoPlayer.Data.Domain
{
    public class PlaylistVideo : IEntity
    {
        public int PlaylistId { get; set; }

        public int VideoId { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { PlaylistId, VideoId }; }
        }

        #endregion IEntity Members
    }

    public class PlaylistVideoMap : EntityTypeConfiguration<PlaylistVideo>, IEntityTypeConfiguration
    {
        public PlaylistVideoMap()
        {
            ToTable(Constants.Tables.PlaylistVideos);
            HasKey(x => new { x.PlaylistId, x.VideoId });
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return PluginManager.IsPluginInstalled(Constants.PluginSystemName); }
        }

        #endregion IEntityTypeConfiguration Members
    }
}