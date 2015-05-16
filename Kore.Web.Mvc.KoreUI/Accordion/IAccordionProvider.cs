using System.IO;

namespace Kore.Web.Mvc.KoreUI
{
    public interface IAccordionProvider
    {
        string AccordionTag { get; }

        void BeginAccordion(Accordion accordion);

        void BeginAccordionPanel(TextWriter writer, string title, string panelId, string parentAccordionId, bool expanded);

        void EndAccordionPanel(TextWriter writer);
    }
}