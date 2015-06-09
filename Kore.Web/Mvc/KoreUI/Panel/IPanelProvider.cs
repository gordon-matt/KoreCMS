using System.IO;

namespace Kore.Web.Mvc.KoreUI
{
    public interface IPanelProvider
    {
        void BeginPanel(Panel panel, TextWriter writer);

        void BeginPanelSection(PanelSectionType sectionType, TextWriter writer, string title = null);

        void EndPanel(Panel panel, TextWriter writer);

        void EndPanelSection(PanelSectionType sectionType, TextWriter writer);
    }
}