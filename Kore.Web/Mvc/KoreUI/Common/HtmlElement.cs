﻿using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using Kore.Web.Mvc.KoreUI.Providers;

namespace Kore.Web.Mvc.KoreUI
{
    public abstract class HtmlElement
    {
        public IDictionary<string, object> HtmlAttributes { get; private set; }

        public HtmlElement(object htmlAttributes)
        {
            this.HtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
        }

        internal IKoreUIProvider Provider { get; set; }

        protected internal abstract void StartTag(TextWriter textWriter);

        protected internal abstract void EndTag(TextWriter textWriter);

        public void EnsureClass(string className)
        {
            EnsureHtmlAttribute("class", className, false);
        }

        public void EnsureHtmlAttribute(string key, string value, bool replaceExisting = true)
        {
            if (this.HtmlAttributes.ContainsKey(key))
            {
                if (replaceExisting)
                {
                    this.HtmlAttributes[key] = value;
                }
                else
                {
                    this.HtmlAttributes[key] += " " + value;
                }
            }
            else
            {
                this.HtmlAttributes.Add(key, value);
            }
        }
    }
}