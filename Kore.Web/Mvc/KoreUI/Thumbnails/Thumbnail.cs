namespace Kore.Web.Mvc.KoreUI
{
    public class Thumbnail : HtmlElement
    {
        public string ImageSource { get; set; }

        public string ImageAltText { get; set; }

        public Thumbnail(string src, string alt)
            : this(src, alt, null, null)
        {
        }

        public Thumbnail(string src, string alt, object divHtmlAttributes, object imgHtmlAttributes)
            : base(KoreUISettings.Provider.ThumbnailProvider.ThumbnailTag, divHtmlAttributes)
        {
            this.ImageSource = src;
            this.ImageAltText = alt;
            KoreUISettings.Provider.ThumbnailProvider.BeginThumbnail(this);
        }
    }
}