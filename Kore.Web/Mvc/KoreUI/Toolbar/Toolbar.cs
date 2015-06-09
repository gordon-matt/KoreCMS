using System;
using System.IO;
using System.Web.Mvc;

namespace Kore.Web.Mvc.KoreUI
{
    public class Toolbar : HtmlElement
    {
        public string Id { get; private set; }

        public Toolbar(string id = null, object htmlAttributes = null)
            : base(htmlAttributes)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = "toolbar-" + Guid.NewGuid();
            }

            this.Id = HtmlHelper.GenerateIdFromName(id);
            EnsureHtmlAttribute("id", this.Id);
        }

        protected internal override void StartTag(TextWriter textWriter)
        {
            Provider.ToolbarProvider.BeginToolbar(this, textWriter);
        }

        protected internal override void EndTag(TextWriter textWriter)
        {
            Provider.ToolbarProvider.EndToolbar(this, textWriter);
        }
    }
}