using System.IO;
using System.Web.Mvc;

namespace Kore.Web.Mvc.KoreUI
{
    public class Bootstrap3TabsProvider : ITabsProvider
    {
        #region ITabsProvider Members

        public string TabsTag
        {
            get { return "div"; }
        }

        public void BeginTabs(Tabs tabs)
        {
            tabs.EnsureHtmlAttribute("role", "tabpanel");

            switch (tabs.Position)
            {
                case TabPosition.Left: tabs.EnsureClass("tabbable tabs-left"); break;
                case TabPosition.Right: tabs.EnsureClass("tabbable tabs-right"); break;
                //case TabPosition.Bottom: tabs.EnsureClass("tabbable tabs-bottom"); break;
            }
        }

        public void BeginTabsHeader(TextWriter writer)
        {
            writer.Write(@"<ul class=""nav nav-tabs"" role=""tablist"">");
        }

        public void BeginTabContent(TextWriter writer)
        {
            writer.Write(@"<div class=""tab-content"">");
        }

        public void BeginTabPanel(TabPanel panel, TextWriter writer)
        {
            var builder = new TagBuilder("div");
            builder.MergeAttribute("id", panel.Id);
            builder.MergeAttribute("role", "tabpanel");
            builder.AddCssClass("tab-pane");

            if (panel.IsActive)
            {
                builder.AddCssClass("active");
            }

            writer.Write(builder.ToString(TagRenderMode.StartTag));
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
                    @"<li role=""presentation"" class=""active""><a href=""#{0}"" aria-controls=""{0}"" role=""tab"" data-toggle=""tab"">{1}</a></li>",
                    tabId,
                    label));
            }
            else
            {
                writer.Write(string.Format(
                    @"<li role=""presentation""><a href=""#{0}"" aria-controls=""{0}"" role=""tab"" data-toggle=""tab"">{1}</a></li>",
                    tabId,
                    label));
            }
        }

        #endregion ITabsProvider Members
    }
}