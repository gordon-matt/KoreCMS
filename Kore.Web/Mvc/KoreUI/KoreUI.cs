using System;
using System.Web.Mvc;

namespace Kore.Web.Mvc.KoreUI
{
    public class KoreUI<TModel>
    {
        private readonly HtmlHelper<TModel> html;

        internal KoreUI(HtmlHelper<TModel> html)
        {
            this.html = html;
        }

        public MvcHtmlString RenderScripts()
        {
            return KoreUISettings.Provider.RenderScripts();
        }

        #region Accordion

        public AccordionBuilder<TModel> Begin(Accordion accordion)
        {
            if (accordion == null)
            {
                throw new ArgumentNullException("accordion");
            }

            return new AccordionBuilder<TModel>(this.html, accordion);
        }

        #endregion Accordion

        #region Badge

        public MvcHtmlString Badge(string text, object htmlAttributes = null)
        {
            return KoreUISettings.Provider.Badge(text, htmlAttributes);
        }

        #endregion Badge

        #region Buttons

        public MvcHtmlString ModalLaunchButton(string modalId, string text, object htmlAttributes = null)
        {
            return KoreUISettings.Provider.ModalProvider.ModalLaunchButton(modalId, text, htmlAttributes);
        }

        public MvcHtmlString ActionLink(string text, State state, string actionName, string controllerName, object routeValues = null, object htmlAttributes = null)
        {
            return KoreUISettings.Provider.ActionLink(html, text, state, actionName, controllerName, routeValues, htmlAttributes);
        }

        public MvcHtmlString Button(string text, State state, string onClick = null, object htmlAttributes = null)
        {
            return KoreUISettings.Provider.Button(text, state, onClick, htmlAttributes);
        }

        public MvcHtmlString SubmitButton(string text, State state, object htmlAttributes = null)
        {
            return KoreUISettings.Provider.SubmitButton(text, state, htmlAttributes);
        }

        #endregion Buttons

        #region Inline Label

        public MvcHtmlString InlineLabel(string text, State state, object htmlAttributes = null)
        {
            return KoreUISettings.Provider.InlineLabel(text, state, htmlAttributes);
        }

        #endregion Inline Label

        #region Modal (Dialog)

        public ModalBuilder<TModel> Begin(Modal modal)
        {
            if (modal == null)
            {
                throw new ArgumentNullException("modal");
            }

            return new ModalBuilder<TModel>(this.html, modal);
        }

        #endregion Modal (Dialog)

        #region TextBoxWithAddOns

        public MvcHtmlString TextBoxWithAddOns(string name, object value, string prependValue, string appendValue, object htmlAttributes = null)
        {
            return KoreUISettings.Provider.TextBoxWithAddOns(html, name, value, prependValue, appendValue, htmlAttributes);
        }

        #endregion TextBoxWithAddOns

        #region Quotes

        public MvcHtmlString Quote(string text, string author, string titleOfWork, object htmlAttributes = null)
        {
            return KoreUISettings.Provider.Quote(text, author, titleOfWork, htmlAttributes);
        }

        #endregion Quotes

        #region Tabs

        public TabsBuilder<TModel> Begin(Tabs tabs)
        {
            if (tabs == null)
            {
                throw new ArgumentNullException("tabs");
            }

            return new TabsBuilder<TModel>(this.html, tabs);
        }

        #endregion Tabs

        #region Thumbnails

        public MvcHtmlString Thumbnail(string src, string alt, string href = null, object aHtmlAttributes = null, object imgHtmlAttributes = null)
        {
            return KoreUISettings.Provider.ThumbnailProvider.Thumbnail(html, src, alt, href, aHtmlAttributes, imgHtmlAttributes);
        }

        public ThumbnailBuilder<TModel> Begin(Thumbnail thumbnail)
        {
            if (thumbnail == null)
            {
                throw new ArgumentNullException("thumbnail");
            }

            return new ThumbnailBuilder<TModel>(this.html, thumbnail);
        }

        #endregion Thumbnails

        #region Toolbar

        public ToolbarBuilder<TModel> Begin(Toolbar toolbar)
        {
            if (toolbar == null)
            {
                throw new ArgumentNullException("toolbar");
            }

            return new ToolbarBuilder<TModel>(this.html, toolbar);
        }

        #endregion Toolbar
    }
}