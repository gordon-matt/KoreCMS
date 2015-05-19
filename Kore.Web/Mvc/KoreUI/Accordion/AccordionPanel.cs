using System;
using System.IO;

namespace Kore.Web.Mvc.KoreUI
{
    public class AccordionPanel : IDisposable
    {
        private readonly TextWriter textWriter;

        internal AccordionPanel(TextWriter writer, string title, string panelId, string parentAccordionId, bool expanded = false)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }

            this.textWriter = writer;

            KoreUISettings.Provider.AccordionProvider.BeginAccordionPanel(this.textWriter, title, panelId, parentAccordionId, expanded);
        }

        public void Dispose()
        {
            KoreUISettings.Provider.AccordionProvider.EndAccordionPanel(this.textWriter);
        }
    }
}