using System.IO;

namespace Kore.Web.Mvc.KoreUI
{
    public interface ITabsProvider
    {
        string TabsTag { get; }

        void BeginTabs(Tabs tabs);

        void BeginTabsHeader(TextWriter writer);

        void BeginTabContent(TextWriter writer);

        void BeginTabPanel(TabPanel panel, TextWriter writer);

        void EndTabPanel(TextWriter writer);

        void EndTabsHeader(TextWriter writer);

        void EndTabs(TextWriter writer);

        void WriteTab(TextWriter writer, string label, string tabId, bool isActive);
    }
}