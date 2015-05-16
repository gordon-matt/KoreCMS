using System.IO;
using Kore.Web.Mvc.KoreUI.Providers;

namespace Kore.Web.Mvc.KoreUI
{
    public class KendoUITabsProvider : ITabsProvider
    {
        private readonly KendoBootstrap3UIProvider uiProvider;

        public KendoUITabsProvider(KendoBootstrap3UIProvider uiProvider)
        {
            this.uiProvider = uiProvider;
        }

        #region ITabsProvider Members

        public string TabsTag
        {
            get { return "div"; }
        }

        public void BeginTabs(Tabs tabs)
        {
            uiProvider.Scripts.Add(string.Format(
@"$('#{0}').kendoTabStrip({{
    animation:  {{
        open: {{
            effects: 'fadeIn'
        }}
    }}
}});", tabs.Id));
        }

        public void BeginTabsHeader(TextWriter writer)
        {
            writer.Write("<ul>");
        }

        public void BeginTabContent(TextWriter writer)
        {
        }

        public void BeginTabPanel(TabPanel panel, TextWriter writer)
        {
            writer.Write("<div>");
        }

        public void EndTabPanel(TextWriter writer)
        {
            writer.Write("</div>");
        }

        public void EndTabsHeader(TextWriter writer)
        {
            writer.Write("</ul>");
        }

        public void EndTabs(TextWriter writer)
        {
            writer.Write("</div>");
        }

        public void WriteTab(TextWriter writer, string label, string tabId, bool isActive)
        {
            if (isActive)
            {
                writer.Write(string.Format(
                    @"<li role=""tab"" aria-controls=""{0}"" class=""k-state-active"">{1}</li>",
                    tabId,
                    label));
            }
            else
            {
                writer.Write(string.Format(
                    @"<li role=""tab"" aria-controls=""{0}"">{1}</li>",
                    tabId,
                    label));
            }
        }

        #endregion ITabsProvider Members
    }
}