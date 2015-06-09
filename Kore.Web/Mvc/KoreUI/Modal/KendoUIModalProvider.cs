using System.IO;
using System.Web.Mvc;
using Kore.Web.Mvc.KoreUI.Providers;

namespace Kore.Web.Mvc.KoreUI
{
    public class KendoUIModalProvider : IModalProvider
    {
        private string title;

        private readonly KendoBootstrap3UIProvider uiProvider;

        public KendoUIModalProvider(KendoBootstrap3UIProvider uiProvider)
        {
            this.uiProvider = uiProvider;
        }

        #region IModalProvider Members

        public void BeginModal(Modal modal, TextWriter writer)
        {
            var builder = new TagBuilder("div");
            builder.MergeAttributes<string, object>(modal.HtmlAttributes);
            string tag = builder.ToString(TagRenderMode.StartTag);

            writer.Write(tag);
        }

        public void BeginModalSectionPanel(ModalSection section, TextWriter writer, string title = null)
        {
            switch (section)
            {
                case ModalSection.Header:
                    {
                        this.title = title;
                        writer.Write(@"<div class=""modal-header"">");
                    }
                    break;

                case ModalSection.Body: writer.Write(@"<div class=""modal-body"">"); break;
                case ModalSection.Footer: writer.Write(@"<div class=""modal-footer"">"); break;
            }
        }

        public void EndModal(Modal modal, TextWriter writer)
        {
            uiProvider.Scripts.Add(string.Format(
@"$('#{0}').kendoWindow({{
    actions: ['Maximize', 'Minimize', 'Close'],
    modal: true,
    draggable: true,
    resizable: true,
    title: '{1}',
    width: '500px',
    height: '300px',
    visible: false
}}).data('kendoWindow').center();", modal.Id, title));
            
            writer.Write("</div>");
        }

        public void EndModalSectionPanel(ModalSection section, TextWriter writer)
        {
            writer.Write("</div>");
        }

        public MvcHtmlString ModalLaunchButton(string modalId, string text, object htmlAttributes = null)
        {
            var builder = new FluentTagBuilder("span")
                .GenerateId(modalId + "-launch-button")
                .AddCssClass("k-button")
                .SetInnerText(text);

            uiProvider.Scripts.Add(string.Format(
@"$('#{0}-launch-button')
.bind('click', function() {{
    $('#{0}').data('kendoWindow').open();
}});", modalId));

            return MvcHtmlString.Create(builder.ToString());
        }

        public MvcHtmlString ModalCloseButton(string modalId, string text, object htmlAttributes = null)
        {
            var builder = new FluentTagBuilder("span")
                .GenerateId(modalId + "-close-button")
                .AddCssClass("k-button")
                .SetInnerText(text);

            uiProvider.Scripts.Add(string.Format(
@"$('#{0}-close-button')
.bind('click', function() {{
    $('#{0}').data('kendoWindow').close();
}});", modalId));

            return MvcHtmlString.Create(builder.ToString());
        }

        #endregion IModalProvider Members
    }
}