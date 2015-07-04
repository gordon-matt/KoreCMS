/*
 * Many thanks to Saleth Prakash: http://www.codeproject.com/Articles/237411/Html-Controls-for-ASP-Net-MVC
*/

namespace Kore.Web.Mvc.Html
{
    using System.Drawing;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// A Html5 helper class which contains all the svg related elements.
    /// </summary>
    public class Html5SvgHelper : IHtml5SvgHelper
    {
        private HtmlHelper htmlHelper;

        /// <summary>
        /// Initializes the Html5 helper class of svg related elements.
        /// </summary>
        /// <param name="htmlHelper">The html helper class instance that comes from the current view context.</param>
        public Html5SvgHelper(HtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        /// <summary>
        /// Renders a empty svg element in the client browser.
        /// </summary>
        /// <param name="name">The name for the svg, but this renders as ID for the svg element.</param>
        /// <param name="notSupportedMessage">The message appears when the svg tag is not supported by the client browser.</param>
        /// <returns>A svg element.</returns>
        public MvcHtmlString EmptySvg(string name, string notSupportedMessage)
        {
            return EmptySvg(name, notSupportedMessage, null);
        }

        /// <summary>
        /// Renders a empty svg element in the client browser.
        /// </summary>
        /// <param name="name">The name for the svg, but this renders as ID for the svg element.</param>
        /// <param name="notSupportedMessage">The message appears when the svg tag is not supported by the client browser.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the svg element.</param>
        /// <returns>A svg element.</returns>
        public MvcHtmlString EmptySvg(string name, string notSupportedMessage, object htmlAttributes)
        {
            TagBuilder tagBuilder = CreateSvgTag(name, notSupportedMessage, htmlAttributes);
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }

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
        public MvcHtmlString Rectangle(string name, string notSupportedMessage, int x, int y, int width, int height, int thickness, string lineColor, object htmlAttributes)
        {
            return CreateSvgRect(name, notSupportedMessage, x, y, width, height, thickness, lineColor, htmlAttributes, DrawMode.Stroke);
        }

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
        public MvcHtmlString FilledRectangle(string name, string notSupportedMessage, int x, int y, int width, int height, string fillColor, object htmlAttributes)
        {
            return CreateSvgRect(name, notSupportedMessage, x, y, width, height, 0, fillColor, htmlAttributes, DrawMode.Fill);
        }

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
        public MvcHtmlString Ellipse(string name, string notSupportedMessage, int cx, int cy, int width, int height, int thickness, string lineColor, object htmlAttributes)
        {
            if (width == height)
                return CreateSvgCircle(name, notSupportedMessage, cx, cy, width / 2, thickness, lineColor, htmlAttributes, DrawMode.Stroke);
            return CreateSvgEllipse(name, notSupportedMessage, cx, cy, width, height, thickness, lineColor, htmlAttributes, DrawMode.Stroke);
        }

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
        public MvcHtmlString FilledEllipse(string name, string notSupportedMessage, int cx, int cy, int width, int height, string fillColor, object htmlAttributes)
        {
            if (width == height)
                return CreateSvgCircle(name, notSupportedMessage, cx, cy, width / 2, 0, fillColor, htmlAttributes, DrawMode.Fill);
            return CreateSvgEllipse(name, notSupportedMessage, cx, cy, width, height, 0, fillColor, htmlAttributes, DrawMode.Fill);
        }

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
        public MvcHtmlString Circle(string name, string notSupportedMessage, int cx, int cy, float radius, int thickness, string lineColor, object htmlAttributes)
        {
            return CreateSvgCircle(name, notSupportedMessage, cx, cy, radius, thickness, lineColor, htmlAttributes, DrawMode.Stroke);
        }

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
        public MvcHtmlString FilledCircle(string name, string notSupportedMessage, int cx, int cy, float radius, string fillColor, object htmlAttributes)
        {
            return CreateSvgCircle(name, notSupportedMessage, cx, cy, radius, 0, fillColor, htmlAttributes, DrawMode.Fill);
        }

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
        public MvcHtmlString Polygon(string name, string notSupportedMessage, Point[] points, int thickness, string lineColor, object htmlAttributes)
        {
            return CreateSvgPolygon(name, notSupportedMessage, points, thickness, lineColor, htmlAttributes, DrawMode.Stroke);
        }

        /// <summary>
        /// Renders a svg element containing a filled polygon in the client browser.
        /// </summary>
        /// <param name="name">The name for the svg, but this renders as ID for the svg element.</param>
        /// <param name="notSupportedMessage">The message appears when the svg tag is not supported by the client browser.</param>
        /// <param name="points">The points on which the polygon is drawn.</param>
        /// <param name="fillColor">The fill color of the polygon.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the svg element.</param>
        /// <returns>A svg element containing a filled polygon.</returns>
        public MvcHtmlString FilledPolygon(string name, string notSupportedMessage, Point[] points, string fillColor, object htmlAttributes)
        {
            return CreateSvgPolygon(name, notSupportedMessage, points, 0, fillColor, htmlAttributes, DrawMode.Fill);
        }

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
        public MvcHtmlString Text(string name, string notSupportedMessage, int x, int y, string text, string fontFamily, int fontSize, string fontWeight, string fontStyle, string textDecoration, int thickness, string lineColor, object htmlAttributes)
        {
            return CreateSvgText(name, notSupportedMessage, x, y, text, fontFamily, fontSize, fontWeight, fontStyle, textDecoration, thickness, lineColor, htmlAttributes, DrawMode.Stroke);
        }

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
        public MvcHtmlString FilledText(string name, string notSupportedMessage, int x, int y, string text, string fontFamily, int fontSize, string fontWeight, string fontStyle, string textDecoration, string fillColor, object htmlAttributes)
        {
            return CreateSvgText(name, notSupportedMessage, x, y, text, fontFamily, fontSize, fontWeight, fontStyle, textDecoration, 0, fillColor, htmlAttributes, DrawMode.Fill);
        }

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
        public MvcHtmlString Line(string name, string notSupportedMessage, int x1, int y1, int x2, int y2, int thickness, string lineColor, object htmlAttributes)
        {
            TagBuilder tagBuilder = CreateSvgTag(name, notSupportedMessage, htmlAttributes);
            TagBuilder lineTagBuilder = new TagBuilder("line");
            lineTagBuilder.MergeAttribute("id", name + "line");
            lineTagBuilder.MergeAttribute("x1", x1.ToString());
            lineTagBuilder.MergeAttribute("y1", y1.ToString());
            lineTagBuilder.MergeAttribute("x2", x2.ToString());
            lineTagBuilder.MergeAttribute("y2", y2.ToString());
            lineTagBuilder.MergeAttribute("stroke", lineColor);
            lineTagBuilder.MergeAttribute("stroke-width", thickness + "px");
            string lineTag = lineTagBuilder.ToString(TagRenderMode.SelfClosing);
            tagBuilder.InnerHtml = string.Format("{0}{1}{0}{2}", System.Environment.NewLine, lineTag, tagBuilder.InnerHtml);
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }

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
        public MvcHtmlString PolyLine(string name, string notSupportedMessage, Point[] points, int thickness, string lineColor, object htmlAttributes)
        {
            TagBuilder tagBuilder = CreateSvgTag(name, notSupportedMessage, htmlAttributes);
            TagBuilder polylineTagBuilder = new TagBuilder("polyline");
            polylineTagBuilder.MergeAttribute("id", name + "Polyline");
            string pointsStr = "";
            foreach (Point point in points)
            {
                pointsStr += point.X + "," + point.Y + " ";
            }
            pointsStr = pointsStr.TrimEnd(' ');
            polylineTagBuilder.MergeAttribute("points", pointsStr);
            polylineTagBuilder.MergeAttribute("fill", "#FFFFFF");
            polylineTagBuilder.MergeAttribute("stroke", lineColor);
            polylineTagBuilder.MergeAttribute("stroke-width", thickness + "px");
            string polylineTag = polylineTagBuilder.ToString(TagRenderMode.SelfClosing);
            tagBuilder.InnerHtml = string.Format("{0}{1}{0}{2}", System.Environment.NewLine, polylineTag, tagBuilder.InnerHtml);
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }

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
        public MvcHtmlString Image(string name, string notSupportedMessage, int x, int y, int width, int height, string imageUrl, object htmlAttributes)
        {
            TagBuilder tagBuilder = CreateSvgTag(name, notSupportedMessage, htmlAttributes);
            TagBuilder imgTagBuilder = new TagBuilder("image");
            imgTagBuilder.MergeAttribute("id", name + "Image");
            imgTagBuilder.MergeAttribute("x", x.ToString());
            imgTagBuilder.MergeAttribute("y", y.ToString());
            imgTagBuilder.MergeAttribute("width", width.ToString());
            imgTagBuilder.MergeAttribute("height", height.ToString());
            string url = UrlHelper.GenerateContentUrl(imageUrl, htmlHelper.ViewContext.HttpContext);
            imgTagBuilder.MergeAttribute("xlink:href", url);
            string imgTag = imgTagBuilder.ToString(TagRenderMode.SelfClosing);
            tagBuilder.InnerHtml = string.Format("{0}{1}{0}{2}", System.Environment.NewLine, imgTag, tagBuilder.InnerHtml);
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Creates a rectangle in a svg element.
        /// </summary>
        private MvcHtmlString CreateSvgRect(string name, string notSupportedMessage, int xPosition, int yPosition, int rectWidth, int rectHeight, int thickness, string color, object htmlAttributes, DrawMode drawMode)
        {
            TagBuilder tagBuilder = CreateSvgTag(name, notSupportedMessage, htmlAttributes);
            TagBuilder rectTagBuilder = new TagBuilder("rect");
            rectTagBuilder.MergeAttribute("id", name + "Rect");
            rectTagBuilder.MergeAttribute("x", xPosition.ToString());
            rectTagBuilder.MergeAttribute("y", yPosition.ToString());
            rectTagBuilder.MergeAttribute("width", rectWidth.ToString());
            rectTagBuilder.MergeAttribute("height", rectHeight.ToString());
            if (drawMode == DrawMode.Fill)
                rectTagBuilder.MergeAttribute("fill", color);
            else
            {
                rectTagBuilder.MergeAttribute("fill", "#FFFFFF");
                rectTagBuilder.MergeAttribute("stroke", color);
                rectTagBuilder.MergeAttribute("stroke-width", thickness + "px");
            }
            string rectTag = rectTagBuilder.ToString(TagRenderMode.SelfClosing);
            tagBuilder.InnerHtml = string.Format("{0}{1}{0}{2}", System.Environment.NewLine, rectTag, tagBuilder.InnerHtml);
            return MvcHtmlString.Create(tagBuilder.ToString());
        }

        /// <summary>
        /// Creates a ellipse in a svg element.
        /// </summary>
        private MvcHtmlString CreateSvgEllipse(string name, string notSupportedMessage, int centerX, int centerY, int width, int height, int thickness, string color, object htmlAttributes, DrawMode drawMode)
        {
            TagBuilder tagBuilder = CreateSvgTag(name, notSupportedMessage, htmlAttributes);
            TagBuilder ellipseTagBuilder = new TagBuilder("ellipse");
            ellipseTagBuilder.MergeAttribute("id", name + "Ellipse");
            ellipseTagBuilder.MergeAttribute("cx", centerX.ToString());
            ellipseTagBuilder.MergeAttribute("cy", centerY.ToString());
            ellipseTagBuilder.MergeAttribute("rx", (width / 2).ToString());
            ellipseTagBuilder.MergeAttribute("ry", (height / 2).ToString());
            if (drawMode == DrawMode.Fill)
                ellipseTagBuilder.MergeAttribute("fill", color);
            else
            {
                ellipseTagBuilder.MergeAttribute("fill", "#FFFFFF");
                ellipseTagBuilder.MergeAttribute("stroke", color);
                ellipseTagBuilder.MergeAttribute("stroke-width", thickness + "px");
            }
            string ellipseTag = ellipseTagBuilder.ToString(TagRenderMode.SelfClosing);
            tagBuilder.InnerHtml = string.Format("{0}{1}{0}{2}", System.Environment.NewLine, ellipseTag, tagBuilder.InnerHtml);
            return MvcHtmlString.Create(tagBuilder.ToString());
        }

        /// <summary>
        /// Creates a circle in a svg element.
        /// </summary>
        private MvcHtmlString CreateSvgCircle(string name, string notSupportedMessage, int centerX, int centerY, float radius, int thickness, string color, object htmlAttributes, DrawMode drawMode)
        {
            TagBuilder tagBuilder = CreateSvgTag(name, notSupportedMessage, htmlAttributes);
            TagBuilder circleTagBuilder = new TagBuilder("circle");
            circleTagBuilder.MergeAttribute("id", name + "Circle");
            circleTagBuilder.MergeAttribute("cx", centerX.ToString());
            circleTagBuilder.MergeAttribute("cy", centerY.ToString());
            circleTagBuilder.MergeAttribute("r", radius.ToString());
            if (drawMode == DrawMode.Fill)
                circleTagBuilder.MergeAttribute("fill", color);
            else
            {
                circleTagBuilder.MergeAttribute("fill", "#FFFFFF");
                circleTagBuilder.MergeAttribute("stroke", color);
                circleTagBuilder.MergeAttribute("stroke-width", thickness + "px");
            }
            string circleTag = circleTagBuilder.ToString(TagRenderMode.SelfClosing);
            tagBuilder.InnerHtml = string.Format("{0}{1}{0}{2}", System.Environment.NewLine, circleTag, tagBuilder.InnerHtml);
            return MvcHtmlString.Create(tagBuilder.ToString());
        }

        /// <summary>
        /// Creates a polygon in a svg element.
        /// </summary>
        private MvcHtmlString CreateSvgPolygon(string name, string notSupportedMessage, Point[] points, int thickness, string color, object htmlAttributes, DrawMode drawMode)
        {
            TagBuilder tagBuilder = CreateSvgTag(name, notSupportedMessage, htmlAttributes);
            TagBuilder polygonTagBuilder = new TagBuilder("polygon");
            polygonTagBuilder.MergeAttribute("id", name + "Polygon");
            string pointsStr = "";
            foreach (Point point in points)
            {
                pointsStr += point.X + "," + point.Y + " ";
            }
            pointsStr = pointsStr.TrimEnd(' ');
            polygonTagBuilder.MergeAttribute("points", pointsStr);
            if (drawMode == DrawMode.Fill)
                polygonTagBuilder.MergeAttribute("fill", color);
            else
            {
                polygonTagBuilder.MergeAttribute("fill", "#FFFFFF");
                polygonTagBuilder.MergeAttribute("stroke", color);
                polygonTagBuilder.MergeAttribute("stroke-width", thickness + "px");
            }
            string polygonTag = polygonTagBuilder.ToString(TagRenderMode.SelfClosing);
            tagBuilder.InnerHtml = string.Format("{0}{1}{0}{2}", System.Environment.NewLine, polygonTag, tagBuilder.InnerHtml);
            return MvcHtmlString.Create(tagBuilder.ToString());
        }

        /// <summary>
        /// Creates a text in a svg element.
        /// </summary>
        private MvcHtmlString CreateSvgText(string name, string notSupportedMessage, int xPosition, int yPosition, string text, string fontFamily, int fontSize, string fontWeight, string fontStyle, string textDecoration, int thickness, string color, object htmlAttributes, DrawMode drawMode)
        {
            TagBuilder tagBuilder = CreateSvgTag(name, notSupportedMessage, htmlAttributes);
            TagBuilder textTagBuilder = new TagBuilder("text");
            textTagBuilder.MergeAttribute("id", name + "Text");
            textTagBuilder.MergeAttribute("x", xPosition.ToString());
            textTagBuilder.MergeAttribute("y", yPosition.ToString());
            textTagBuilder.MergeAttribute("text-anchor", "middle");
            textTagBuilder.MergeAttribute("text-decoration", textDecoration);
            textTagBuilder.MergeAttribute("font-style", fontStyle);
            textTagBuilder.MergeAttribute("font-weight", fontWeight);
            textTagBuilder.MergeAttribute("font-size", fontSize + "px");
            textTagBuilder.MergeAttribute("font-family", fontFamily);
            if (drawMode == DrawMode.Fill)
                textTagBuilder.MergeAttribute("fill", color);
            else
            {
                textTagBuilder.MergeAttribute("fill", "#FFFFFF");
                textTagBuilder.MergeAttribute("stroke", color);
                textTagBuilder.MergeAttribute("stroke-width", thickness + "px");
            }
            textTagBuilder.InnerHtml = text;
            string textTag = textTagBuilder.ToString(TagRenderMode.Normal);
            tagBuilder.InnerHtml = string.Format("{0}{1}{0}{2}", System.Environment.NewLine, textTag, tagBuilder.InnerHtml);
            return MvcHtmlString.Create(tagBuilder.ToString());
        }

        /// <summary>
        /// Creates a svg element.
        /// </summary>
        private static TagBuilder CreateSvgTag(string name, string notSupportedMessage, object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("svg");
            if (htmlAttributes != null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary(htmlAttributes);
                tagBuilder.MergeAttributes(routeValueDictionary);
            }
            tagBuilder.MergeAttribute("xmlns", "http://www.w3.org/2000/svg");
            tagBuilder.MergeAttribute("id", name);
            tagBuilder.InnerHtml = notSupportedMessage;
            return tagBuilder;
        }
    }
}