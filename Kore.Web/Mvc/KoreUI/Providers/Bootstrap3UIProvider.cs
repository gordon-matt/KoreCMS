using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Kore.Web.Mvc.KoreUI.Providers
{
    public class Bootstrap3UIProvider : BaseUIProvider
    {
        private IAccordionProvider accordionProvider;
        private IModalProvider modalProvider;
        private ITabsProvider tabsProvider;
        private IThumbnailProvider thumbnailProvider;
        private IToolbarProvider toolbarProvider;

        #region IKoreUIProvider Members

        #region General

        public override MvcHtmlString ActionLink(HtmlHelper html, string text, State state, string actionName, string controllerName, object routeValues = null, object htmlAttributes = null)
        {
            var urlHelper = new UrlHelper(html.ViewContext.RequestContext);
            string stateCss = GetButtonCssClass(state);

            var builder = new FluentTagBuilder("a")
                .MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes))
                .MergeAttribute("href", urlHelper.Action(actionName, controllerName, routeValues))
                .AddCssClass(stateCss)
                .SetInnerText(text);

            return MvcHtmlString.Create(builder.ToString());
        }

        public override MvcHtmlString Badge(string text, object htmlAttributes = null)
        {
            var builder = new FluentTagBuilder("span")
                .MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes))
                .AddCssClass("badge")
                .SetInnerHtml(text);

            return MvcHtmlString.Create(builder.ToString());
        }

        public override MvcHtmlString Button(string text, State state, string onClick = null, object htmlAttributes = null)
        {
            string stateCss = GetButtonCssClass(state);

            var builder = new FluentTagBuilder("button")
                .MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes))
                .MergeAttribute("type", "button")
                .AddCssClass(stateCss)
                .SetInnerText(text);

            if (!string.IsNullOrEmpty(onClick))
            {
                builder.MergeAttribute("onclick", onClick);
            }

            return MvcHtmlString.Create(builder.ToString());
        }

        public override MvcHtmlString InlineLabel(string text, State state, object htmlAttributes = null)
        {
            string stateCss = GetButtonCssClass(state);

            var builder = new FluentTagBuilder("span")
                .MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes))
                .AddCssClass(stateCss)
                .SetInnerText(text);

            return MvcHtmlString.Create(builder.ToString());
        }

        public override MvcHtmlString Quote(string text, string author, string titleOfWork, object htmlAttributes = null)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException("text");
            }

            var builder = new FluentTagBuilder("blockquote")
                .MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes))
                .StartTag("p")
                    .SetInnerHtml(text)
                .EndTag()
                .StartTag("footer")
                    .AppendContent(author + ", ")
                    .StartTag("cite")
                        .MergeAttribute("title", titleOfWork)
                        .SetInnerText(titleOfWork)
                    .EndTag()
                .EndTag();

            return MvcHtmlString.Create(builder.ToString());
        }

        public override MvcHtmlString SubmitButton(string text, State state, object htmlAttributes = null)
        {
            string stateCss = GetButtonCssClass(state);

            var builder = new FluentTagBuilder("button")
                .MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes))
                .MergeAttribute("type", "submit")
                .AddCssClass(stateCss)
                .SetInnerText(text);

            return MvcHtmlString.Create(builder.ToString());
        }

        public override MvcHtmlString TextBoxWithAddOns(HtmlHelper html, string name, object value, string prependValue, string appendValue, object htmlAttributes = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            var builder = new FluentTagBuilder("div")
                .AddCssClass("input-group");

            if (!string.IsNullOrEmpty(prependValue))
            {
                builder = builder
                    .StartTag("div")
                        .AddCssClass("input-group-addon")
                        .SetInnerHtml(prependValue)
                    .EndTag();
            }

            builder = builder.AppendContent(html.TextBox(name, value, htmlAttributes).ToString());

            if (!string.IsNullOrEmpty(appendValue))
            {
                builder = builder
                .StartTag("div")
                    .AddCssClass("input-group-addon")
                    .SetInnerHtml(appendValue)
                .EndTag();
            }

            return MvcHtmlString.Create(builder.ToString());
        }

        #endregion General

        #region Special

        public override IAccordionProvider AccordionProvider
        {
            get { return accordionProvider ?? (accordionProvider = new Bootstrap3AccordionProvider()); }
        }

        public override IModalProvider ModalProvider
        {
            get { return modalProvider ?? (modalProvider = new Bootstrap3ModalProvider()); }
        }

        public override ITabsProvider TabsProvider
        {
            get { return tabsProvider ?? (tabsProvider = new Bootstrap3TabsProvider()); }
        }

        public override IThumbnailProvider ThumbnailProvider
        {
            get { return thumbnailProvider ?? (thumbnailProvider = new Bootstrap3ThumbnailProvider()); }
        }

        public override IToolbarProvider ToolbarProvider
        {
            get { return toolbarProvider ?? (toolbarProvider = new Bootstrap3ToolbarProvider()); }
        }

        #endregion Special

        #endregion IKoreUIProvider Members

        protected override string GetButtonCssClass(State state)
        {
            switch (state)
            {
                case State.Important: return "btn btn-danger";
                case State.Default: return "btn btn-default";
                case State.Info: return "btn btn-info";
                case State.Inverse: return "btn btn-inverse";
                case State.Primary: return "btn btn-primary";
                case State.Success: return "btn btn-success";
                case State.Warning: return "btn btn-warning";
                default: return "btn btn-default";
            }
        }

        protected override string GetLabelCssClass(State state)
        {
            switch (state)
            {
                case State.Important: return "label label-danger";
                case State.Default: return "label label-default";
                case State.Info: return "label label-info";
                case State.Inverse: return "label label-inverse";
                case State.Primary: return "label label-primary";
                case State.Success: return "label label-success";
                case State.Warning: return "label label-warning";
                default: return "label label-default";
            }
        }
    }
}