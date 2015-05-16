using System.IO;
using System.Web.Mvc;

namespace Kore.Web.Mvc.KoreUI
{
    public interface IModalProvider
    {
        string ModalTag { get; }

        void BeginModal(Modal modal);

        void BeginModalSectionPanel(ModalSection section, TextWriter writer, string title = null);

        void EndModal(Modal modal);

        void EndModalSectionPanel(ModalSection section, TextWriter writer);

        MvcHtmlString ModalLaunchButton(string modalId, string text, object htmlAttributes = null);

        MvcHtmlString ModalCloseButton(string modalId, string text, object htmlAttributes = null);
    }
}