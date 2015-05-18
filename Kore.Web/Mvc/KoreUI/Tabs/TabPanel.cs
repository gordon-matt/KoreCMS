using System;
using System.IO;

namespace Kore.Web.Mvc.KoreUI
{
    public class TabPanel : IDisposable
    {
        public string Id { get; private set; }

        public bool IsActive { get; private set; }

        private readonly TextWriter textWriter;

        internal TabPanel(TextWriter writer, string id, bool isActive = false)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("id");
            }

            this.Id = id;
            this.IsActive = isActive;
            this.textWriter = writer;
            KoreUISettings.Provider.TabsProvider.BeginTabPanel(this, this.textWriter);
        }

        public void Dispose()
        {
            KoreUISettings.Provider.TabsProvider.EndTabPanel(this.textWriter);
        }
    }
}