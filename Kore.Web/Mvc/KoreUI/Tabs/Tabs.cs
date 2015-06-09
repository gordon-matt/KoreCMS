using System;
using System.IO;
using System.Web.Mvc;

namespace Kore.Web.Mvc.KoreUI
{
    public class Tabs : HtmlElement
    {
        public string Id { get; private set; }

        public TabPosition Position { get; set; }

        public Tabs(string id = null, TabPosition position = TabPosition.Top, object htmlAttributes = null)
            : base(htmlAttributes)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = "tabs-" + Guid.NewGuid();
            }

            this.Position = position;
            this.Id = HtmlHelper.GenerateIdFromName(id);
            EnsureHtmlAttribute("id", this.Id);
        }

        protected internal override void StartTag(TextWriter textWriter)
        {
            Provider.TabsProvider.BeginTabs(this, textWriter);
        }

        protected internal override void EndTag(TextWriter textWriter)
        {
            Provider.TabsProvider.EndTabs(this, textWriter);
        }
    }
}