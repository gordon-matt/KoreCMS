using System.Web.Mvc;

namespace Kore.Web.Mvc.KoreUI
{
    public class PanelBuilder<TModel> : BuilderBase<TModel, Panel>
    {
        internal PanelBuilder(HtmlHelper<TModel> htmlHelper, Panel panel)
            : base(htmlHelper, panel)
        {
        }

        public PanelSection BeginHeader(string title)
        {
            return new PanelSection(PanelSectionType.Heading, base.textWriter, title);
        }

        public PanelSection BeginBody()
        {
            return new PanelSection(PanelSectionType.Body, base.textWriter);
        }

        public override void Dispose()
        {
            KoreUISettings.Provider.PanelProvider.EndPanel(base.element);
            base.Dispose();
        }
    }
}