﻿using System.IO;
using System.Web.Mvc;
using Kore.Web.Mvc.KoreUI.Providers;

namespace Kore.Web.Mvc.KoreUI
{
    public class KendoUIAccordionProvider : IAccordionProvider
    {
        private readonly KendoBootstrap3UIProvider uiProvider;

        public KendoUIAccordionProvider(KendoBootstrap3UIProvider uiProvider)
        {
            this.uiProvider = uiProvider;
        }

        #region IAccordionProvider Members

        public string AccordionTag
        {
            get { return "ul"; }
        }

        public void BeginAccordion(Accordion accordion)
        {
            uiProvider.Scripts.Add(string.Format(
@"$('#{0}').kendoPanelBar({{
    expandMode: 'single'
}});", accordion.Id));
        }

        public void BeginAccordionPanel(TextWriter writer, string title, string panelId, string parentAccordionId, bool expanded)
        {
            var li = new FluentTagBuilder("li", TagRenderMode.StartTag);

            if (expanded)
            {
                li.AddCssClass("k-state-active");
            }

            writer.Write(li.ToString());
            writer.Write(title);
            writer.Write(@"<div class=""k-content"">");
        }

        public void EndAccordionPanel(TextWriter writer)
        {
            writer.Write("</div></li>");
        }

        #endregion IAccordionProvider Members
    }
}