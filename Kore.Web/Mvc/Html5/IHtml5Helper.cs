/*
 * Many thanks to Saleth Prakash: http://www.codeproject.com/Articles/237411/Html-Controls-for-ASP-Net-MVC
*/

using System.Web.Mvc;

namespace Kore.Web.Mvc.Html
{
    /// <summary>
    /// A Html5 helper interface which contains all the Html5 elements.
    /// </summary>
    public interface IHtml5Helper
    {
        /// <summary>
        /// Renders a input element having a placeholder text
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="placeholderText">The placeholder text to be set for the input element.</param>
        /// <returns>A input element having a placeholder text.</returns>
        MvcHtmlString PlaceholderBox(string name, string placeholderText);

        /// <summary>
        /// Renders a input element having a placeholder text
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="placeholderText">The placeholder text to be set for the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element having a placeholder text.</returns>
        MvcHtmlString PlaceholderBox(string name, string placeholderText, object htmlAttributes);

        /// <summary>
        /// Renders a input element of email type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <returns>A input element of email type.</returns>
        MvcHtmlString EmailBox(string name);

        /// <summary>
        /// Renders a input element of email type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of email type.</returns>
        MvcHtmlString EmailBox(string name, object htmlAttributes);

        /// <summary>
        /// Renders a input element of url type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <returns>A input element of url type.</returns>
        MvcHtmlString UrlBox(string name);

        /// <summary>
        /// Renders a input element of url type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of url type.</returns>
        MvcHtmlString UrlBox(string name, object htmlAttributes);

        /// <summary>
        /// Renders a input element of number type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <returns>A input element of number type.</returns>
        MvcHtmlString NumberBox(string name);

        /// <summary>
        /// Renders a input element of number type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of number type.</returns>
        MvcHtmlString NumberBox(string name, object htmlAttributes);

        /// <summary>
        /// Renders a input element of number type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="min">The minimum value of the input element.</param>
        /// <param name="max">The maximum value of the input element.</param>
        /// <param name="step">The stepping value of the input element.</param>
        /// <returns>A input element of number type.</returns>
        MvcHtmlString NumberBox(string name, double min, double max, double step);

        /// <summary>
        /// Renders a input element of number type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="min">The minimum value of the input element.</param>
        /// <param name="max">The maximum value of the input element.</param>
        /// <param name="step">The stepping value of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of number type.</returns>
        MvcHtmlString NumberBox(string name, double min, double max, double step, object htmlAttributes);

        /// <summary>
        /// Renders a input element of range type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <returns>A input element of range type.</returns>
        MvcHtmlString Range(string name);

        /// <summary>
        /// Renders a input element of range type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of range type.</returns>
        MvcHtmlString Range(string name, object htmlAttributes);

        /// <summary>
        /// Renders a input element of range type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="min">The minimum value of the input element.</param>
        /// <param name="max">The maximum value of the input element.</param>
        /// <param name="step">The stepping value of the input element.</param>
        /// <returns>A input element of range type.</returns>
        MvcHtmlString Range(string name, int min, int max, int step);

        /// <summary>
        /// Renders a input element of range type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="min">The minimum value of the input element.</param>
        /// <param name="max">The maximum value of the input element.</param>
        /// <param name="step">The stepping value of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of range type.</returns>
        MvcHtmlString Range(string name, int min, int max, int step, object htmlAttributes);

        /// <summary>
        /// Renders a input element of search type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <returns>A input element of search type.</returns>
        MvcHtmlString SearchBox(string name);

        /// <summary>
        /// Renders a input element of search type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of search type.</returns>
        MvcHtmlString SearchBox(string name, object htmlAttributes);

        /// <summary>
        /// Renders a input element of color type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <returns>A input element of color type.</returns>
        MvcHtmlString ColorBox(string name);

        /// <summary>
        /// Renders a input element of color type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of color type.</returns>
        MvcHtmlString ColorBox(string name, object htmlAttributes);

        /// <summary>
        /// Renders a input element of date type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <returns>A input element of date type.</returns>
        MvcHtmlString DateBox(string name);

        /// <summary>
        /// Renders a input element of date type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of date type.</returns>
        MvcHtmlString DateBox(string name, object htmlAttributes);

        /// <summary>
        /// Renders a input element of month type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <returns>A input element of month type.</returns>
        MvcHtmlString MonthBox(string name);

        /// <summary>
        /// Renders a input element of month type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of month type.</returns>
        MvcHtmlString MonthBox(string name, object htmlAttributes);

        /// <summary>
        /// Renders a input element of week type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <returns>A input element of week type.</returns>
        MvcHtmlString WeekBox(string name);

        /// <summary>
        /// Renders a input element of week type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of week type.</returns>
        MvcHtmlString WeekBox(string name, object htmlAttributes);

        /// <summary>
        /// Renders a input element of time type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <returns>A input element of time type.</returns>
        MvcHtmlString TimeBox(string name);

        /// <summary>
        /// Renders a input element of time type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of time type.</returns>
        MvcHtmlString TimeBox(string name, object htmlAttributes);

        /// <summary>
        /// Renders a input element of datetime type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <returns>A input element of datetime type.</returns>
        MvcHtmlString DateTimeBox(string name);

        /// <summary>
        /// Renders a input element of datetime type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of datetime type.</returns>
        MvcHtmlString DateTimeBox(string name, object htmlAttributes);

        /// <summary>
        /// Renders a input element of datetime-local type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <returns>A input element of datetime-local type.</returns>
        MvcHtmlString DateTimeLocalBox(string name);

        /// <summary>
        /// Renders a input element of datetime-local type.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A input element of datetime-local type.</returns>
        MvcHtmlString DateTimeLocalBox(string name, object htmlAttributes);

        /// <summary>
        /// Renders a progress element.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="innerText">The text to be displayed within the progress element.
        /// If the client browser does not support this element, then the text is displayed as a not supported message.</param>
        /// <returns>A progress element.</returns>
        MvcHtmlString Progress(string name, string innerText);

        /// <summary>
        /// Renders a progress element.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="innerText">The text to be displayed within the progress element.
        /// If the client browser does not support this element, then the text is displayed as a not supported message.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A progress element.</returns>
        MvcHtmlString Progress(string name, string innerText, object htmlAttributes);

        /// <summary>
        /// Renders a progress element.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="innerText">The text to be displayed within the progress element.
        /// If the client browser does not support this element, then the text is displayed as a not supported message.</param>
        /// <param name="max">The maximum value of the progress element.</param>
        /// <returns>A progress element.</returns>
        MvcHtmlString Progress(string name, string innerText, int max);

        /// <summary>
        /// Renders a progress element.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="innerText">The text to be displayed within the progress element.
        /// If the client browser does not support this element, then the text is displayed as a not supported message.</param>
        /// <param name="max">The maximum value of the progress element.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A progress element.</returns>
        MvcHtmlString Progress(string name, string innerText, int max, object htmlAttributes);

        /// <summary>
        /// Renders a progress element.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="innerText">The text to be displayed within the progress element.
        /// If the client browser does not support this element, then the text is displayed as a not supported message.</param>
        /// <param name="max">The maximum value of the progress element.</param>
        /// <param name="value">The current value of the progress element.</param>
        /// <returns>A progress element.</returns>
        MvcHtmlString Progress(string name, string innerText, int max, int value);

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
        MvcHtmlString Progress(string name, string innerText, int max, int value, object htmlAttributes);

        /// <summary>
        /// Renders a meter element.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="innerText">The text to be displayed within the meter element.
        /// If the client browser does not support this element, then the text is displayed as a not supported message.</param>
        /// <returns>A meter element.</returns>
        MvcHtmlString Meter(string name, string innerText);

        /// <summary>
        /// Renders a meter element.
        /// </summary>
        /// <param name="name">The name of the input element.</param>
        /// <param name="innerText">The text to be displayed within the meter element.
        /// If the client browser does not support this element, then the text is displayed as a not supported message.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the input element.</param>
        /// <returns>A meter element.</returns>
        MvcHtmlString Meter(string name, string innerText, object htmlAttributes);

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
        MvcHtmlString Meter(string name, string innerText, double min, double max, double value);

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
        MvcHtmlString Meter(string name, string innerText, double min, double max, double value, object htmlAttributes);
    }
}