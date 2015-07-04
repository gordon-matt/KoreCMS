/*
 * Many thanks to Saleth Prakash: http://www.codeproject.com/Articles/237411/Html-Controls-for-ASP-Net-MVC
*/

namespace Kore.Web.Mvc.Html
{
    using System.Drawing;
    using System.Web.Mvc;

    /// <summary>
    /// A Html5 helper interface which contains all the canvas related elements.
    /// </summary>
    public interface IHtml5CanvasHelper
    {
        /// <summary>
        /// Renders a empty canvas element in the client browser.
        /// </summary>
        /// <param name="name">The name for the canvas, but this renders as ID for the canvas element.</param>
        /// <param name="notSupportedMessage">The message appears when the canvas tag is not supported by the client browser.</param>
        /// <returns>A canvas element of Html5 specifications.</returns>
        MvcHtmlString EmptyCanvas(string name, string notSupportedMessage);

        /// <summary>
        /// Renders a empty canvas element in the client browser.
        /// </summary>
        /// <param name="name">The name for the canvas, but this renders as ID for the canvas element.</param>
        /// <param name="notSupportedMessage">The message appears when the canvas tag is not supported by the client browser.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the canvas element.</param>
        /// <returns>A empty canvas element.</returns>
        MvcHtmlString EmptyCanvas(string name, string notSupportedMessage, object htmlAttributes);

        /// <summary>
        /// Renders a rectangle element in canvas in the client browser.
        /// </summary>
        /// <param name="name">The name for the canvas, but this renders as ID for the canvas element.</param>
        /// <param name="notSupportedMessage">The message appears when the canvas tag is not supported by the client browser.</param>
        /// <param name="x">The x position of the rectangle.</param>
        /// <param name="y">The y position of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <param name="thickness">The line thickness of the rectangle.</param>
        /// <param name="lineColor">The border color of the rectangle.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the canvas element.</param>
        /// <returns>A canvas element containing a rectangle.</returns>
        MvcHtmlString Rectangle(string name, string notSupportedMessage, int x, int y, int width, int height, int thickness, string lineColor, object htmlAttributes);

        /// <summary>
        /// Renders a filled rectangle element in canvas in the client browser.
        /// </summary>
        /// <param name="name">The name for the canvas, but this renders as ID for the canvas element.</param>
        /// <param name="notSupportedMessage">The message appears when the canvas tag is not supported by the client browser.</param>
        /// <param name="x">The x point of the rectangle.</param>
        /// <param name="y">The y point of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <param name="fillColor">The fill color of the rectangle.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the canvas element.</param>
        /// <returns>A canvas element containing a filled rectangle.</returns>
        MvcHtmlString FilledRectangle(string name, string notSupportedMessage, int x, int y, int width, int height, string fillColor, object htmlAttributes);

        /// <summary>
        /// Renders a ellipse element in canvas in the client browser.
        /// </summary>
        /// <param name="name">The name for the canvas, but this renders as ID for the canvas element.</param>
        /// <param name="notSupportedMessage">The message appears when the canvas tag is not supported by the client browser.</param>
        /// <param name="cx">The center x point of the ellipse.</param>
        /// <param name="cy">The center y point of the ellipse.</param>
        /// <param name="width">The width of the ellipse.</param>
        /// <param name="height">The height of the ellipse.</param>
        /// <param name="thickness">The line thickness of the ellipse.</param>
        /// <param name="lineColor">The border color of the ellipse.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the canvas element.</param>
        /// <returns>A canvas element containing a ellipse.</returns>
        MvcHtmlString Ellipse(string name, string notSupportedMessage, int cx, int cy, int width, int height, int thickness, string lineColor, object htmlAttributes);

        /// <summary>
        /// Renders a filled ellipse element in canvas in the client browser.
        /// </summary>
        /// <param name="name">The name for the canvas, but this renders as ID for the canvas element.</param>
        /// <param name="notSupportedMessage">The message appears when the canvas tag is not supported by the client browser.</param>
        /// <param name="cx">The center x point of the ellipse.</param>
        /// <param name="cy">The center y point of the ellipse.</param>
        /// <param name="width">The width of the ellipse.</param>
        /// <param name="height">The height of the ellipse.</param>
        /// <param name="fillColor">The fill color of the ellipse.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the canvas element.</param>
        /// <returns>A canvas element containing a filled ellipse.</returns>
        MvcHtmlString FilledEllipse(string name, string notSupportedMessage, int cx, int cy, int width, int height, string fillColor, object htmlAttributes);

        /// <summary>
        /// Renders a circle element in canvas in the client browser.
        /// </summary>
        /// <param name="name">The name for the canvas, but this renders as ID for the canvas element.</param>
        /// <param name="notSupportedMessage">The message appears when the canvas tag is not supported by the client browser.</param>
        /// <param name="x">The center x point of the circle.</param>
        /// <param name="y">The center y point of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="thickness">The line thickness of the circle.</param>
        /// <param name="lineColor">The border color of the circle.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the canvas element.</param>
        /// <returns>A canvas element containing a circle.</returns>
        MvcHtmlString Circle(string name, string notSupportedMessage, int x, int y, float radius, int thickness, string lineColor, object htmlAttributes);

        /// <summary>
        /// Renders a filled circle element in canvas in the client browser.
        /// </summary>
        /// <param name="name">The name for the canvas, but this renders as ID for the canvas element.</param>
        /// <param name="notSupportedMessage">The message appears when the canvas tag is not supported by the client browser.</param>
        /// <param name="x">The center x point of the circle.</param>
        /// <param name="y">The center y point of the circle.</param>
        /// <param name="radius">The radius of the circle.</param>
        /// <param name="fillColor">The fill color of the circle.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the canvas element.</param>
        /// <returns>A canvas element containing a filled circle.</returns>
        MvcHtmlString FilledCircle(string name, string notSupportedMessage, int x, int y, float radius, string fillColor, object htmlAttributes);

        /// <summary>
        /// Renders a arc element in canvas in the client browser.
        /// </summary>
        /// <param name="name">The name for the canvas, but this renders as ID for the canvas element.</param>
        /// <param name="notSupportedMessage">The message appears when the canvas tag is not supported by the client browser.</param>
        /// <param name="x">The center x point of the arc.</param>
        /// <param name="y">The center y point of the arc.</param>
        /// <param name="radius">The radius of the arc.</param>
        /// <param name="thickness">The line thickness of the arc.</param>
        /// <param name="startAngle">The starting angle of the arc.</param>
        /// <param name="endAngle">The ending angle of the arc.</param>
        /// <param name="lineColor">The border color of the arc.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the canvas element.</param>
        /// <returns>A canvas element containing a arc.</returns>
        MvcHtmlString Arc(string name, string notSupportedMessage, int x, int y, float radius, int thickness, float startAngle, float endAngle, string lineColor, object htmlAttributes);

        /// <summary>
        /// Renders a filled arc element in canvas in the client browser.
        /// </summary>
        /// <param name="name">The name for the canvas, but this renders as ID for the canvas element.</param>
        /// <param name="notSupportedMessage">The message appears when the canvas tag is not supported by the client browser.</param>
        /// <param name="x">The center x point of the arc.</param>
        /// <param name="y">The center y point of the arc.</param>
        /// <param name="radius">The radius of the arc.</param>
        /// <param name="startAngle">The starting angle of the arc.</param>
        /// <param name="endAngle">The ending angle of the arc.</param>
        /// <param name="fillColor">The fill color of the circle.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the canvas element.</param>
        /// <returns>A canvas element containing a filled arc.</returns>
        MvcHtmlString FilledArc(string name, string notSupportedMessage, int x, int y, float radius, float startAngle, float endAngle, string fillColor, object htmlAttributes);

        /// <summary>
        /// Renders a beizer curve element in canvas in the client browser.
        /// </summary>
        /// <param name="name">The name for the canvas, but this renders as ID for the canvas element.</param>
        /// <param name="notSupportedMessage">The message appears when the canvas tag is not supported by the client browser.</param>
        /// <param name="x1">The starting x point of the beizer curve.</param>
        /// <param name="y1">The starting y point of the beizer curve.</param>
        /// <param name="x2">The ending x point of the beizer curve.</param>
        /// <param name="y2">The ending y point of the beizer curve.</param>
        /// <param name="radius">The radius of the beizer curve.</param>
        /// <param name="thickness">The line thickness of the beizer curve.</param>
        /// <param name="lineColor">The border color of the beizer curve.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the canvas element.</param>
        /// <returns>A canvas element containing a beizer curve.</returns>
        MvcHtmlString BeizerCurve(string name, string notSupportedMessage, int x1, int y1, int x2, int y2, float radius, int thickness, string lineColor, object htmlAttributes);

        /// <summary>
        /// Renders a filled beizer curve element in canvas in the client browser.
        /// </summary>
        /// <param name="name">The name for the canvas, but this renders as ID for the canvas element.</param>
        /// <param name="notSupportedMessage">The message appears when the canvas tag is not supported by the client browser.</param>
        /// <param name="x1">The starting x point of the beizer curve.</param>
        /// <param name="y1">The starting y point of the beizer curve.</param>
        /// <param name="x2">The ending x point of the beizer curve.</param>
        /// <param name="y2">The ending y point of the beizer curve.</param>
        /// <param name="radius">The radius of the beizer curve.</param>
        /// <param name="fillColor">The fill color of the beizer curve.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the canvas element.</param>
        /// <returns>A canvas element containing a filled beizer curve.</returns>
        MvcHtmlString FilledBeizerCurve(string name, string notSupportedMessage, int x1, int y1, int x2, int y2, float radius, string fillColor, object htmlAttributes);

        /// <summary>
        /// Renders a quadratic curve element in canvas in the client browser.
        /// </summary>
        /// <param name="name">The name for the canvas, but this renders as ID for the canvas element.</param>
        /// <param name="notSupportedMessage">The message appears when the canvas tag is not supported by the client browser.</param>
        /// <param name="x1">The starting x point of the quadratic curve.</param>
        /// <param name="y1">The starting y point of the quadratic curve.</param>
        /// <param name="x2">The ending x point of the quadratic curve.</param>
        /// <param name="y2">The ending y point of the quadratic curve.</param>
        /// <param name="thickness">The line thickness of the quadratic curve.</param>
        /// <param name="lineColor">The border color of the quadratic curve.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the canvas element.</param>
        /// <returns>A canvas element containing a quadratic curve.</returns>
        MvcHtmlString QuadraticCurve(string name, string notSupportedMessage, int x1, int y1, int x2, int y2, int thickness, string lineColor, object htmlAttributes);

        /// <summary>
        /// Renders a filled quadratic curve element in canvas in the client browser.
        /// </summary>
        /// <param name="name">The name for the canvas, but this renders as ID for the canvas element.</param>
        /// <param name="notSupportedMessage">The message appears when the canvas tag is not supported by the client browser.</param>
        /// <param name="x1">The starting x point of the quadratic curve.</param>
        /// <param name="y1">The starting y point of the quadratic curve.</param>
        /// <param name="x2">The ending x point of the quadratic curve.</param>
        /// <param name="y2">The ending y point of the quadratic curve.</param>
        /// <param name="fillColor">The fill color of the quadratic curve.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the canvas element.</param>
        /// <returns>A canvas element containing a filled quadratic curve.</returns>
        MvcHtmlString FilledQuadraticCurve(string name, string notSupportedMessage, int x1, int y1, int x2, int y2, string fillColor, object htmlAttributes);

        /// <summary>
        /// Renders a polygon element in canvas in the client browser.
        /// </summary>
        /// <param name="name">The name for the canvas, but this renders as ID for the canvas element.</param>
        /// <param name="notSupportedMessage">The message appears when the canvas tag is not supported by the client browser.</param>
        /// <param name="points">The array of the points on which the polygon is drawn.</param>
        /// <param name="thickness">The line thickness of the polygon.</param>
        /// <param name="lineColor">The border color of the polygon.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the canvas element.</param>
        /// <returns>A canvas element containing a polygon.</returns>
        MvcHtmlString Polygon(string name, string notSupportedMessage, Point[] points, int thickness, string lineColor, object htmlAttributes);

        /// <summary>
        /// Renders a filled polygon element in canvas in the client browser.
        /// </summary>
        /// <param name="name">The name for the canvas, but this renders as ID for the canvas element.</param>
        /// <param name="notSupportedMessage">The message appears when the canvas tag is not supported by the client browser.</param>
        /// <param name="points">The array of the points on which the polygon is drawn.</param>
        /// <param name="fillColor">The fill color of the polygon.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the canvas element.</param>
        /// <returns>A canvas element containing a filled polygon.</returns>
        MvcHtmlString FilledPolygon(string name, string notSupportedMessage, Point[] points, string fillColor, object htmlAttributes);

        /// <summary>
        /// Renders a text element in canvas in the client browser.
        /// </summary>
        /// <param name="name">The name for the canvas, but this renders as ID for the canvas element.</param>
        /// <param name="notSupportedMessage">The message appears when the canvas tag is not supported by the client browser.</param>
        /// <param name="x">The x point of the text.</param>
        /// <param name="y">The y point of the text.</param>
        /// <param name="text">The text to be drawn.</param>
        /// <param name="fontFamily">The font family for the text. (e.g.) Arial, Tahoma, etc.</param>
        /// <param name="fontSize">The font size for the text in pixels.</param>
        /// <param name="fontStyle">The font style for the text. (styles: bold, italic).</param>
        /// <param name="thickness">The line thickness of the text.</param>
        /// <param name="lineColor">The border color of the text.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the canvas element.</param>
        /// <returns>A canvas element containing a text.</returns>
        MvcHtmlString Text(string name, string notSupportedMessage, int x, int y, string text, string fontFamily, int fontSize, string fontStyle, int thickness, string lineColor, object htmlAttributes);

        /// <summary>
        /// Renders a filled text element in canvas in the client browser.
        /// </summary>
        /// <param name="name">The name for the canvas, but this renders as ID for the canvas element.</param>
        /// <param name="notSupportedMessage">The message appears when the canvas tag is not supported by the client browser.</param>
        /// <param name="x">The x point of the text.</param>
        /// <param name="y">The y point of the text.</param>
        /// <param name="text">The text to be drawn.</param>
        /// <param name="fontFamily">The font family for the text. (e.g.) Arial, Tahoma, etc.</param>
        /// <param name="fontSize">The font size for the text in pixels.</param>
        /// <param name="fontStyle">The font style for the text. (styles: bold, italic).</param>
        /// <param name="fillColor">The fill color of the text.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the canvas element.</param>
        /// <returns>A canvas element containing a filled text.</returns>
        MvcHtmlString FilledText(string name, string notSupportedMessage, int x, int y, string text, string fontFamily, int fontSize, string fontStyle, string fillColor, object htmlAttributes);

        /// <summary>
        /// Renders a line element in canvas in the client browser.
        /// </summary>
        /// <param name="name">The name for the canvas, but this renders as ID for the canvas element.</param>
        /// <param name="notSupportedMessage">The message appears when the canvas tag is not supported by the client browser.</param>
        /// <param name="x1">The starting x point of the line.</param>
        /// <param name="y1">The starting y point of the line.</param>
        /// <param name="x2">The ending x point of the line.</param>
        /// <param name="y2">The ending y point of the line.</param>
        /// <param name="thickness">The thickness of the line.</param>
        /// <param name="lineColor">The line color of the line.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the canvas element.</param>
        /// <returns>A canvas element containing a line.</returns>
        MvcHtmlString Line(string name, string notSupportedMessage, int x1, int y1, int x2, int y2, int thickness, string lineColor, object htmlAttributes);

        /// <summary>
        /// Renders a image element in canvas in the client browser.
        /// </summary>
        /// <param name="name">The name for the canvas, but this renders as ID for the canvas element.</param>
        /// <param name="notSupportedMessage">The message appears when the canvas tag is not supported by the client browser.</param>
        /// <param name="x">The x point of the image.</param>
        /// <param name="y">The y point of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <param name="imageUrl">The url of the image.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the canvas element.</param>
        /// <returns>A canvas element containing a image.</returns>
        MvcHtmlString Image(string name, string notSupportedMessage, int x, int y, int width, int height, string imageUrl, object htmlAttributes);
    }
}