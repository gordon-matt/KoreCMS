using System.Web.Mvc;

namespace Kore.Web.Mvc.KoreUI
{
    public class Tabs : HtmlElement
    {
        public string Id { get; private set; }

        public TabPosition Position { get; set; }

        public Tabs()
            : this(null, TabPosition.Top, null)
        {
        }

        public Tabs(string id)
            : this(id, TabPosition.Top, null)
        {
        }

        public Tabs(string id, TabPosition position)
            : this(id, position, null)
        {
        }

        public Tabs(string id, TabPosition position, object htmlAttributes)
            : base(KoreUISettings.Provider.TabsProvider.TabsTag, htmlAttributes)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = "tabs";
            }

            this.Position = position;
            this.Id = HtmlHelper.GenerateIdFromName(id);
            EnsureHtmlAttribute("id", this.Id);
            KoreUISettings.Provider.TabsProvider.BeginTabs(this);
        }
    }
}