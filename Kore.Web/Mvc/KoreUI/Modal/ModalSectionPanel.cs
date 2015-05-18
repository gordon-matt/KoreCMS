using System;
using System.IO;
using System.Web.Mvc;

namespace Kore.Web.Mvc.KoreUI
{
    public enum ModalSection
    {
        Header,
        Body,
        Footer
    }

    public class ModalSectionPanel : IDisposable
    {
        private readonly TextWriter textWriter;

        public ModalSection Section { get; private set; }

        internal ModalSectionPanel(ModalSection section, TextWriter writer, string title = null)
        {
            this.Section = section;
            this.textWriter = writer;
            KoreUISettings.Provider.ModalProvider.BeginModalSectionPanel(this.Section, this.textWriter, title);
        }

        public MvcHtmlString ModalCloseButton(string modalId, string text, object htmlAttributes = null)
        {
            return KoreUISettings.Provider.ModalProvider.ModalCloseButton(modalId, text, htmlAttributes);
        }

        public void Dispose()
        {
            KoreUISettings.Provider.ModalProvider.EndModalSectionPanel(this.Section, this.textWriter);
        }
    }
}