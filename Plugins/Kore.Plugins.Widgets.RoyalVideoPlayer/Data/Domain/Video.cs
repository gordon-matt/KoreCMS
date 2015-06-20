using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.RoyalVideoPlayer.Data.Domain
{
    public class Video : IEntity
    {
        public int Id { get; set; }

        public int PlaylistId { get; set; }

        public string ThumbailUrl { get; set; }

        public string VideoUrl { get; set; }

        public string MobileVideoUrl { get; set; }

        public string PosterUrl { get; set; }

        public string MobilePosterUrl { get; set; }

        public bool IsDownloadable { get; set; }

        public string PopoverHtml { get; set; }

        public virtual Playlist Playlist { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class VideoMap : EntityTypeConfiguration<Video>, IEntityTypeConfiguration
    {
        public VideoMap()
        {
            ToTable(Constants.Tables.Videos);
            HasKey(x => x.Id);
            Property(x => x.PlaylistId).IsRequired();
            Property(x => x.ThumbailUrl).IsRequired().HasMaxLength(255);
            Property(x => x.VideoUrl).IsRequired().HasMaxLength(255);
            Property(x => x.MobileVideoUrl).HasMaxLength(255);
            Property(x => x.PosterUrl).IsRequired().HasMaxLength(255);
            Property(x => x.MobilePosterUrl).HasMaxLength(255);
            Property(x => x.IsDownloadable).IsRequired();
            Property(x => x.PopoverHtml).IsMaxLength();
            HasRequired(x => x.Playlist).WithMany(x => x.Videos).HasForeignKey(x => x.PlaylistId);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return PluginManager.IsPluginInstalled(Constants.PluginSystemName); }
        }

        #endregion IEntityTypeConfiguration Members
    }
}