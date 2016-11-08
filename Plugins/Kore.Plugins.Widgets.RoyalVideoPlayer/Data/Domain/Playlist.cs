using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Tenants.Domain;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.RoyalVideoPlayer.Data.Domain
{
    public class Playlist : ITenantEntity
    {
        public int Id { get; set; }

        public int? TenantId { get; set; }

        public string Name { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class PlaylistMap : EntityTypeConfiguration<Playlist>, IEntityTypeConfiguration
    {
        public PlaylistMap()
        {
            ToTable(Constants.Tables.Playlists);
            HasKey(x => x.Id);
            Property(x => x.Name).IsRequired().HasMaxLength(255).IsUnicode(true);
            //HasMany(c => c.Videos).WithMany(x => x.Playlists).Map(m =>
            //{
            //    m.MapLeftKey("PlaylistId");
            //    m.MapRightKey("VideoId");
            //    m.ToTable(Constants.Tables.PlaylistVideos);
            //});
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return PluginManager.IsPluginInstalled(Constants.PluginSystemName); }
        }

        #endregion IEntityTypeConfiguration Members
    }
}