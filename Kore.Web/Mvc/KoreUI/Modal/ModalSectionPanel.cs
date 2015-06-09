using System;
using System.IO;
using System.Web.Mvc;
using Kore.Web.Mvc.KoreUI.Providers;

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
        private readonly IKoreUIProvider provider;

        public ModalSection Section { get; private set; }

        internal ModalSectionPanel(IKoreUIProvider provider, ModalSection section, TextWriter writer, string title = null)
        {
            this.provider = provider;
            this.Section = section;
            this.textWriter = writer;
            provider.ModalProvider.BeginModalSectionPanel(this.Section, this.textWriter, title);
        }

        public MvcHtmlString ModalCloseButton(string modalId, string text, object htmlAttributes = null)
        {
            return provider.ModalProvider.ModalCloseButton(modalId, text, htmlAttributes);
        }

        public void Dispose()
        {
            provider.ModalProvider.EndModalSectionPanel(this.Section, this.textWriter);
        }
    }
}