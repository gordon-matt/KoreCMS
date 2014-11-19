using System.Collections.Generic;
using System.Web.Mvc;

namespace Kore.Web.Mvc.Bootstrap.Version2
{
    public class BootstrapForm : HtmlElement
    {
        public BootstrapFormType FormType { get; set; }

        public BootstrapForm(BootstrapFormType formType, string formAction, FormMethod method, IDictionary<string, object> htmlAttributes)
            : base("form", htmlAttributes)
        {
            this.FormType = formType;

            EnsureHtmlAttribute("action", formAction);
            EnsureHtmlAttribute("method", HtmlHelper.GetFormMethodString(method), true);

            switch (this.FormType)
            {
                case BootstrapFormType.Horizontal: EnsureClass("form-horizontal"); break;
                case BootstrapFormType.Vertical: EnsureClass("form-vertical"); break;
                case BootstrapFormType.Inline: EnsureClass("form-inline"); break;
                case BootstrapFormType.Search: EnsureClass("form-search"); break;
            }
        }
    }
}