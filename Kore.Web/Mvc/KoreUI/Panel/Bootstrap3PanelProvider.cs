using System.IO;

namespace Kore.Web.Mvc.KoreUI
{
    public class Bootstrap3PanelProvider : IPanelProvider
    {
        #region IPanelProvider Members

        public string PanelTag
        {
            get { return "div"; }
        }

        public void BeginPanel(Panel panel)
        {
            switch (panel.State)
            {
                case State.Default: panel.EnsureClass("panel panel-default"); break;
                case State.Important: panel.EnsureClass("panel panel-danger"); break;
                case State.Info: panel.EnsureClass("panel panel-info"); break;
                case State.Inverse: panel.EnsureClass("panel panel-inverse"); break;
                case State.Primary: panel.EnsureClass("panel panel-primary"); break;
                case State.Success: panel.EnsureClass("panel panel-success"); break;
                case State.Warning: panel.EnsureClass("panel panel-warning"); break;
            }
        }

        public void BeginPanelSection(PanelSectionType sectionType, TextWriter writer, string title = null)
        {
            switch (sectionType)
            {
                case PanelSectionType.Heading:
                    {
                        writer.Write(string.Format(
@"<div class=""panel-heading"">
    <h3 class=""panel-title"">{0}</h3>", title));
                    }
                    break;

                case PanelSectionType.Body: writer.Write(@"<div class=""panel-body"">"); break;
            }
        }

        public void EndPanel(Panel panel)
        {
        }

        public void EndPanelSection(PanelSectionType sectionType, TextWriter writer)
        {
            writer.Write("</div>");
        }

        #endregion IPanelProvider Members
    }
}