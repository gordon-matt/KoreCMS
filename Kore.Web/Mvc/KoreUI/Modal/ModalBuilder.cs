﻿using System.Web.Mvc;

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
            return new ModalSectionPanel(base.element.Provider, ModalSection.Header, base.textWriter, title);
        }

        public ModalSectionPanel BeginBody()
        {
            return new ModalSectionPanel(base.element.Provider, ModalSection.Body, base.textWriter);
        }

        public ModalSectionPanel BeginFooter()
        {
            return new ModalSectionPanel(base.element.Provider, ModalSection.Footer, base.textWriter);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}