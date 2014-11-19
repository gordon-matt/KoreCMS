using System;
using System.IO;

namespace Kore.Web.Mvc.Bootstrap.Version2
{
    public class CarouselCaptionPanel : IDisposable
    {
        private readonly TextWriter textWriter;

        internal CarouselCaptionPanel(TextWriter writer)
        {
            this.textWriter = writer;
            this.textWriter.Write(@"<div class=""carousel-caption"">");
        }

        public void Dispose()
        {
            this.textWriter.Write("</div></div>");
        }
    }
}