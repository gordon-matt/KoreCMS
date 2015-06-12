using System.IO;
using System.Web.Mvc;
using Kore.Web.Mvc.KoreUI;

namespace Kore.Web.Common.Html
{
    public class Tiles : HtmlElement
    {
        public Tiles()
            : this(null)
        {
        }

        public Tiles(object htmlAttributes)
            : base(htmlAttributes)
        {
            EnsureClass("tiles");
        }

        protected override void StartTag(TextWriter textWriter)
        {
            var builder = new TagBuilder("div");
            builder.MergeAttributes<string, object>(this.HtmlAttributes);
            string tag = builder.ToString(TagRenderMode.StartTag);

            textWriter.Write(tag);
        }

        protected override void EndTag(TextWriter textWriter)
        {
            textWriter.Write("</div>");
        }
    }
}