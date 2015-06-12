using System.Web.Mvc;
using Kore.Web.Mvc.KoreUI;

namespace Kore.Web.Common.Html
{
    public class TilesBuilder<TModel> : BuilderBase<TModel, Tiles>
    {
        internal TilesBuilder(HtmlHelper<TModel> htmlHelper, Tiles tiles)
            : base(htmlHelper, tiles)
        {
        }

        public TileRow BeginTileRow(object htmlAttributes = null)
        {
            return new TileRow(base.textWriter, htmlAttributes);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}