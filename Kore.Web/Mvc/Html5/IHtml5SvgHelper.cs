/*
 * Many thanks to Saleth Prakash: http://www.codeproject.com/Articles/237411/Html-Controls-for-ASP-Net-MVC
*/

namespace Kore.Web.Mvc.Html
{
    using System.Drawing;
    using System.Web.Mvc;

    /// <summary>
    /// A Html5 helper interface which contains all the svg related elements.
    /// </summary>
    public interface IHtml5SvgHelper
    {
        /// <summary>
        /// Renders a empty svg element in the client browser.
        /// </summary>
        /// <param name="name">The name for the svg, but this renders as ID for the svg element.</param>
        /// <param name="notSupportedMessage">The message appears when the svg tag is not supported by the client browser.</param>
        /// <returns>A svg element.</returns>
        MvcHtmlString EmptySvg(string name, string notSupportedMessage);

        /// <summary>
        /// Renders a empty svg element in the client browser.
        /// </summary>
        /// <param name="name">The name for the svg, but this renders as ID for the svg element.</param>
        /// <param name="notSupportedMessage">The message appears when the svg tag is not supported by the client browser.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the svg element.</param>
        /// <returns>A svg element.</returns>
        MvcHtmlString EmptySvg(string name, string notSupportedMessage, object htmlAttributes);

        /// <summary>
        /// Renders a svg element containing a rectangle in the client browser.
        /// </summary>
        /// <param name="name">The name for the svg, but this renders as ID for the svg element.</param>
        /// <param name="notSupportedMessage">The message appears when the svg tag is not supported by the client browser.</param>
        /// <param name="x">The x point of the rectangle.</param>
        /// <param name="y">The y point of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <param name="thickness">The line thickness of the rectangle.</param>
        /// <param name="lineColor">The border color of the rectangle.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the svg element.</param>
        /// <returns>A svg element containing a rectangle.</returns>
        MvcHtmlString Rectangle(string name, string notSupportedMessage, int x, int y, int width, int height, int thickness, string lineColor, object htmlAttributes);

        /// <summary>
        /// Renders a svg element containing a filled rectangle in the client browser.
        /// </summary>
        /// <param name="name">The name for the svg, but this renders as ID for the svg element.</param>
        /// <param name="notSupportedMessage">The message appears when the svg tag is not supported by the client browser.</param>
        /// <param name="x">The x point of the rectangle.</param>
        /// <param name="y">The y point of the rectangle.</param>
        /// <param name="width">The width point of the rectangle.</param>
        /// <param name="height">The height point of the rectangle.</param>
        /// <param name="fillColor">The fill color of the rectangle.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the svg element.</param>
        /// <returns>A svg element containing a filled rectangle.</returns>
        MvcHtmlString FilledRectangle(string name, string notSupportedMessage, int x, int y, int width, int height, string fillColor, object htmlAttributes);

        /// <summary>
        /// Renders a svg element containing a ellipse in the client browser.
        /// </summary>
        /// <param name="name">The name for the svg, but this renders as ID for the svg element.</param>
        /// <param name="notSupportedMessage">The message appears when the svg tag is not supported by the client browser.</param>
        /// <param name="cx">The center x point of the ellipse.</param>
        /// <param name="cy">The center y point of the ellipse.</param>
        /// <param name="width">The width of the ellipse.</param>
        /// <param name="height">The height of the ellipse.</param>
        /// <param name="thickness">The line thickness of the ellipse.</param>
        /// <param name="lineColor">The border color of the ellipse.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the svg element.</param>
        /// <returns>A svg element containing a ellipse.</returns>
        MvcHtmlString Ellipse(string name, string notSupportedMessage, int cx, int cy, int width, int height, int thickness, string lineColor, object htmlAttributes);

        /// <summary>
        /// Renders a svg element containing a filled ellipse in the client browser.
        /// </summary>
        /// <param name="name">The name for the svg, but this renders as ID for the svg element.</param>
        /// <param name="notSupportedMessage">The message appears when the svg tag is not supported by the client browser.</param>
        /// <param name="cx">The center x point of the ellipse.</param>
        /// <param name="cy">The center y point of the ellipse.</param>
        /// <param name="width">The width of the ellipse.</param>
        /// <param name="height">The height of the ellipse.</param>
        /// <param name="fillColor">The fill color of the ellipse.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the svg element.</param>
        /// <returns>A svg element containing a filled ellipse.</returns>
        MvcHtmlString FilledEllipse(string name, string notSupportedMessage, int cx, int cy, int width, int height, string fillColor, object htmlAttributes);

        /// <summary>
        /// Renders a svg element containing a circle in the client browser.
        /// </summary>
        /// <param name="name">The name for the svg, but this renders as ID for the svg element.</param>
        /// <param name="notSupportedMessage">The message appears when the svg tag is not supported by the client browser.</param>
        /// <param name="cx">The center x point of the circle.</param>
        /// <param name="cy">The center y point of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="thickness">The line thickness of the circle.</param>
        /// <param name="lineColor">The border color of the circle.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the svg element.</param>
        /// <returns>A svg element containing a circle.</returns>
        MvcHtmlString Circle(string name, string notSupportedMessage, int cx, int cy, float radius, int thickness, string lineColor, object htmlAttributes);

        /// <summary>
        /// Renders a svg element containing a filled circle in the client browser.
        /// </summary>
        /// <param name="name">The name for the svg, but this renders as ID for the svg element.</param>
        /// <param name="notSupportedMessage">The message appears when the svg tag is not supported by the client browser.</param>
        /// <param name="cx">The center x point of the circle.</param>
        /// <param name="cy">The center y point of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="fillColor">The filled color of the circle.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the svg element.</param>
        /// <returns>A svg element containing a filled circle.</returns>
        MvcHtmlString FilledCircle(string name, string notSupportedMessage, int cx, int cy, float radius, string fillColor, object htmlAttributes);

        /// <summary>
        /// Renders a svg element containing a polygon in the client browser.
        /// </summary>
        /// <param name="name">The name for the svg, but this renders as ID for the svg element.</param>
        /// <param name="notSupportedMessage">The message appears when the svg tag is not supported by the client browser.</param>
        /// <param name="points">The points on which the polygon is drawn.</param>
        /// <param name="thickness">The line thickness of the polygon.</param>
        /// <param name="lineColor">The border color of the polygon.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the svg element.</param>
        /// <returns>A svg element containing a polygon.</returns>
        MvcHtmlString Polygon(string name, string notSupportedMessage, Point[] points, int thickness, string lineColor, object htmlAttributes);

        /// <summary>
        /// Renders a svg element containing a filled polygon in the client browser.
        /// </summary>
        /// <param name="name">The name for the svg, but this renders as ID for the svg element.</param>
        /// <param name="notSupportedMessage">The message appears when the svg tag is not supported by the client browser.</param>
        /// <param name="points">The points on which the polygon is drawn.</param>
        /// <param name="fillColor">The fill color of the polygon.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the svg element.</param>
        /// <returns>A svg element containing a filled polygon.</returns>
        MvcHtmlString FilledPolygon(string name, string notSupportedMessage, Point[] points, string fillColor, object htmlAttributes);

        /// <summary>
        /// Renders a svg element containing a text in the client browser.
        /// </summary>
        /// <param name="name">The name for the svg, but this renders as ID for the svg element.</param>
        /// <param name="notSupportedMessage">The message appears when the svg tag is not supported by the client browser.</param>
        /// <param name="x">The x point of the text.</param>
        /// <param name="y">The y point of the text.</param>
        /// <param name="text">The text to be drawn.</param>
        /// <param name="fontFamily">The font family for the text. (e.g.) Arial, Tahoma, etc.</param>
        /// <param name="fontSize">The font size for the text in pixels.</param>
        /// <param name="fontWeight">The font weight for the text. (weights: bold, etc.).</param>
        /// <param name="fontStyle">The font style for the text. (styles: italic, etc.).</param>
        /// <param name="textDecoration">The text decoration for the text. (styles: underline, etc.).</param>
        /// <param name="thickness">The line thickness of the text.</param>
        /// <param name="lineColor">The border color of the text.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the svg element.</param>
        /// <returns>A svg element containing a text.</returns>
        MvcHtmlString Text(string name, string notSupportedMessage, int x, int y, string text, string fontFamily, int fontSize, string fontWeight, string fontStyle, string textDecoration, int thickness, string lineColor, object htmlAttributes);

        /// <summary>
        /// Renders a svg element containing a filled text in the client browser.
        /// </summary>
        /// <param name="name">The name for the svg, but this renders as ID for the svg element.</param>
        /// <param name="notSupportedMessage">The message appears when the svg tag is not supported by the client browser.</param>
        /// <param name="x">The x point of the text.</param>
        /// <param name="y">The y point of the text.</param>
        /// <param name="text">The text to be drawn.</param>
        /// <param name="fontFamily">The font family for the text. (e.g.) Arial, Tahoma, etc.</param>
        /// <param name="fontSize">The font size for the text in pixels.</param>
        /// <param name="fontWeight">The font weight for the text. (weights: bold, etc.).</param>
        /// <param name="fontStyle">The font style for the text. (styles: italic, etc.).</param>
        /// <param name="textDecoration">The text decoration for the text. (styles: underline, etc.).</param>
        /// <param name="fillColor">The fill color of the text.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the svg element.</param>
        /// <returns>A svg element containing a filled text.</returns>
        MvcHtmlString FilledText(string name, string notSupportedMessage, int x, int y, string text, string fontFamily, int fontSize, string fontWeight, string fontStyle, string textDecoration, string fillColor, object htmlAttributes);

        /// <summary>
        /// Renders a svg element containing a line in the client browser.
        /// </summary>
        /// <param name="name">The name for the svg, but this renders as ID for the svg element.</param>
        /// <param name="notSupportedMessage">The message appears when the svg tag is not supported by the client browser.</param>
        /// <param name="x1">The starting x point of the line.</param>
        /// <param name="y1">The starting y point of the line.</param>
        /// <param name="x2">The ending x point of the line.</param>
        /// <param name="y2">The ending y point of the line.</param>
        /// <param name="thickness">The thickness of the line.</param>
        /// <param name="lineColor">The line color of the line.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the svg element.</param>
        /// <returns>A svg element containing a line.</returns>
        MvcHtmlString Line(string name, string notSupportedMessage, int x1, int y1, int x2, int y2, int thickness, string lineColor, object htmlAttributes);

        /// <summary>
        /// Renders a svg element containing a polyline in the client browser.
        /// </summary>
        /// <param name="name">The name for the svg, but this renders as ID for the svg element.</param>
        /// <param name="notSupportedMessage">The message appears when the svg tag is not supported by the client browser.</param>
        /// <param name="points">The points on which the polyline is drawn.</param>
        /// <param name="thickness">The line thickness of the polyline.</param>
        /// <param name="lineColor">The line color of the polyline.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the svg element.</param>
        /// <returns>A svg element containing a polyline.</returns>
        MvcHtmlString PolyLine(string name, string notSupportedMessage, Point[] points, int thickness, string lineColor, object htmlAttributes);

        /// <summary>
        /// Renders a svg element containing a image in the client browser.
        /// </summary>
        /// <param name="name">The name for the svg, but this renders as ID for the svg element.</param>
        /// <param name="notSupportedMessage">The message appears when the svg tag is not supported by the client browser.</param>
        /// <param name="x">The x point of the image.</param>
        /// <param name="y">The y point of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="imageUrl">The url of the image.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the svg element.</param>
        /// <returns>A svg element containing a image.</returns>
        MvcHtmlString Image(string name, string notSupportedMessage, int x, int y, int width, int height, string imageUrl, object htmlAttributes);
    }
}