﻿using System.Web.Mvc;

namespace Kore.Web.Mvc.Bootstrap.Version2
{
    public class Accordion : HtmlElement
    {
        public string Id { get; set; }

        public Accordion(string id)
            : this(id, null)
        {
        }

        public Accordion(string id, object htmlAttributes)
            : base("div", htmlAttributes)
        {
            this.Id = HtmlHelper.GenerateIdFromName(id);
            EnsureClass("accordion");
            EnsureHtmlAttribute("id", this.Id);
        }
    }
}