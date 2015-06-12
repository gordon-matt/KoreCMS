using System;
using System.IO;
using System.Web.Mvc;

namespace Kore.Web.Common.Html
{
    public class TileRow : IDisposable
    {
        private readonly TextWriter textWriter;

        internal TileRow(TextWriter writer, object htmlAttributes = null)
        {
            this.textWriter = writer;

            var divBuilder = new TagBuilder("div");
            divBuilder.AddCssClass("tilerow");

            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            divBuilder.MergeAttributes(attributes);

            if (attributes.ContainsKey("class"))
            {
                divBuilder.AddCssClass(attributes["class"].ToString());
            }

            this.textWriter.Write(divBuilder.ToString(TagRenderMode.StartTag));
        }

        public Tile BeginTile(string href, MetroColor color = MetroColor.Blue, MetroTileSize size = MetroTileSize.Single)
        {
            return new Tile(this.textWriter, href, color, size);
        }

        public void Tile(string href, string text, MetroColor color = MetroColor.Blue, MetroTileSize size = MetroTileSize.Single, object aHtmlAttributes = null, object divHtmlAttributes = null)
        {
            var aBuilder = new TagBuilder("a");
            aBuilder.MergeAttribute("href", href);
            aBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(aHtmlAttributes));

            var divBuilder = new TagBuilder("div");
            divBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(divHtmlAttributes));
            divBuilder.AddCssClass("tile");
            divBuilder.AddCssClass(GetColorCssClass(color));
            divBuilder.AddCssClass(GetSizeCssClass(size));
            divBuilder.InnerHtml = string.Format("<h2>{0}</h2>", text);

            aBuilder.InnerHtml = divBuilder.ToString();

            this.textWriter.Write(aBuilder.ToString());
        }

        private string GetColorCssClass(MetroColor color)
        {
            switch (color)
            {
                case MetroColor.Black: return "blackbg";
                case MetroColor.Blue: return "bluebg";
                case MetroColor.Brown: return "brownbg";
                case MetroColor.Green: return "greenbg";
                case MetroColor.Lime: return "limebg";
                case MetroColor.Magenta: return "magentabg";
                case MetroColor.Orange: return "orangebg";
                case MetroColor.Pink: return "pinkbg";
                case MetroColor.Purple: return "purplebg";
                case MetroColor.Red: return "redbg";
                case MetroColor.Teal: return "tealbg";
                case MetroColor.White: return "whitebg";
                default: return null;
            }
        }

        private string GetSizeCssClass(MetroTileSize size)
        {
            switch (size)
            {
                //case MetroTileSize.DoubleBoth: throw new NotSupportedException(); //currently not supported with code52 metro
                case MetroTileSize.DoubleHorizontal: return "two-h";
                case MetroTileSize.DoubleVertical: return "two-v";
                case MetroTileSize.Single:
                default: return "one";
            }
        }

        public void Dispose()
        {
            this.textWriter.Write("</div>");
        }
    }
}