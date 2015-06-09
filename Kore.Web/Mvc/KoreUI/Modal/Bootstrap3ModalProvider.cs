using System.IO;
using System.Web.Mvc;

namespace Kore.Web.Mvc.KoreUI
{
    public class Bootstrap3ModalProvider : IModalProvider
    {
        #region IModalProvider Members

        public void BeginModal(Modal modal, TextWriter writer)
        {
            modal.EnsureClass("modal fade");
            modal.EnsureHtmlAttribute("tabindex", "-1");
            modal.EnsureHtmlAttribute("role", "dialog");
            modal.EnsureHtmlAttribute("aria-labelledby", modal.Id + "-label");
            modal.EnsureHtmlAttribute("aria-hidden", "true");

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
                        writer.Write(@"<div class=""modal-dialog""><div class=""modal-content"">");

                        //var builder = new FluentTagBuilder("div", TagRenderMode.StartTag)
                        //    .AddCssClass("modal-dialog")
                        //    .StartTag("div", TagRenderMode.StartTag)
                        //        .AddCssClass("modal-content")
                        //    .EndTag();

                        //writer.Write(builder.ToString());

                        writer.Write(@"<div class=""modal-header"">");

                        writer.Write(string.Format(
@"<button type=""button"" class=""close"" data-dismiss=""modal"" aria-label=""Close"">
    <span aria-hidden=""true"">&times;</span>
</button>
<h4 class=""modal-title"">{0}</h4>", title));
                    }
                    break;

                case ModalSection.Body: writer.Write(@"<div class=""modal-body"">"); break;
                case ModalSection.Footer: writer.Write(@"<div class=""modal-footer"">"); break;
            }
        }

        public void EndModalSectionPanel(ModalSection section, TextWriter writer)
        {
            if (section == ModalSection.Footer)
            {
                writer.Write("</div></div></div>");
            }
            else
            {
                writer.Write("</div>");
            }
        }

        public void EndModal(Modal modal, TextWriter writer)
        {
            writer.Write("</div>");
        }

        public MvcHtmlString ModalLaunchButton(string modalId, string text, object htmlAttributes = null)
        {
            var builder = new FluentTagBuilder("button")
                .AddCssClass("btn btn-primary")
                .MergeAttribute("type", "button")
                .MergeAttribute("data-toggle", "modal")
                .MergeAttribute("data-target", "#" + modalId)
                .SetInnerText(text);

            return MvcHtmlString.Create(builder.ToString());
        }

        public MvcHtmlString ModalCloseButton(string modalId, string text, object htmlAttributes = null)
        {
            var builder = new FluentTagBuilder("button")
                .AddCssClass("btn btn-default")
                .MergeAttribute("type", "button")
                .MergeAttribute("data-dismiss", "modal")
                .SetInnerText(text);

            return MvcHtmlString.Create(builder.ToString());
        }

        #endregion IModalProvider Members
    }
}