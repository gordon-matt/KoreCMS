﻿using System.Web.Mvc;

namespace Kore.Web.Mvc.Bootstrap.Version2
{
    public class AccordionBuilder<TModel> : BuilderBase<TModel, Accordion>
    {
        internal AccordionBuilder(HtmlHelper<TModel> htmlHelper, Accordion accordion)
            : base(htmlHelper, accordion)
        {
        }

        public AccordionPanel BeginPanel(string title, string id)
        {
            return new AccordionPanel(base.textWriter, title, id, base.element.Id);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}