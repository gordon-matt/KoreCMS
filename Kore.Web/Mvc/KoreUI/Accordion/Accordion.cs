using System.Web.Mvc;

namespace Kore.Web.Mvc.KoreUI
{
    public class Accordion : HtmlElement
    {
        public string Id { get; private set; }

        public Accordion(string id)
            : this(id, null)
        {
        }

        public Accordion(string id, object htmlAttributes)
            : base(KoreUISettings.Provider.AccordionProvider.AccordionTag, htmlAttributes)
        {
            this.Id = HtmlHelper.GenerateIdFromName(id);
            EnsureHtmlAttribute("id", this.Id);
            KoreUISettings.Provider.AccordionProvider.BeginAccordion(this);
        }
    }
}