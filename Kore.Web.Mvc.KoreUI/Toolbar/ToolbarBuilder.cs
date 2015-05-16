using System.Web.Mvc;

namespace Kore.Web.Mvc.KoreUI
{
    public class ToolbarBuilder<TModel> : BuilderBase<TModel, Toolbar>
    {
        internal ToolbarBuilder(HtmlHelper<TModel> htmlHelper, Toolbar toolbar)
            : base(htmlHelper, toolbar)
        {
        }

        public ButtonGroup BeginButtonGroup()
        {
            return new ButtonGroup(base.textWriter);
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}