using System.Collections.Generic;
using Kore.Localization;

namespace Kore.Plugins.Widgets.RoyalVideoPlayer.Infrastructure
{
    public class LanguagePackInvariant : ILanguagePack
    {
        #region ILanguagePack Members

        public string CultureCode
        {
            get { return null; }
        }

        public IDictionary<string, string> LocalizedStrings
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { LocalizableStrings.RoyalVideoPlayer, "Royal Video Player" },
                    { LocalizableStrings.Models.Playlist.Name, "Name" },
                    { LocalizableStrings.Models.Video.IsDownloadable, "Is Downloadable" },
                    { LocalizableStrings.Models.Video.MobilePosterUrl, "Mobile Poster URL" },
                    { LocalizableStrings.Models.Video.MobileVideoUrl, "Mobile Video URL" },
                    { LocalizableStrings.Models.Video.PopoverHtml, "Popover HTML" },
                    { LocalizableStrings.Models.Video.PosterUrl, "Poster URL" },
                    { LocalizableStrings.Models.Video.ThumbnailUrl, "Thumbnail URL" },
                    { LocalizableStrings.Models.Video.Title, "Title" },
                    { LocalizableStrings.Models.Video.VideoUrl, "Video URL" }
                };
            }
        }

        #endregion ILanguagePack Members
    }
}