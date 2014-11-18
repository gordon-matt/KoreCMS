using System;
using System.IO;

namespace Kore.Web.Mvc.Bootstrap.Version3
{
    internal enum ModalSection
    {
        Header,
        Body,
        Footer
    }

    public class ModalSectionPanel : IDisposable
    {
        private readonly TextWriter textWriter;

        internal ModalSectionPanel(ModalSection section, TextWriter writer, string title = null, bool hasCloseButton = true)
        {
            this.textWriter = writer;

            switch (section)
            {
                case ModalSection.Header:
                    this.textWriter.Write(@"<div class=""modal-header"">");

                    if (hasCloseButton)
                    {
                        this.textWriter.Write(@"<button type=""button"" class=""close"" data-dismiss=""modal""><span aria-hidden=""true"">&times;</span><span class=""sr-only"">Close</span></button>");
                    }

                    if (!string.IsNullOrEmpty(title))
                    {
                        this.textWriter.Write(string.Format(@"<h4 class=""modal-title"">{0}</h4>", title));
                    }

                    break;

                case ModalSection.Body:
                    this.textWriter.Write(@"<div class=""modal-body"">");
                    break;

                case ModalSection.Footer:
                    this.textWriter.Write(@"<div class=""modal-footer"">");
                    break;
            }
        }

        public void Dispose()
        {
            this.textWriter.Write("</div>");
        }
    }
}