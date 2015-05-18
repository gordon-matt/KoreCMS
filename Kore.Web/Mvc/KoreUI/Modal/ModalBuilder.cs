using System.Web.Mvc;

namespace Kore.Web.Mvc.KoreUI
{
    public class ModalBuilder<TModel> : BuilderBase<TModel, Modal>
    {
        internal ModalBuilder(HtmlHelper<TModel> htmlHelper, Modal modal)
            : base(htmlHelper, modal)
        {
        }

        public ModalSectionPanel BeginHeader(string title)
        {
            return new ModalSectionPanel(ModalSection.Header, base.textWriter, title);
        }

        public ModalSectionPanel BeginBody()
        {
            return new ModalSectionPanel(ModalSection.Body, base.textWriter);
        }

        public ModalSectionPanel BeginFooter()
        {
            return new ModalSectionPanel(ModalSection.Footer, base.textWriter);
        }

        public override void Dispose()
        {
            KoreUISettings.Provider.ModalProvider.EndModal(base.element);
            base.Dispose();
        }
    }
}