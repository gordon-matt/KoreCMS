using System.IO;
using System.Web.Mvc;

namespace Kore.Web.Mvc.KoreUI
{
    public interface IThumbnailProvider
    {
        string ThumbnailTag { get; }

        void BeginThumbnail(Thumbnail thumbnail);

        void BeginCaptionPanel(TextWriter writer);

        void EndCaptionPanel(TextWriter writer);

        void WriteThumbnailImage(Thumbnail thumbnail, UrlHelper url, TextWriter writer);

        MvcHtmlString Thumbnail(HtmlHelper html, string src, string alt, string href, object aHtmlAttributes = null, object imgHtmlAttributes = null);
    }
}