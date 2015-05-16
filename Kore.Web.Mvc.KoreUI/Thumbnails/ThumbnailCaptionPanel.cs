using System;
using System.IO;

namespace Kore.Web.Mvc.KoreUI
{
    public class ThumbnailCaptionPanel : IDisposable
    {
        private readonly TextWriter textWriter;

        internal ThumbnailCaptionPanel(TextWriter writer)
        {
            this.textWriter = writer;
            KoreUISettings.Provider.ThumbnailProvider.BeginCaptionPanel(this.textWriter);
        }

        public void Dispose()
        {
            KoreUISettings.Provider.ThumbnailProvider.EndCaptionPanel(this.textWriter);
        }
    }
}