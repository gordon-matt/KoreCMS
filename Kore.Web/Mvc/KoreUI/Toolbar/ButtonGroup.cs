using System;
using System.IO;

namespace Kore.Web.Mvc.KoreUI
{
    public class ButtonGroup : IDisposable
    {
        private readonly TextWriter textWriter;

        internal ButtonGroup(TextWriter writer)
        {
            this.textWriter = writer;
            KoreUISettings.Provider.ToolbarProvider.BeginButtonGroup(this.textWriter);
        }

        public void Button(string text, State state, string onClick = null, object htmlAttributes = null)
        {
            KoreUISettings.Provider.ToolbarProvider.AddButton(this.textWriter, text, state, onClick, htmlAttributes);
        }

        public void Dispose()
        {
            KoreUISettings.Provider.ToolbarProvider.EndButtonGroup(this.textWriter);
        }
    }
}