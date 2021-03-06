﻿using System.Collections.Generic;
using System.Web.Mvc;

namespace Kore.Web.Mvc.KoreUI
{
    public abstract class HtmlElement
    {
        // Fields
        protected readonly IDictionary<string, object> htmlAttributes;

        protected string tag;

        // Methods
        public HtmlElement(string tag, object htmlAttributes)
        {
            this.tag = tag;
            this.htmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
        }

        // Properties
        internal string EndTag
        {
            get
            {
                return string.Format("</{0}>", this.tag);
            }
        }

        internal virtual string StartTag
        {
            get
            {
                var builder = new TagBuilder(this.tag);
                builder.MergeAttributes<string, object>(this.htmlAttributes);
                return builder.ToString(TagRenderMode.StartTag);
            }
        }

        public void EnsureClass(string className)
        {
            EnsureHtmlAttribute("class", className, false);
        }

        public void EnsureHtmlAttribute(string key, string value, bool replaceExisting = true)
        {
            if (this.htmlAttributes.ContainsKey(key))
            {
                if (replaceExisting)
                {
                    this.htmlAttributes[key] = value;
                }
                else
                {
                    this.htmlAttributes[key] += " " + value;
                }
            }
            else
            {
                this.htmlAttributes.Add(key, value);
            }
        }
    }
}