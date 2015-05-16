using System.Web.Mvc;

namespace Kore.Web.Mvc.KoreUI
{
    public class AccordionBuilder<TModel> : BuilderBase<TModel, Accordion>
    {
        internal AccordionBuilder(HtmlHelper<TModel> htmlHelper, Accordion accordion)
            : base(htmlHelper, accordion)
        {
        }

        public AccordionPanel BeginPanel(string title, string id, bool expanded = false)
        {
            return new AccordionPanel(base.textWriter, title, id, base.element.Id, expanded);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}