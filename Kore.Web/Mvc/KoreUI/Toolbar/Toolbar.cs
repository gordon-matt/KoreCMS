using System;
using System.Web.Mvc;

namespace Kore.Web.Mvc.KoreUI
{
    public class Toolbar : HtmlElement
    {
        public string Id { get; private set; }

        public Toolbar(string id)
            : this(id, null)
        {
        }

        public Toolbar(string id, object htmlAttributes)
            : base(KoreUISettings.Provider.ToolbarProvider.ToolbarTag, htmlAttributes)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = "toolbar-" + Guid.NewGuid();
            }

            this.Id = HtmlHelper.GenerateIdFromName(id);
            EnsureHtmlAttribute("id", this.Id);
            KoreUISettings.Provider.ToolbarProvider.BeginToolbar(this);
        }
    }
}