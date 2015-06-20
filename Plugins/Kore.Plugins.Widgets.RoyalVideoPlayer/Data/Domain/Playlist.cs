﻿using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.RoyalVideoPlayer.Data.Domain
{
    public class Playlist : IEntity
    {
        private ICollection<Video> videos;

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Video> Videos
        {
            get { return videos ?? (videos = new HashSet<Video>()); }
            set { videos = value; }
        }

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