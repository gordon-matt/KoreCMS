using System.Web.Mvc;

namespace Kore.Web.Mvc.KoreUI
{
    public class ThumbnailBuilder<TModel> : BuilderBase<TModel, Thumbnail>
    {
        internal ThumbnailBuilder(HtmlHelper<TModel> htmlHelper, Thumbnail thumbnail)
            : base(htmlHelper, thumbnail)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            base.element.Provider.ThumbnailProvider.WriteThumbnailImage(this.element, urlHelper, base.textWriter);
        }

        public ThumbnailCaptionPanel BeginCaptionPanel()
        {
            return new ThumbnailCaptionPanel(base.element.Provider, base.textWriter);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}