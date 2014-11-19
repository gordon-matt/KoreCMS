using System;
using System.Web.Mvc;

namespace Kore.Web.Mvc.Bootstrap.Version3
{
    public class ModalBuilder<TModel> : BuilderBase<TModel, Modal>, IDisposable
    {
        internal ModalBuilder(HtmlHelper<TModel> htmlHelper, Modal modal)
            : base(htmlHelper, modal)
        {
            base.textWriter.Write(@"<div class=""modal-dialog""><div class=""modal-content"">");
        }

        public ModalSectionPanel BeginHeader(string title = null, bool hasCloseButton = true)
        {
            return new ModalSectionPanel(ModalSection.Header, base.textWriter, title, hasCloseButton);
        }

        public ModalSectionPanel BeginBody()
        {
            return new ModalSectionPanel(ModalSection.Body, base.textWriter);
        }

        public ModalSectionPanel BeginFooter()
        {
            return new ModalSectionPanel(ModalSection.Footer, base.textWriter);
        }

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            this.textWriter.Write("</div></div>");
        }

        #endregion IDisposable Members
    }
}