using System;
using System.IO;

namespace Kore.Web.Common.Html
{
    public class Tile : IDisposable
    {
        private readonly TextWriter textWriter;

        internal Tile(TextWriter writer, string href, MetroColor color, MetroTileSize size)
        {
            this.textWriter = writer;

            this.textWriter.Write("<a href=\"");
            this.textWriter.Write(href);
            this.textWriter.Write("\" ><div class=\"tile ");
            this.textWriter.Write(GetColorCssClass(color));
            this.textWriter.Write(" ");
            this.textWriter.Write(GetSizeCssClass(size));
            this.textWriter.Write("\" >");
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

        public void Dispose()
        {
            this.textWriter.Write("</div></a>");
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
    }
}