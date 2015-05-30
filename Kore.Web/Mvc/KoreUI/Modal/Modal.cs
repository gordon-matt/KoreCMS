using System;
using System.Web.Mvc;

namespace Kore.Web.Mvc.KoreUI
{
    public class Modal : HtmlElement
    {
        public string Id { get; private set; }

        public Modal()
            : this(null, null)
        {
        }

        public Modal(string id)
            : this(id, null)
        {
        }

        public Modal(string id, object htmlAttributes)
            : base(KoreUISettings.Provider.ModalProvider.ModalTag, htmlAttributes)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = "modal-" + Guid.NewGuid();
            }
            this.Id = HtmlHelper.GenerateIdFromName(id);
            EnsureHtmlAttribute("id", this.Id);
            KoreUISettings.Provider.ModalProvider.BeginModal(this);
        }
    }
}