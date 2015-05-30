using System;
using System.IO;

namespace Kore.Web.Mvc.KoreUI
{
    public enum PanelSectionType
    {
        Heading,
        Body
    }

    public class PanelSection : IDisposable
    {
        private readonly TextWriter textWriter;

        public PanelSectionType SectionType { get; private set; }

        internal PanelSection(PanelSectionType sectionType, TextWriter writer, string title = null)
        {
            this.SectionType = sectionType;
            this.textWriter = writer;
            KoreUISettings.Provider.PanelProvider.BeginPanelSection(this.SectionType, this.textWriter, title);
        }

        public void Dispose()
        {
            KoreUISettings.Provider.PanelProvider.EndPanelSection(this.SectionType, this.textWriter);
        }
    }
}