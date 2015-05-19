﻿using System.Collections.Generic;
using System.Web.Mvc;

namespace Kore.Web.Mvc.KoreUI.Providers
{
    public abstract class BaseUIProvider : IKoreUIProvider
    {
        private ICollection<string> scripts;

        public ICollection<string> Scripts
        {
            get { return scripts ?? (scripts = new List<string>()); }
            set { scripts = value; }
        }

        #region IKoreUIProvider Members

        public virtual MvcHtmlString RenderScripts()
        {
            return new MvcHtmlString(string.Join(System.Environment.NewLine, Scripts));
        }

        #region General

        public abstract MvcHtmlString ActionLink(HtmlHelper html, string text, State state, string actionName, string controllerName, object routeValues = null, object htmlAttributes = null);

        public abstract MvcHtmlString Badge(string text, object htmlAttributes = null);

        public abstract MvcHtmlString Button(string text, State state, string onClick = null, object htmlAttributes = null);

        public abstract MvcHtmlString InlineLabel(string text, State state, object htmlAttributes = null);

        public abstract MvcHtmlString Quote(string text, string author, string titleOfWork, object htmlAttributes = null);

        public abstract MvcHtmlString SubmitButton(string text, State state, object htmlAttributes = null);

        public abstract MvcHtmlString TextBoxWithAddOns(HtmlHelper html, string name, object value, string prependValue, string appendValue, object htmlAttributes = null);

        #endregion General

        #region Special

        public abstract IAccordionProvider AccordionProvider { get; }

        public abstract IModalProvider ModalProvider { get; }

        public abstract ITabsProvider TabsProvider { get; }

        public abstract IThumbnailProvider ThumbnailProvider { get; }

        public abstract IToolbarProvider ToolbarProvider { get; }

        #endregion Special

        #endregion IKoreUIProvider Members

        protected abstract string GetButtonCssClass(State state);

        protected abstract string GetLabelCssClass(State state);
    }
}