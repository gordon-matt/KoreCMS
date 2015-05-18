using System.IO;

namespace Kore.Web.Mvc.KoreUI
{
    public class Bootstrap3ToolbarProvider : IToolbarProvider
    {
        #region IToolbarProvider Members

        public string ToolbarTag
        {
            get { return "div"; }
        }

        public void BeginToolbar(Toolbar toolbar)
        {
            toolbar.EnsureClass("btn-toolbar");
            toolbar.EnsureHtmlAttribute("role", "toolbar");
        }

        public void BeginButtonGroup(TextWriter writer)
        {
            writer.Write(@"<div class=""btn-group"">");
        }

        public void EndButtonGroup(TextWriter writer)
        {
            writer.Write("</div>");
        }

        public void AddButton(TextWriter writer, string text, State state, string onClick = null, object htmlAttributes = null)
        {
            var button = KoreUISettings.Provider.Button(text, state, onClick, htmlAttributes);
            writer.Write(button.ToString());
        }

        #endregion IToolbarProvider Members
    }
}