using System;
using System.Web.Mvc;
using Kore.Web.Mvc.KoreUI.Providers;

namespace Kore.Web.Mvc.KoreUI
{
    public class KoreUI<TModel>
    {
        private readonly HtmlHelper<TModel> html;
        private readonly IKoreUIProvider provider;

        internal KoreUI(HtmlHelper<TModel> html, IKoreUIProvider provider = null)
        {
            this.html = html;
            this.provider = provider ?? KoreUISettings.DefaultProvider;
        }

        public MvcHtmlString RenderScripts()
        {
            return provider.RenderScripts();
        }

        #region Accordion

        public AccordionBuilder<TModel> Begin(Accordion accordion)
        {
            if (accordion == null)
            {
                throw new ArgumentNullException("accordion");
            }

            accordion.Provider = provider;
            return new AccordionBuilder<TModel>(this.html, accordion);
        }

        #endregion Accordion

        #region Badge

        public MvcHtmlString Badge(string text, object htmlAttributes = null)
        {
            return provider.Badge(text, htmlAttributes);
        }

        #endregion Badge

        #region Buttons

        public MvcHtmlString ModalLaunchButton(string modalId, string text, object htmlAttributes = null)
        {
            return provider.ModalProvider.ModalLaunchButton(modalId, text, htmlAttributes);
        }

        public MvcHtmlString ActionLink(string text, State state, string actionName, string controllerName, object routeValues = null, object htmlAttributes = null)
        {
            return provider.ActionLink(html, text, state, actionName, controllerName, routeValues, htmlAttributes);
        }

        public MvcHtmlString Button(string text, State state, string onClick = null, object htmlAttributes = null)
        {
            return provider.Button(text, state, onClick, htmlAttributes);
        }

        public MvcHtmlString SubmitButton(string text, State state, object htmlAttributes = null)
        {
            return provider.SubmitButton(text, state, htmlAttributes);
        }

        #endregion Buttons

        #region Inline Label

        public MvcHtmlString InlineLabel(string text, State state, object htmlAttributes = null)
        {
            return provider.InlineLabel(text, state, htmlAttributes);
        }

        #endregion Inline Label

        #region Modal (Dialog)

        public ModalBuilder<TModel> Begin(Modal modal)
        {
            if (modal == null)
            {
                throw new ArgumentNullException("modal");
            }

            modal.Provider = provider;
            return new ModalBuilder<TModel>(this.html, modal);
        }

        #endregion Modal (Dialog)

        #region TextBoxWithAddOns

        public MvcHtmlString TextBoxWithAddOns(string name, object value, string prependValue, string appendValue, object htmlAttributes = null)
        {
            return provider.TextBoxWithAddOns(html, name, value, prependValue, appendValue, htmlAttributes);
        }

        #endregion TextBoxWithAddOns

        #region Panel

        public PanelBuilder<TModel> Begin(Panel panel)
        {
            if (panel == null)
            {
                throw new ArgumentNullException("panel");
            }

            panel.Provider = provider;
            return new PanelBuilder<TModel>(this.html, panel);
        }

        #endregion Panel

        #region Quotes

        public MvcHtmlString Quote(string text, string author, string titleOfWork, object htmlAttributes = null)
        {
            return provider.Quote(text, author, titleOfWork, htmlAttributes);
        }

        #endregion Quotes

        #region Tabs

        public TabsBuilder<TModel> Begin(Tabs tabs)
        {
            if (tabs == null)
            {
                throw new ArgumentNullException("tabs");
            }

            tabs.Provider = provider;
            return new TabsBuilder<TModel>(this.html, tabs);
        }

        #endregion Tabs

        #region Thumbnails

        public MvcHtmlString Thumbnail(string src, string alt, string href = null, object aHtmlAttributes = null, object imgHtmlAttributes = null)
        {
            return provider.ThumbnailProvider.Thumbnail(html, src, alt, href, aHtmlAttributes, imgHtmlAttributes);
        }

        public ThumbnailBuilder<TModel> Begin(Thumbnail thumbnail)
        {
            if (thumbnail == null)
            {
                throw new ArgumentNullException("thumbnail");
            }

            thumbnail.Provider = provider;
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

            toolbar.Provider = provider;
            return new ToolbarBuilder<TModel>(this.html, toolbar);
        }

        #endregion Toolbar
    }
}