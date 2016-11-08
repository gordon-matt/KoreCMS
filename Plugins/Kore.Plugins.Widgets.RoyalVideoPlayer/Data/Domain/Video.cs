using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Tenants.Domain;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.RoyalVideoPlayer.Data.Domain
{
    public class Video : ITenantEntity
    {
        public int Id { get; set; }

        public int? TenantId { get; set; }

        public string Title { get; set; }

        public string ThumbnailUrl { get; set; }

        public string VideoUrl { get; set; }

        public string MobileVideoUrl { get; set; }

        public string PosterUrl { get; set; }

        public string MobilePosterUrl { get; set; }

        public bool IsDownloadable { get; set; }

        public string PopoverHtml { get; set; }

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
            Property(x => x.Title).IsRequired().HasMaxLength(255).IsUnicode(true);
            Property(x => x.ThumbnailUrl).IsRequired().HasMaxLength(255).IsUnicode(true);
            Property(x => x.VideoUrl).IsRequired().HasMaxLength(255).IsUnicode(true);
            Property(x => x.MobileVideoUrl).HasMaxLength(255).IsUnicode(true);
            Property(x => x.PosterUrl).IsRequired().HasMaxLength(255).IsUnicode(true);
            Property(x => x.MobilePosterUrl).HasMaxLength(255).IsUnicode(true);
            Property(x => x.IsDownloadable).IsRequired();
            Property(x => x.PopoverHtml).IsMaxLength().IsUnicode(true);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return PluginManager.IsPluginInstalled(Constants.PluginSystemName); }
        }

        #endregion IEntityTypeConfiguration Members
    }
}