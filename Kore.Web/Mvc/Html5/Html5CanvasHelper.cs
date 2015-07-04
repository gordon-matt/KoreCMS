/*
 * Many thanks to Saleth Prakash: http://www.codeproject.com/Articles/237411/Html-Controls-for-ASP-Net-MVC
*/

namespace Kore.Web.Mvc.Html
{
    using System.Drawing;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// A Html5 helper class which contains all the canvas related elements.
    /// </summary>
    public class Html5CanvasHelper : IHtml5CanvasHelper
    {
        private HtmlHelper htmlHelper;

        /// <summary>
        /// Initializes the Html5 helper class of canvas related elements.
        /// </summary>
        /// <param name="htmlHelper">The html helper class instance that comes from the current view context.</param>
        public Html5CanvasHelper(HtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        /// <summary>
        /// Renders a empty canvas element in the client browser.
        /// </summary>
        /// <param name="name">The name for the canvas, but this renders as ID for the canvas element.</param>
        /// <param name="notSupportedMessage">The message appears when the canvas tag is not supported by the client browser.</param>
        /// <returns>A canvas element of Html5 specifications.</returns>
        public MvcHtmlString EmptyCanvas(string name, string notSupportedMessage)
        {
            return EmptyCanvas(name, notSupportedMessage, null);
        }

        /// <summary>
        /// Renders a empty canvas element in the client browser.
        /// </summary>
        /// <param name="name">The name for the canvas, but this renders as ID for the canvas element.</param>
        /// <param name="notSupportedMessage">The message appears when the canvas tag is not supported by the client browser.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the canvas element.</param>
        /// <returns>A empty canvas element.</returns>
        public MvcHtmlString EmptyCanvas(string name, string notSupportedMessage, object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("canvas");
            if (htmlAttributes != null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary(htmlAttributes);
                tagBuilder.MergeAttributes(routeValueDictionary);
            }
            tagBuilder.MergeAttribute("id", name);
            tagBuilder.InnerHtml = notSupportedMessage;
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }

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
        public MvcHtmlString Rectangle(string name, string notSupportedMessage, int x, int y, int width, int height, int thickness, string lineColor, object htmlAttributes)
        {
            return CreateCanvasRect(name, notSupportedMessage, x, y, width, height, thickness, lineColor, htmlAttributes, DrawMode.Stroke);
        }

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
        public MvcHtmlString FilledRectangle(string name, string notSupportedMessage, int x, int y, int width, int height, string fillColor, object htmlAttributes)
        {
            return CreateCanvasRect(name, notSupportedMessage, x, y, width, height, 0, fillColor, htmlAttributes, DrawMode.Fill);
        }

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
        public MvcHtmlString Ellipse(string name, string notSupportedMessage, int cx, int cy, int width, int height, int thickness, string lineColor, object htmlAttributes)
        {
            if (width == height)
                return CreateCanvasCircle(name, notSupportedMessage, cx, cy, width / 2, thickness, lineColor, htmlAttributes, DrawMode.Stroke);
            return CreateCanvasEllipse(name, notSupportedMessage, cx, cy, width, height, thickness, lineColor, htmlAttributes, DrawMode.Stroke);
        }

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
        public MvcHtmlString FilledEllipse(string name, string notSupportedMessage, int cx, int cy, int width, int height, string fillColor, object htmlAttributes)
        {
            if (width == height)
                return CreateCanvasCircle(name, notSupportedMessage, cx, cy, width / 2, 0, fillColor, htmlAttributes, DrawMode.Fill);
            return CreateCanvasEllipse(name, notSupportedMessage, cx, cy, width, height, 0, fillColor, htmlAttributes, DrawMode.Fill);
        }

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
        public MvcHtmlString Circle(string name, string notSupportedMessage, int x, int y, float radius, int thickness, string lineColor, object htmlAttributes)
        {
            return CreateCanvasCircle(name, notSupportedMessage, x, y, radius, thickness, lineColor, htmlAttributes, DrawMode.Stroke);
        }

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
        public MvcHtmlString FilledCircle(string name, string notSupportedMessage, int x, int y, float radius, string fillColor, object htmlAttributes)
        {
            return CreateCanvasCircle(name, notSupportedMessage, x, y, radius, 0, fillColor, htmlAttributes, DrawMode.Fill);
        }

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
        public MvcHtmlString Arc(string name, string notSupportedMessage, int x, int y, float radius, int thickness, float startAngle, float endAngle, string lineColor, object htmlAttributes)
        {
            return CreateCanvasArc(name, notSupportedMessage, x, y, radius, thickness, startAngle, endAngle, lineColor, htmlAttributes, DrawMode.Stroke);
        }

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
        public MvcHtmlString FilledArc(string name, string notSupportedMessage, int x, int y, float radius, float startAngle, float endAngle, string fillColor, object htmlAttributes)
        {
            return CreateCanvasArc(name, notSupportedMessage, x, y, radius, 0, startAngle, endAngle, fillColor, htmlAttributes, DrawMode.Fill);
        }

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
        public MvcHtmlString BeizerCurve(string name, string notSupportedMessage, int x1, int y1, int x2, int y2, float radius, int thickness, string lineColor, object htmlAttributes)
        {
            return CreateCanvasBeizerCurve(name, notSupportedMessage, x1, y1, x2, y2, radius, thickness, lineColor, htmlAttributes, DrawMode.Stroke);
        }

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
        public MvcHtmlString FilledBeizerCurve(string name, string notSupportedMessage, int x1, int y1, int x2, int y2, float radius, string fillColor, object htmlAttributes)
        {
            return CreateCanvasBeizerCurve(name, notSupportedMessage, x1, y1, x2, y2, radius, 0, fillColor, htmlAttributes, DrawMode.Fill);
        }

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
        public MvcHtmlString QuadraticCurve(string name, string notSupportedMessage, int x1, int y1, int x2, int y2, int thickness, string lineColor, object htmlAttributes)
        {
            return CreateCanvasQuadraticCurve(name, notSupportedMessage, x1, y1, x2, y2, thickness, lineColor, htmlAttributes, DrawMode.Stroke);
        }

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
        public MvcHtmlString FilledQuadraticCurve(string name, string notSupportedMessage, int x1, int y1, int x2, int y2, string fillColor, object htmlAttributes)
        {
            return CreateCanvasQuadraticCurve(name, notSupportedMessage, x1, y1, x2, y2, 0, fillColor, htmlAttributes, DrawMode.Fill);
        }

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
        public MvcHtmlString Polygon(string name, string notSupportedMessage, Point[] points, int thickness, string lineColor, object htmlAttributes)
        {
            return CreateCanvasPolygon(name, notSupportedMessage, points, thickness, lineColor, htmlAttributes, DrawMode.Stroke);
        }

        /// <summary>
        /// Renders a filled polygon element in canvas in the client browser.
        /// </summary>
        /// <param name="name">The name for the canvas, but this renders as ID for the canvas element.</param>
        /// <param name="notSupportedMessage">The message appears when the canvas tag is not supported by the client browser.</param>
        /// <param name="points">The array of the points on which the polygon is drawn.</param>
        /// <param name="fillColor">The fill color of the polygon.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the canvas element.</param>
        /// <returns>A canvas element containing a filled polygon.</returns>
        public MvcHtmlString FilledPolygon(string name, string notSupportedMessage, Point[] points, string fillColor, object htmlAttributes)
        {
            return CreateCanvasPolygon(name, notSupportedMessage, points, 0, fillColor, htmlAttributes, DrawMode.Fill);
        }

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
        public MvcHtmlString Text(string name, string notSupportedMessage, int x, int y, string text, string fontFamily, int fontSize, string fontStyle, int thickness, string lineColor, object htmlAttributes)
        {
            return CreateCanvasText(name, notSupportedMessage, x, y, text, fontFamily, fontSize, fontStyle, thickness, lineColor, htmlAttributes, DrawMode.Stroke);
        }

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
        public MvcHtmlString FilledText(string name, string notSupportedMessage, int x, int y, string text, string fontFamily, int fontSize, string fontStyle, string fillColor, object htmlAttributes)
        {
            return CreateCanvasText(name, notSupportedMessage, x, y, text, fontFamily, fontSize, fontStyle, 0, fillColor, htmlAttributes, DrawMode.Fill);
        }

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
        public MvcHtmlString Line(string name, string notSupportedMessage, int x1, int y1, int x2, int y2, int thickness, string lineColor, object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("canvas");
            if (htmlAttributes != null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary(htmlAttributes);
                tagBuilder.MergeAttributes(routeValueDictionary);
            }
            tagBuilder.MergeAttribute("id", name);
            tagBuilder.InnerHtml = notSupportedMessage;
            string canvasTag = tagBuilder.ToString(TagRenderMode.Normal);
            TagBuilder scriptTagBuilder = new TagBuilder("script");
            scriptTagBuilder.MergeAttribute("type", "text/javascript");
            StringBuilder scriptStringBuilder = new StringBuilder();
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendFormat("var c = document.getElementById('{0}');", name);
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendLine("var context=c.getContext('2d');");
            scriptStringBuilder.AppendFormat("context.strokeStyle = '{0}';", lineColor);
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendFormat("context.lineWidth = {0};", thickness);
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendFormat("context.moveTo({0},{1});", x1, y1);
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendFormat("context.lineTo({0},{1});", x2, y2);
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendLine("context.stroke();");
            scriptTagBuilder.InnerHtml = scriptStringBuilder.ToString();
            string scriptTag = scriptTagBuilder.ToString(TagRenderMode.Normal);
            StringBuilder tagStringBuilder = new StringBuilder();
            tagStringBuilder.AppendLine(canvasTag);
            tagStringBuilder.AppendLine(scriptTag);
            return MvcHtmlString.Create(tagStringBuilder.ToString());
        }

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
        public MvcHtmlString Image(string name, string notSupportedMessage, int x, int y, int width, int height, string imageUrl, object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("canvas");
            if (htmlAttributes != null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary(htmlAttributes);
                tagBuilder.MergeAttributes(routeValueDictionary);
            }
            tagBuilder.MergeAttribute("id", name);
            tagBuilder.InnerHtml = notSupportedMessage;
            string canvasTag = tagBuilder.ToString(TagRenderMode.Normal);
            TagBuilder scriptTagBuilder = new TagBuilder("script");
            scriptTagBuilder.MergeAttribute("type", "text/javascript");
            StringBuilder scriptStringBuilder = new StringBuilder();
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendFormat("var c = document.getElementById('{0}');", name);
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendLine("var context=c.getContext('2d');");
            string url = UrlHelper.GenerateContentUrl(imageUrl, htmlHelper.ViewContext.HttpContext);
            scriptStringBuilder.AppendLine("var imgObj = new Image();");
            scriptStringBuilder.AppendFormat("imgObj.src = '{0}';", url);
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendLine("imgObj.onload = function () {");
            scriptStringBuilder.AppendFormat("    context.drawImage(imgObj,{0},{1},{2},{3});", x, y, width, height);
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendLine("}");
            scriptTagBuilder.InnerHtml = scriptStringBuilder.ToString();
            string scriptTag = scriptTagBuilder.ToString(TagRenderMode.Normal);
            StringBuilder tagStringBuilder = new StringBuilder();
            tagStringBuilder.AppendLine(canvasTag);
            tagStringBuilder.AppendLine(scriptTag);
            return MvcHtmlString.Create(tagStringBuilder.ToString());
        }

        /// <summary>
        /// Creates a rectangle in the canvas element using javascript.
        /// </summary>
        private MvcHtmlString CreateCanvasRect(string name, string notSupportedMessage, int xPosition, int yPosition, int rectWidth, int rectHeight, int thickness, string color, object htmlAttributes, DrawMode drawMode)
        {
            TagBuilder tagBuilder = new TagBuilder("canvas");
            if (htmlAttributes != null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary(htmlAttributes);
                tagBuilder.MergeAttributes(routeValueDictionary);
            }
            tagBuilder.MergeAttribute("id", name);
            tagBuilder.InnerHtml = notSupportedMessage;
            string canvasTag = tagBuilder.ToString(TagRenderMode.Normal);
            TagBuilder scriptTagBuilder = new TagBuilder("script");
            scriptTagBuilder.MergeAttribute("type", "text/javascript");
            StringBuilder scriptStringBuilder = new StringBuilder();
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendFormat("var c = document.getElementById('{0}');", name);
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendLine("var context=c.getContext('2d');");
            if (drawMode == DrawMode.Fill)
                scriptStringBuilder.AppendFormat("context.fillStyle = '{0}';", color);
            else
                scriptStringBuilder.AppendFormat("context.strokeStyle = '{0}';", color);
            scriptStringBuilder.AppendLine();
            if (drawMode == DrawMode.Stroke)
                scriptStringBuilder.AppendFormat("context.lineWidth = {0};", thickness);
            scriptStringBuilder.AppendLine();
            if (drawMode == DrawMode.Fill)
                scriptStringBuilder.AppendFormat("context.fillRect({0},{1},{2},{3});", xPosition, yPosition, rectWidth, rectHeight);
            else
                scriptStringBuilder.AppendFormat("context.strokeRect({0},{1},{2},{3});", xPosition, yPosition, rectWidth, rectHeight);
            scriptStringBuilder.AppendLine();
            scriptTagBuilder.InnerHtml = scriptStringBuilder.ToString();
            string scriptTag = scriptTagBuilder.ToString(TagRenderMode.Normal);
            StringBuilder tagStringBuilder = new StringBuilder();
            tagStringBuilder.AppendLine(canvasTag);
            tagStringBuilder.AppendLine(scriptTag);
            return MvcHtmlString.Create(tagStringBuilder.ToString());
        }

        /// <summary>
        /// Creates a ellipse in the canvas element using javascript.
        /// </summary>
        private MvcHtmlString CreateCanvasEllipse(string name, string notSupportedMessage, int centerX, int centerY, int width, int height, int thickness, string color, object htmlAttributes, DrawMode drawMode)
        {
            TagBuilder tagBuilder = new TagBuilder("canvas");
            if (htmlAttributes != null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary(htmlAttributes);
                tagBuilder.MergeAttributes(routeValueDictionary);
            }
            tagBuilder.MergeAttribute("id", name);
            tagBuilder.InnerHtml = notSupportedMessage;
            string canvasTag = tagBuilder.ToString(TagRenderMode.Normal);
            TagBuilder scriptTagBuilder = new TagBuilder("script");
            scriptTagBuilder.MergeAttribute("type", "text/javascript");
            StringBuilder scriptStringBuilder = new StringBuilder();
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendFormat("var c = document.getElementById('{0}');", name);
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendLine("var context=c.getContext('2d');");
            if (drawMode == DrawMode.Fill)
                scriptStringBuilder.AppendFormat("context.fillStyle = '{0}';", color);
            else
                scriptStringBuilder.AppendFormat("context.strokeStyle = '{0}';", color);
            scriptStringBuilder.AppendLine();
            if (drawMode == DrawMode.Stroke)
                scriptStringBuilder.AppendFormat("context.lineWidth = {0};", thickness);
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendLine("context.beginPath();");
            scriptStringBuilder.AppendFormat("context.moveTo({0}, {1} - {2} / 2);", centerX, centerY, height);
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendFormat("context.bezierCurveTo({0} + {1} / 2, {2} - {3} / 2, {0} + {1} / 2, {2} + {3} / 2, {0}, {2} + {3} / 2);", centerX, width, centerY, height);
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendFormat("context.bezierCurveTo({0} - {1} / 2, {2} + {3} / 2, {0} - {1} / 2, {2} - {3} / 2, {0}, {2} - {3} / 2);", centerX, width, centerY, height);
            scriptStringBuilder.AppendLine();
            if (drawMode == DrawMode.Fill)
                scriptStringBuilder.AppendLine("context.fill();");
            else
                scriptStringBuilder.AppendLine("context.stroke();");
            scriptStringBuilder.AppendLine("context.closePath();");
            scriptStringBuilder.AppendLine();
            scriptTagBuilder.InnerHtml = scriptStringBuilder.ToString();
            string scriptTag = scriptTagBuilder.ToString(TagRenderMode.Normal);
            StringBuilder tagStringBuilder = new StringBuilder();
            tagStringBuilder.AppendLine(canvasTag);
            tagStringBuilder.AppendLine(scriptTag);
            return MvcHtmlString.Create(tagStringBuilder.ToString());
        }

        /// <summary>
        /// Creates a circle in the canvas element using javascript.
        /// </summary>
        private MvcHtmlString CreateCanvasCircle(string name, string notSupportedMessage, int centerX, int centerY, float radius, int thickness, string color, object htmlAttributes, DrawMode drawMode)
        {
            TagBuilder tagBuilder = new TagBuilder("canvas");
            if (htmlAttributes != null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary(htmlAttributes);
                tagBuilder.MergeAttributes(routeValueDictionary);
            }
            tagBuilder.MergeAttribute("id", name);
            tagBuilder.InnerHtml = notSupportedMessage;
            string canvasTag = tagBuilder.ToString(TagRenderMode.Normal);
            TagBuilder scriptTagBuilder = new TagBuilder("script");
            scriptTagBuilder.MergeAttribute("type", "text/javascript");
            StringBuilder scriptStringBuilder = new StringBuilder();
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendFormat("var c = document.getElementById('{0}');", name);
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendLine("var context=c.getContext('2d');");
            if (drawMode == DrawMode.Fill)
                scriptStringBuilder.AppendFormat("context.fillStyle = '{0}';", color);
            else
                scriptStringBuilder.AppendFormat("context.strokeStyle = '{0}';", color);
            scriptStringBuilder.AppendLine();
            if (drawMode == DrawMode.Stroke)
                scriptStringBuilder.AppendFormat("context.lineWidth = {0};", thickness);
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendFormat("context.arc({0}, {1}, {2}, 0, Math.PI * 2, true);", centerX, centerY, radius);
            scriptStringBuilder.AppendLine();
            if (drawMode == DrawMode.Fill)
                scriptStringBuilder.AppendLine("context.fill();");
            else
                scriptStringBuilder.AppendLine("context.stroke();");
            scriptTagBuilder.InnerHtml = scriptStringBuilder.ToString();
            string scriptTag = scriptTagBuilder.ToString(TagRenderMode.Normal);
            StringBuilder tagStringBuilder = new StringBuilder();
            tagStringBuilder.AppendLine(canvasTag);
            tagStringBuilder.AppendLine(scriptTag);
            return MvcHtmlString.Create(tagStringBuilder.ToString());
        }

        /// <summary>
        /// Creates a arc in the canvas element using javascript.
        /// </summary>
        private MvcHtmlString CreateCanvasArc(string name, string notSupportedMessage, int centerX, int centerY, float radius, int thickness, float startAngle, float endAngle, string color, object htmlAttributes, DrawMode drawMode)
        {
            TagBuilder tagBuilder = new TagBuilder("canvas");
            if (htmlAttributes != null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary(htmlAttributes);
                tagBuilder.MergeAttributes(routeValueDictionary);
            }
            tagBuilder.MergeAttribute("id", name);
            tagBuilder.InnerHtml = notSupportedMessage;
            string canvasTag = tagBuilder.ToString(TagRenderMode.Normal);
            TagBuilder scriptTagBuilder = new TagBuilder("script");
            scriptTagBuilder.MergeAttribute("type", "text/javascript");
            StringBuilder scriptStringBuilder = new StringBuilder();
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendFormat("var c = document.getElementById('{0}');", name);
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendLine("var context=c.getContext('2d');");
            if (drawMode == DrawMode.Fill)
                scriptStringBuilder.AppendFormat("context.fillStyle = '{0}';", color);
            else
                scriptStringBuilder.AppendFormat("context.strokeStyle = '{0}';", color);
            scriptStringBuilder.AppendLine();
            if (drawMode == DrawMode.Stroke)
                scriptStringBuilder.AppendFormat("context.lineWidth = {0};", thickness);
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendFormat("context.arc({0}, {1}, {2}, {3} * Math.PI/180, {4} * Math.PI/180, false);", centerX, centerY, radius, startAngle, endAngle);
            scriptStringBuilder.AppendLine();
            if (drawMode == DrawMode.Fill)
                scriptStringBuilder.AppendLine("context.fill();");
            else
                scriptStringBuilder.AppendLine("context.stroke();");
            scriptTagBuilder.InnerHtml = scriptStringBuilder.ToString();
            string scriptTag = scriptTagBuilder.ToString(TagRenderMode.Normal);
            StringBuilder tagStringBuilder = new StringBuilder();
            tagStringBuilder.AppendLine(canvasTag);
            tagStringBuilder.AppendLine(scriptTag);
            return MvcHtmlString.Create(tagStringBuilder.ToString());
        }

        /// <summary>
        /// Creates a beizer curve in the canvas element using javascript.
        /// </summary>
        private MvcHtmlString CreateCanvasBeizerCurve(string name, string notSupportedMessage, int startX, int startY, int endX, int endY, float radius, int thickness, string color, object htmlAttributes, DrawMode drawMode)
        {
            TagBuilder tagBuilder = new TagBuilder("canvas");
            if (htmlAttributes != null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary(htmlAttributes);
                tagBuilder.MergeAttributes(routeValueDictionary);
            }
            tagBuilder.MergeAttribute("id", name);
            tagBuilder.InnerHtml = notSupportedMessage;
            string canvasTag = tagBuilder.ToString(TagRenderMode.Normal);
            TagBuilder scriptTagBuilder = new TagBuilder("script");
            scriptTagBuilder.MergeAttribute("type", "text/javascript");
            StringBuilder scriptStringBuilder = new StringBuilder();
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendFormat("var c = document.getElementById('{0}');", name);
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendLine("var context=c.getContext('2d');");
            if (drawMode == DrawMode.Fill)
                scriptStringBuilder.AppendFormat("context.fillStyle = '{0}';", color);
            else
                scriptStringBuilder.AppendFormat("context.strokeStyle = '{0}';", color);
            scriptStringBuilder.AppendLine();
            if (drawMode == DrawMode.Stroke)
                scriptStringBuilder.AppendFormat("context.lineWidth = {0};", thickness);
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendLine("context.beginPath();");
            scriptStringBuilder.AppendFormat("context.moveTo({0}, {1});", startX, startY);
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendFormat("context.bezierCurveTo({0}, {1}, {2}, {3} , {4}* Math.PI/180, {5} * Math.PI/180);", startX, startY, endX, endY, radius, 200);
            scriptStringBuilder.AppendLine();
            if (drawMode == DrawMode.Fill)
                scriptStringBuilder.AppendLine("context.fill();");
            else
                scriptStringBuilder.AppendLine("context.stroke();");
            scriptStringBuilder.AppendLine("context.closePath();");
            scriptStringBuilder.AppendLine();
            scriptTagBuilder.InnerHtml = scriptStringBuilder.ToString();
            string scriptTag = scriptTagBuilder.ToString(TagRenderMode.Normal);
            StringBuilder tagStringBuilder = new StringBuilder();
            tagStringBuilder.AppendLine(canvasTag);
            tagStringBuilder.AppendLine(scriptTag);
            return MvcHtmlString.Create(tagStringBuilder.ToString());
        }

        /// <summary>
        /// Creates a quadratic curve in the canvas element using javascript.
        /// </summary>
        private MvcHtmlString CreateCanvasQuadraticCurve(string name, string notSupportedMessage, int startX, int startY, int endX, int endY, int thickness, string color, object htmlAttributes, DrawMode drawMode)
        {
            TagBuilder tagBuilder = new TagBuilder("canvas");
            if (htmlAttributes != null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary(htmlAttributes);
                tagBuilder.MergeAttributes(routeValueDictionary);
            }
            tagBuilder.MergeAttribute("id", name);
            tagBuilder.InnerHtml = notSupportedMessage;
            string canvasTag = tagBuilder.ToString(TagRenderMode.Normal);
            TagBuilder scriptTagBuilder = new TagBuilder("script");
            scriptTagBuilder.MergeAttribute("type", "text/javascript");
            StringBuilder scriptStringBuilder = new StringBuilder();
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendFormat("var c = document.getElementById('{0}');", name);
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendLine("var context=c.getContext('2d');");
            if (drawMode == DrawMode.Fill)
                scriptStringBuilder.AppendFormat("context.fillStyle = '{0}';", color);
            else
                scriptStringBuilder.AppendFormat("context.strokeStyle = '{0}';", color);
            scriptStringBuilder.AppendLine();
            if (drawMode == DrawMode.Stroke)
                scriptStringBuilder.AppendFormat("context.lineWidth = {0};", thickness);
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendLine("context.beginPath();");
            scriptStringBuilder.AppendFormat("context.moveTo({0}, {1});", startX, startY);
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendFormat("context.quadraticCurveTo({0}, {1}, {2}, {3});", startX, startY, endX, endY);
            scriptStringBuilder.AppendLine();
            if (drawMode == DrawMode.Fill)
                scriptStringBuilder.AppendLine("context.fill();");
            else
                scriptStringBuilder.AppendLine("context.stroke();");
            scriptStringBuilder.AppendLine("context.closePath();");
            scriptStringBuilder.AppendLine();
            scriptTagBuilder.InnerHtml = scriptStringBuilder.ToString();
            string scriptTag = scriptTagBuilder.ToString(TagRenderMode.Normal);
            StringBuilder tagStringBuilder = new StringBuilder();
            tagStringBuilder.AppendLine(canvasTag);
            tagStringBuilder.AppendLine(scriptTag);
            return MvcHtmlString.Create(tagStringBuilder.ToString());
        }

        /// <summary>
        /// Creates a polygon in the canvas element using javascript.
        /// </summary>
        private MvcHtmlString CreateCanvasPolygon(string name, string notSupportedMessage, Point[] points, int thickness, string color, object htmlAttributes, DrawMode drawMode)
        {
            TagBuilder tagBuilder = new TagBuilder("canvas");
            if (htmlAttributes != null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary(htmlAttributes);
                tagBuilder.MergeAttributes(routeValueDictionary);
            }
            tagBuilder.MergeAttribute("id", name);
            tagBuilder.InnerHtml = notSupportedMessage;
            string canvasTag = tagBuilder.ToString(TagRenderMode.Normal);
            TagBuilder scriptTagBuilder = new TagBuilder("script");
            scriptTagBuilder.MergeAttribute("type", "text/javascript");
            StringBuilder scriptStringBuilder = new StringBuilder();
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendFormat("var c = document.getElementById('{0}');", name);
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendLine("var context=c.getContext('2d');");
            if (drawMode == DrawMode.Fill)
                scriptStringBuilder.AppendFormat("context.fillStyle = '{0}';", color);
            else
                scriptStringBuilder.AppendFormat("context.strokeStyle = '{0}';", color);
            scriptStringBuilder.AppendLine();
            if (drawMode == DrawMode.Stroke)
                scriptStringBuilder.AppendFormat("context.lineWidth = {0};", thickness);
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendLine("context.beginPath();");
            string pointsStr = "";
            foreach (Point point in points)
            {
                pointsStr += point.X + "," + point.Y + ",";
            }
            pointsStr = "var points = [" + pointsStr.TrimEnd(',') + "];";
            scriptStringBuilder.AppendLine(pointsStr);
            scriptStringBuilder.AppendLine("context.moveTo(points[0], points[1]);");
            scriptStringBuilder.AppendLine("for( item=2 ; item < points.length-1 ; item+=2 ){ context.lineTo( points[item] , points[item+1] ) }");
            scriptStringBuilder.AppendLine("context.closePath();");
            if (drawMode == DrawMode.Fill)
                scriptStringBuilder.AppendLine("context.fill();");
            else
                scriptStringBuilder.AppendLine("context.stroke();");
            scriptStringBuilder.AppendLine();
            scriptTagBuilder.InnerHtml = scriptStringBuilder.ToString();
            string scriptTag = scriptTagBuilder.ToString(TagRenderMode.Normal);
            StringBuilder tagStringBuilder = new StringBuilder();
            tagStringBuilder.AppendLine(canvasTag);
            tagStringBuilder.AppendLine(scriptTag);
            return MvcHtmlString.Create(tagStringBuilder.ToString());
        }

        /// <summary>
        /// Creates a text in the canvas element using javascript.
        /// </summary>
        private MvcHtmlString CreateCanvasText(string name, string notSupportedMessage, int xPosition, int yPosition, string text, string fontFamily, int fontSize, string fontStyle, int thickness, string color, object htmlAttributes, DrawMode drawMode)
        {
            TagBuilder tagBuilder = new TagBuilder("canvas");
            if (htmlAttributes != null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary(htmlAttributes);
                tagBuilder.MergeAttributes(routeValueDictionary);
            }
            tagBuilder.MergeAttribute("id", name);
            tagBuilder.InnerHtml = notSupportedMessage;
            string canvasTag = tagBuilder.ToString(TagRenderMode.Normal);
            TagBuilder scriptTagBuilder = new TagBuilder("script");
            scriptTagBuilder.MergeAttribute("type", "text/javascript");
            StringBuilder scriptStringBuilder = new StringBuilder();
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendFormat("var c = document.getElementById('{0}');", name);
            scriptStringBuilder.AppendLine();
            scriptStringBuilder.AppendLine("var context=c.getContext('2d');");
            if (drawMode == DrawMode.Fill)
                scriptStringBuilder.AppendFormat("context.fillStyle = '{0}';", color);
            else
                scriptStringBuilder.AppendFormat("context.strokeStyle = '{0}';", color);
            scriptStringBuilder.AppendLine();
            if (drawMode == DrawMode.Stroke)
                scriptStringBuilder.AppendFormat("context.lineWidth = {0};", thickness);
            scriptStringBuilder.AppendLine("context.textBaseline = 'top';");
            scriptStringBuilder.AppendFormat("context.font = '{0} {1}px {2}';", fontStyle, fontSize, fontFamily);
            scriptStringBuilder.AppendLine();
            if (drawMode == DrawMode.Fill)
                scriptStringBuilder.AppendFormat("context.fillText('{0}', {1}, {2});", text, xPosition, yPosition);
            else
                scriptStringBuilder.AppendFormat("context.strokeText('{0}', {1}, {2});", text, xPosition, yPosition);
            scriptStringBuilder.AppendLine();
            scriptTagBuilder.InnerHtml = scriptStringBuilder.ToString();
            string scriptTag = scriptTagBuilder.ToString(TagRenderMode.Normal);
            StringBuilder tagStringBuilder = new StringBuilder();
            tagStringBuilder.AppendLine(canvasTag);
            tagStringBuilder.AppendLine(scriptTag);
            return MvcHtmlString.Create(tagStringBuilder.ToString());
        }
    }
}