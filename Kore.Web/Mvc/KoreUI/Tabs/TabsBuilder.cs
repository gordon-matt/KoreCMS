using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Kore.Web.Mvc.KoreUI
{
    public class TabsBuilder<TModel> : BuilderBase<TModel, Tabs>
    {
        private bool isHeaderClosed;
        private Queue<string> tabIds;
        private bool writingContent;
        private string activeTabId;

        private bool isFirstTab = true;

        internal TabsBuilder(HtmlHelper<TModel> htmlHelper, Tabs tabs)
            : base(htmlHelper, tabs)
        {
            this.tabIds = new Queue<string>();
            this.isHeaderClosed = false;
            this.writingContent = false;
            KoreUISettings.Provider.TabsProvider.BeginTabsHeader(this.textWriter);
        }

        public TabPanel BeginPanel()
        {
            this.writingContent = true;
            this.CloseHeader();
            if (this.tabIds.Count == 0)
            {
                throw new InvalidOperationException("Tab definition not found. Use AddTab before creating a new panel.");
            }

            string tabId = this.tabIds.Dequeue();
            if (tabId == activeTabId)
            {
                KoreUISettings.Provider.TabsProvider.BeginTabContent(this.textWriter);
                isFirstTab = false;
                return new TabPanel(base.textWriter, tabId, true);
            }

            return new TabPanel(base.textWriter, tabId);
        }

        private void CheckBuilderState()
        {
            if (this.writingContent)
            {
                throw new InvalidOperationException("Tab definition cannot be mixed with content panels.");
            }
        }

        private void CloseHeader()
        {
            if (!this.isHeaderClosed)
            {
                KoreUISettings.Provider.TabsProvider.EndTabsHeader(this.textWriter);
                this.isHeaderClosed = true;
            }
        }

        public override void Dispose()
        {
            this.CloseHeader();

            // Close Tab Content Div:
            KoreUISettings.Provider.TabsProvider.EndTabs(this.textWriter);
            base.Dispose();
        }

        public void Tab(string label, string id)
        {
            if (string.IsNullOrWhiteSpace(label))
            {
                throw new ArgumentNullException("label");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("id");
            }

            this.CheckBuilderState();
            string tabId = HtmlHelper.GenerateIdFromName(id);
            this.tabIds.Enqueue(tabId);

            if (isFirstTab)
            {
                activeTabId = tabId;
                KoreUISettings.Provider.TabsProvider.WriteTab(this.textWriter, label, tabId, true);
                isFirstTab = false;
            }
            else
            {
                KoreUISettings.Provider.TabsProvider.WriteTab(this.textWriter, label, tabId, false);
            }
        }
    }
}