﻿using System;
using System.IO;
using System.Web.Mvc;
using Kore.Web.Mvc.KoreUI.Providers;

namespace Kore.Web.Mvc.KoreUI
{
    public class Accordion : HtmlElement
    {
        public string Id { get; private set; }

        public Accordion(string id = null, object htmlAttributes = null)
            : base(htmlAttributes)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = "accordion-" + Guid.NewGuid();
            }
            this.Id = HtmlHelper.GenerateIdFromName(id);
            EnsureHtmlAttribute("id", this.Id);
        }

        protected internal override void StartTag(TextWriter textWriter)
        {
            Provider.AccordionProvider.BeginAccordion(this, textWriter);
        }

        protected internal override void EndTag(TextWriter textWriter)
        {
            Provider.AccordionProvider.EndAccordion(this, textWriter);
        }
    }
}