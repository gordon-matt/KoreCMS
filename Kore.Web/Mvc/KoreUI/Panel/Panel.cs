using System;
using System.Web.Mvc;

namespace Kore.Web.Mvc.KoreUI
{
    public class Panel : HtmlElement
    {
        public string Id { get; private set; }

        public State State { get; private set; }

        public Panel(string id = null, State state = State.Primary, object htmlAttributes = null)
            : base(KoreUISettings.Provider.PanelProvider.PanelTag, htmlAttributes)
        {
            if (id == null)
            {
                id = "panel-" + Guid.NewGuid();
            }
            this.Id = HtmlHelper.GenerateIdFromName(id);
            this.State = state;
            EnsureHtmlAttribute("id", this.Id);
            KoreUISettings.Provider.PanelProvider.BeginPanel(this);
        }
    }
}