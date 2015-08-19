using System;
using Kore.Web.Mvc.Themes;

namespace Kore.Web.Areas.Admin.Configuration.Models
{
    public class EdmThemeConfiguration
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public bool SupportRtl { get; set; }

        public bool MobileTheme { get; set; }

        public string PreviewImageUrl { get; set; }

        public string PreviewText { get; set; }

        public bool IsDefaultDesktopTheme { get; set; }

        public bool IsDefaultMobileTheme { get; set; }

        public static implicit operator EdmThemeConfiguration(ThemeConfiguration other)
        {
            return new EdmThemeConfiguration
            {
                Id = Guid.NewGuid(), //To Keep OData v4 happy
                Title = other.ThemeName,
                SupportRtl = other.SupportRtl,
                MobileTheme = other.MobileTheme,
                PreviewImageUrl = other.PreviewImageUrl,
                PreviewText = other.PreviewText
            };
        }
    }
}