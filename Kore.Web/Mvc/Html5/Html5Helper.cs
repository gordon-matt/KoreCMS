/*
 * Many thanks to Saleth Prakash: http://www.codeproject.com/Articles/237411/Html-Controls-for-ASP-Net-MVC
*/

namespace Kore.Web.Mvc.Html
{
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// A Html5 helper class which contains all the Html5 elements.
    /// </summary>
    public class Html5Helper : IHtml5Helper
    {
        private HtmlHelper htmlHelper;

        /// <summary>
        /// Initializes a Html5Helper class used to render Html5 elements.
        /// </summary>
        /// <param name="htmlHelper">The html helper class instance that comes from the current view context.</param>
        public Html5Helper(HtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
            Canvas = new Html5CanvasHelper(this.htmlHelper);
            Svg = new Html5SvgHelper(this.htmlHelper);
            Media = new Html5MediaHelper(this.htmlHelper);
        }

        /// <summary>
        /// A Html5 helper class instance which contains all the canvas related elements.
        /// </summary>
        public Html5CanvasHelper Canvas { get; private set; }

        /// <summary>
        /// A Html5 helper class instance which contains all the svg related elements.
        /// </summary>
        public Html5SvgHelper Svg { get; private set; }

        /// <summary>
        /// A Html5 helper class instance which contains all the media related elements.
        /// </summary>
        public Html5MediaHelper Media { get; private set; }

        /// <summary>
        /// Renders a input element having a placeholder text.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="placeholderText">The placeholder text to be set for the input element.</param>
        /// <returns>A input element having a placeholder text.</returns>
        public MvcHtmlString PlaceholderBox(string name, string placeholderText)
        {
            return PlaceholderBox(name, placeholderText, null);
        }

        /// <summary>
        /// Renders a input element having a placeholder text.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="placeholderText">The placeholder text to be set for the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element having a placeholder text.</returns>
        public MvcHtmlString PlaceholderBox(string name, string placeholderText, object htmlAttributes)
        {
            TagBuilder tagBuilder = BuildInputTag(name, "text", htmlAttributes);
            tagBuilder.MergeAttribute("placeholder", placeholderText);
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Renders a input element of email type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <returns>A input element of email type.</returns>
        public MvcHtmlString EmailBox(string name)
        {
            return EmailBox(name, null);
        }

        /// <summary>
        /// Renders a input element of email type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of email type.</returns>
        public MvcHtmlString EmailBox(string name, object htmlAttributes)
        {
            TagBuilder tagBuilder = BuildInputTag(name, "email", htmlAttributes);
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Renders a input element of url type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <returns>A input element of url type.</returns>
        public MvcHtmlString UrlBox(string name)
        {
            return UrlBox(name, null);
        }

        /// <summary>
        /// Renders a input element of url type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of url type.</returns>
        public MvcHtmlString UrlBox(string name, object htmlAttributes)
        {
            TagBuilder tagBuilder = BuildInputTag(name, "url", htmlAttributes);
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Renders a input element of number type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <returns>A input element of number type.</returns>
        public MvcHtmlString NumberBox(string name)
        {
            return NumberBox(name, null);
        }

        /// <summary>
        /// Renders a input element of number type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of number type.</returns>
        public MvcHtmlString NumberBox(string name, object htmlAttributes)
        {
            return NumberBox(name, -1, -1, 0, null);
        }

        /// <summary>
        /// Renders a input element of number type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="min">The minimum value of the input element.</param>
        /// <param name="max">The maximum value of the input element.</param>
        /// <param name="step">The stepping value of the input element.</param>
        /// <returns>A input element of number type.</returns>
        public MvcHtmlString NumberBox(string name, double min, double max, double step)
        {
            return NumberBox(name, min, max, step, null);
        }

        /// <summary>
        /// Renders a input element of number type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="min">The minimum value of the input element.</param>
        /// <param name="max">The maximum value of the input element.</param>
        /// <param name="step">The stepping value of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of number type.</returns>
        public MvcHtmlString NumberBox(string name, double min, double max, double step, object htmlAttributes)
        {
            TagBuilder tagBuilder = BuildInputTag(name, "number", htmlAttributes);
            if (min != -1)
                tagBuilder.MergeAttribute("min", min.ToString());
            if (max != -1)
                tagBuilder.MergeAttribute("max", max.ToString());
            if (step != 0)
                tagBuilder.MergeAttribute("step", step.ToString());
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Renders a input element of range type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <returns>A input element of range type.</returns>
        public MvcHtmlString Range(string name)
        {
            return Range(name, null);
        }

        /// <summary>
        /// Renders a input element of range type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of range type.</returns>
        public MvcHtmlString Range(string name, object htmlAttributes)
        {
            return Range(name, -1, -1, 0, null);
        }

        /// <summary>
        /// Renders a input element of range type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="min">The minimum value of the input element.</param>
        /// <param name="max">The maximum value of the input element.</param>
        /// <param name="step">The stepping value of the input element.</param>
        /// <returns>A input element of range type.</returns>
        public MvcHtmlString Range(string name, int min, int max, int step)
        {
            return Range(name, min, max, step, null);
        }

        /// <summary>
        /// Renders a input element of range type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="min">The minimum value of the input element.</param>
        /// <param name="max">The maximum value of the input element.</param>
        /// <param name="step">The stepping value of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of range type.</returns>
        public MvcHtmlString Range(string name, int min, int max, int step, object htmlAttributes)
        {
            TagBuilder tagBuilder = BuildInputTag(name, "range", htmlAttributes);
            if (min != -1)
                tagBuilder.MergeAttribute("min", min.ToString());
            if (max != -1)
                tagBuilder.MergeAttribute("max", max.ToString());
            if (step != 0)
                tagBuilder.MergeAttribute("step", step.ToString());
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Renders a input element of search type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <returns>A input element of search type.</returns>
        public MvcHtmlString SearchBox(string name)
        {
            return SearchBox(name, null);
        }

        /// <summary>
        /// Renders a input element of search type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of search type.</returns>
        public MvcHtmlString SearchBox(string name, object htmlAttributes)
        {
            TagBuilder tagBuilder = BuildInputTag(name, "search", htmlAttributes);
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Renders a input element of color type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <returns>A input element of color type.</returns>
        public MvcHtmlString ColorBox(string name)
        {
            return ColorBox(name, null);
        }

        /// <summary>
        /// Renders a input element of color type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of color type.</returns>
        public MvcHtmlString ColorBox(string name, object htmlAttributes)
        {
            TagBuilder tagBuilder = BuildInputTag(name, "color", htmlAttributes);
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Renders a input element of date type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <returns>A input element of date type.</returns>
        public MvcHtmlString DateBox(string name)
        {
            return DateBox(name, null);
        }

        /// <summary>
        /// Renders a input element of date type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of date type.</returns>
        public MvcHtmlString DateBox(string name, object htmlAttributes)
        {
            TagBuilder tagBuilder = BuildInputTag(name, "date", htmlAttributes);
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Renders a input element of month type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <returns>A input element of month type.</returns>
        public MvcHtmlString MonthBox(string name)
        {
            return MonthBox(name, null);
        }

        /// <summary>
        /// Renders a input element of month type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of month type.</returns>
        public MvcHtmlString MonthBox(string name, object htmlAttributes)
        {
            TagBuilder tagBuilder = BuildInputTag(name, "month", htmlAttributes);
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Renders a input element of week type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <returns>A input element of week type.</returns>
        public MvcHtmlString WeekBox(string name)
        {
            return WeekBox(name, null);
        }

        /// <summary>
        /// Renders a input element of week type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of week type.</returns>
        public MvcHtmlString WeekBox(string name, object htmlAttributes)
        {
            TagBuilder tagBuilder = BuildInputTag(name, "week", htmlAttributes);
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Renders a input element of time type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <returns>A input element of time type.</returns>
        public MvcHtmlString TimeBox(string name)
        {
            return TimeBox(name, null);
        }

        /// <summary>
        /// Renders a input element of time type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of time type.</returns>
        public MvcHtmlString TimeBox(string name, object htmlAttributes)
        {
            TagBuilder tagBuilder = BuildInputTag(name, "time", htmlAttributes);
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Renders a input element of datetime type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <returns>A input element of datetime type.</returns>
        public MvcHtmlString DateTimeBox(string name)
        {
            return DateTimeBox(name, null);
        }

        /// <summary>
        /// Renders a input element of datetime type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of datetime type.</returns>
        public MvcHtmlString DateTimeBox(string name, object htmlAttributes)
        {
            TagBuilder tagBuilder = BuildInputTag(name, "datetime", htmlAttributes);
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Renders a input element of datetime-local type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <returns>A input element of datetime-local type.</returns>
        public MvcHtmlString DateTimeLocalBox(string name)
        {
            return DateTimeLocalBox(name, null);
        }

        /// <summary>
        /// Renders a input element of datetime-local type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of datetime-local type.</returns>
        public MvcHtmlString DateTimeLocalBox(string name, object htmlAttributes)
        {
            TagBuilder tagBuilder = BuildInputTag(name, "datetime-local", htmlAttributes);
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Renders a progress element.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="innerText">The text to be displayed within the progress element.
        /// If the client browser does not support this element, then the text is displayed as a not supported message.</param>
        /// <returns>A progress element.</returns>
        public MvcHtmlString Progress(string name, string innerText)
        {
            return Progress(name, innerText, -1, -1, null);
        }

        /// <summary>
        /// Renders a progress element.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="innerText">The text to be displayed within the progress element.
        /// If the client browser does not support this element, then the text is displayed as a not supported message.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A progress element.</returns>
        public MvcHtmlString Progress(string name, string innerText, object htmlAttributes)
        {
            return Progress(name, innerText, -1, -1, htmlAttributes);
        }

        /// <summary>
        /// Renders a progress element.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="innerText">The text to be displayed within the progress element.
        /// If the client browser does not support this element, then the text is displayed as a not supported message.</param>
        /// <param name="max">The maximum value of the progress element.</param>
        /// <returns>A progress element.</returns>
        public MvcHtmlString Progress(string name, string innerText, int max)
        {
            return Progress(name, innerText, max, -1, null);
        }

        /// <summary>
        /// Renders a progress element.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="innerText">The text to be displayed within the progress element.
        /// If the client browser does not support this element, then the text is displayed as a not supported message.</param>
        /// <param name="max">The maximum value of the progress element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A progress element.</returns>
        public MvcHtmlString Progress(string name, string innerText, int max, object htmlAttributes)
        {
            return Progress(name, innerText, max, -1, htmlAttributes);
        }

        /// <summary>
        /// Renders a progress element.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="innerText">The text to be displayed within the progress element.
        /// If the client browser does not support this element, then the text is displayed as a not supported message.</param>
        /// <param name="max">The maximum value of the progress element.</param>
        /// <param name="value">The current value of the progress element.</param>
        /// <returns>A progress element.</returns>
        public MvcHtmlString Progress(string name, string innerText, int max, int value)
        {
            return Progress(name, innerText, max, value, null);
        }

        /// <summary>
        /// Renders a progress element.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="innerText">The text to be displayed within the progress element.
        /// If the client browser does not support this element, then the text is displayed as a not supported message.</param>
        /// <param name="max">The maximum value of the progress element.</param>
        /// <param name="value">The current value of the progress element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A progress element.</returns>
        public MvcHtmlString Progress(string name, string innerText, int max, int value, object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("progress");
            if (htmlAttributes != null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary(htmlAttributes);
                tagBuilder.MergeAttributes(routeValueDictionary);
            }
            if (value != -1)
                tagBuilder.MergeAttribute("value", value.ToString());
            if (max != -1)
                tagBuilder.MergeAttribute("max", max.ToString());
            tagBuilder.MergeAttribute("id", name);
            tagBuilder.InnerHtml = innerText;
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Renders a meter element.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="innerText">The text to be displayed within the meter element.
        /// If the client browser does not support this element, then the text is displayed as a not supported message.</param>
        /// <returns>A meter element.</returns>
        public MvcHtmlString Meter(string name, string innerText)
        {
            return Meter(name, innerText, -1, -1, -1, null);
        }

        /// <summary>
        /// Renders a meter element.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="innerText">The text to be displayed within the meter element.
        /// If the client browser does not support this element, then the text is displayed as a not supported message.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A meter element.</returns>
        public MvcHtmlString Meter(string name, string innerText, object htmlAttributes)
        {
            return Meter(name, innerText, -1, -1, -1, htmlAttributes);
        }

        /// <summary>
        /// Renders a meter element.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="innerText">The text to be displayed within the meter element.
        /// If the client browser does not support this element, then the text is displayed as a not supported message.</param>
        /// <param name="min">The minimum value of the meter element.</param>
        /// <param name="max">The maximum value of the meter element.</param>
        /// <param name="value">The current value of the meter element.</param>
        /// <returns>A meter element.</returns>
        public MvcHtmlString Meter(string name, string innerText, double min, double max, double value)
        {
            return Meter(name, innerText, max, min, value, null);
        }

        /// <summary>
        /// Renders a meter element.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="innerText">The text to be displayed within the meter element.
        /// If the client browser does not support this element, then the text is displayed as a not supported message.</param>
        /// <param name="min">The minimum value of the meter element.</param>
        /// <param name="max">The maximum value of the meter element.</param>
        /// <param name="value">The current value of the meter element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A meter element.</returns>
        public MvcHtmlString Meter(string name, string innerText, double min, double max, double value, object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("meter");
            if (htmlAttributes != null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary(htmlAttributes);
                tagBuilder.MergeAttributes(routeValueDictionary);
            }
            if (value != -1)
                tagBuilder.MergeAttribute("value", value.ToString());
            if (max != -1)
                tagBuilder.MergeAttribute("max", max.ToString());
            if (min != -1)
                tagBuilder.MergeAttribute("min", min.ToString());
            tagBuilder.MergeAttribute("id", name);
            tagBuilder.InnerHtml = innerText;
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Creates a input element with the specified type.
        /// </summary>
        private TagBuilder BuildInputTag(string name, string inputType, object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("input");
            if (htmlAttributes != null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary(htmlAttributes);
                tagBuilder.MergeAttributes(routeValueDictionary);
            }
            tagBuilder.MergeAttribute("type", inputType);
            tagBuilder.MergeAttribute("name", name);
            tagBuilder.MergeAttribute("id", name);
            return tagBuilder;
        }
    }
}