using System.IO;

namespace Kore.Web.Mvc.KoreUI
{
    public interface IPanelProvider
    {
        string PanelTag { get; }

        void BeginPanel(Panel panel);

        void BeginPanelSection(PanelSectionType sectionType, TextWriter writer, string title = null);

        void EndPanel(Panel panel);

        void EndPanelSection(PanelSectionType sectionType, TextWriter writer);
    }
}