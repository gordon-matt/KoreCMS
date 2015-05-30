using System.Web.Mvc;

namespace Kore.Web.Mvc.KoreUI.Providers
{
    public interface IKoreUIProvider
    {
        MvcHtmlString RenderScripts();

        #region General

        MvcHtmlString ActionLink(HtmlHelper html, string text, State state, string actionName, string controllerName, object routeValues = null, object htmlAttributes = null);

        MvcHtmlString Badge(string text, object htmlAttributes = null);

        MvcHtmlString Button(string text, State state, string onClick = null, object htmlAttributes = null);

        MvcHtmlString InlineLabel(string text, State state, object htmlAttributes = null);

        MvcHtmlString Quote(string text, string author, string titleOfWork, object htmlAttributes = null);

        MvcHtmlString SubmitButton(string text, State state, object htmlAttributes = null);

        MvcHtmlString TextBoxWithAddOns(HtmlHelper html, string name, object value, string prependValue, string appendValue, object htmlAttributes = null);

        #endregion General

        #region Special

        IAccordionProvider AccordionProvider { get; }

        IModalProvider ModalProvider { get; }

        IPanelProvider PanelProvider { get; }

        ITabsProvider TabsProvider { get; }

        IThumbnailProvider ThumbnailProvider { get; }

        IToolbarProvider ToolbarProvider { get; }

        #endregion Special
    }
}