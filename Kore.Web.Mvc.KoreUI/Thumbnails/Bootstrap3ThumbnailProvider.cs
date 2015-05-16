using System.IO;
using System.Web.Mvc;

namespace Kore.Web.Mvc.KoreUI
{
    public class Bootstrap3ThumbnailProvider : IThumbnailProvider
    {
        #region IThumbnailProvider Members

        public string ThumbnailTag
        {
            get { return "div"; }
        }

        public void BeginThumbnail(Thumbnail thumbnail)
        {
            thumbnail.EnsureClass("thumbnail");
        }

        public void BeginCaptionPanel(TextWriter writer)
        {
            writer.Write(@"<div class=""caption"">");
        }

        public void EndCaptionPanel(TextWriter writer)
        {
            writer.Write("</div>");
        }

        public void WriteThumbnailImage(Thumbnail thumbnail, UrlHelper url, TextWriter writer)
        {
            writer.Write(string.Format(
                @"<img src=""{0}"" alt=""{1}"" />",
                url.Content(thumbnail.ImageSource),
                thumbnail.ImageAltText));
        }

        public MvcHtmlString Thumbnail(HtmlHelper html, string src, string alt, string href, object aHtmlAttributes = null, object imgHtmlAttributes = null)
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);

            var builder = new FluentTagBuilder("a")
                .MergeAttribute("href", href)
                .MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(aHtmlAttributes))
                .MergeAttribute("class", "thumbnail")
                .StartTag("img", TagRenderMode.SelfClosing)
                    .MergeAttribute("src", urlHelper.Content(src))
                    .MergeAttribute("alt", alt)
                    .MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(imgHtmlAttributes))
                .EndTag();

            return MvcHtmlString.Create(builder.ToString());
        }

        #endregion IThumbnailProvider Members
    }
}