using System.IO;
using System.Web.Mvc;

namespace Kore.Web.Mvc.KoreUI
{
    public interface IModalProvider
    {
        void BeginModal(Modal modal, TextWriter writer);

        void BeginModalSectionPanel(ModalSection section, TextWriter writer, string title = null);

        void EndModal(Modal modal, TextWriter writer);

        void EndModalSectionPanel(ModalSection section, TextWriter writer);

        MvcHtmlString ModalLaunchButton(string modalId, string text, object htmlAttributes = null);

        MvcHtmlString ModalCloseButton(string modalId, string text, object htmlAttributes = null);
    }
}