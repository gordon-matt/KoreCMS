/*
 * Many thanks to Saleth Prakash: http://www.codeproject.com/Articles/237411/Html-Controls-for-ASP-Net-MVC
*/

namespace Kore.Web.Mvc.Html
{
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class Html5SvgExtensions
    {
        public static MvcHtmlString Svg(this HtmlHelper htmlHelper, string name, string notSupportedMessage)
        {
            return Svg(htmlHelper, name, notSupportedMessage, null);
        }

        public static MvcHtmlString Svg(this HtmlHelper htmlHelper, string name, string notSupportedMessage, object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("svg");
            if (htmlAttributes != null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary(htmlAttributes);
                tagBuilder.MergeAttributes(routeValueDictionary);
            }
            tagBuilder.MergeAttribute("id", name);
            tagBuilder.InnerHtml = notSupportedMessage;
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString Rectangle(this HtmlHelper htmlHelper, string name, string notSupportedMessage, int x, int y, int width, int height, int thickness, string borderColor, object htmlAttributes)
        {
            return CreateSvgRect(name, notSupportedMessage, x, y, width, height, thickness, borderColor, htmlAttributes, DrawMode.Stroke);
        }

        public static MvcHtmlString FilledRectangle(this HtmlHelper htmlHelper, string name, string notSupportedMessage, int x, int y, int width, int height, string fillColor, object htmlAttributes)
        {
            return CreateSvgRect(name, notSupportedMessage, x, y, width, height, 0, fillColor, htmlAttributes, DrawMode.Fill);
        }

        public static MvcHtmlString Ellipse(this HtmlHelper htmlHelper, string name, string notSupportedMessage, int cx, int cy, int width, int height, int thickness, string borderColor, object htmlAttributes)
        {
            if (width == height)
                return CreateSvgCircle(name, notSupportedMessage, cx, cy, width / 2, thickness, borderColor, htmlAttributes, DrawMode.Stroke);
            return CreateSvgEllipse(name, notSupportedMessage, cx, cy, width, height, thickness, borderColor, htmlAttributes, DrawMode.Stroke);
        }

        public static MvcHtmlString FilledEllipse(this HtmlHelper htmlHelper, string name, string notSupportedMessage, int cx, int cy, int width, int height, string fillColor, object htmlAttributes)
        {
            if (width == height)
                return CreateSvgCircle(name, notSupportedMessage, cx, cy, width / 2, 0, fillColor, htmlAttributes, DrawMode.Fill);
            return CreateSvgEllipse(name, notSupportedMessage, cx, cy, width, height, 0, fillColor, htmlAttributes, DrawMode.Fill);
        }

        public static MvcHtmlString Circle(this HtmlHelper htmlHelper, string name, string notSupportedMessage, int x, int y, float radius, int thickness, string borderColor, object htmlAttributes)
        {
            return CreateSvgCircle(name, notSupportedMessage, x, y, radius, thickness, borderColor, htmlAttributes, DrawMode.Stroke);
        }

        public static MvcHtmlString FilledCircle(this HtmlHelper htmlHelper, string name, string notSupportedMessage, int x, int y, float radius, string fillColor, object htmlAttributes)
        {
            return CreateSvgCircle(name, notSupportedMessage, x, y, radius, 0, fillColor, htmlAttributes, DrawMode.Fill);
        }

        public static MvcHtmlString Arc(this HtmlHelper htmlHelper, string name, string notSupportedMessage, int x, int y, int thickness, float radius, float startAngle, float endAngle, string borderColor, object htmlAttributes)
        {
            return CreateSvgArc(name, notSupportedMessage, x, y, radius, thickness, startAngle, endAngle, borderColor, htmlAttributes, DrawMode.Stroke);
        }

        public static MvcHtmlString FilledArc(this HtmlHelper htmlHelper, string name, string notSupportedMessage, int x, int y, float radius, float startAngle, float endAngle, string fillColor, object htmlAttributes)
        {
            return CreateSvgArc(name, notSupportedMessage, x, y, radius, 0, startAngle, endAngle, fillColor, htmlAttributes, DrawMode.Fill);
        }

        public static MvcHtmlString BeizerCurve(this HtmlHelper htmlHelper, string name, string notSupportedMessage, int x1, int y1, int x2, int y2, float radius, int thickness, string borderColor, object htmlAttributes)
        {
            return CreateSvgBeizerCurve(name, notSupportedMessage, x1, y1, x2, y2, radius, thickness, borderColor, htmlAttributes, DrawMode.Stroke);
        }

        public static MvcHtmlString FilledBeizerCurve(this HtmlHelper htmlHelper, string name, string notSupportedMessage, int x1, int y1, int x2, int y2, float radius, string fillColor, object htmlAttributes)
        {
            return CreateSvgBeizerCurve(name, notSupportedMessage, x1, y1, x2, y2, radius, 0, fillColor, htmlAttributes, DrawMode.Fill);
        }

        public static MvcHtmlString QuadraticCurve(this HtmlHelper htmlHelper, string name, string notSupportedMessage, int x1, int y1, int x2, int y2, int thickness, string borderColor, object htmlAttributes)
        {
            return CreateSvgQuadraticCurve(name, notSupportedMessage, x1, y1, x2, y2, thickness, borderColor, htmlAttributes, DrawMode.Stroke);
        }

        public static MvcHtmlString FilledQuadraticCurve(this HtmlHelper htmlHelper, string name, string notSupportedMessage, int x, int y, int x1, int y1, int x2, int y2, string fillColor, object htmlAttributes)
        {
            return CreateSvgQuadraticCurve(name, notSupportedMessage, x1, y1, x2, y2, 0, fillColor, htmlAttributes, DrawMode.Fill);
        }

        public static MvcHtmlString Polygon(this HtmlHelper htmlHelper, string name, string notSupportedMessage, int[] points, int thickness, string borderColor, object htmlAttributes)
        {
            return CreateSvgPolygon(name, notSupportedMessage, points, thickness, borderColor, htmlAttributes, DrawMode.Stroke);
        }

        public static MvcHtmlString FilledPolygon(this HtmlHelper htmlHelper, string name, string notSupportedMessage, int[] points, string fillColor, object htmlAttributes)
        {
            return CreateSvgPolygon(name, notSupportedMessage, points, 0, fillColor, htmlAttributes, DrawMode.Fill);
        }

        public static MvcHtmlString Line(this HtmlHelper htmlHelper, string name, string notSupportedMessage, int x1, int y1, int x2, int y2, int thickness, string lineColor, object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("svg");
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

        private static MvcHtmlString CreateSvgRect(string name, string notSupportedMessage, int xPosition, int yPosition, int rectWidth, int rectHeight, int thickness, string color, object htmlAttributes, DrawMode drawMode)
        {
            TagBuilder tagBuilder = new TagBuilder("svg");
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

        private static MvcHtmlString CreateSvgEllipse(string name, string notSupportedMessage, int centerX, int centerY, int width, int height, int thickness, string color, object htmlAttributes, DrawMode drawMode)
        {
            TagBuilder tagBuilder = new TagBuilder("svg");
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

        private static MvcHtmlString CreateSvgCircle(string name, string notSupportedMessage, int centerX, int centerY, float radius, int thickness, string color, object htmlAttributes, DrawMode drawMode)
        {
            TagBuilder tagBuilder = new TagBuilder("svg");
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

        private static MvcHtmlString CreateSvgArc(string name, string notSupportedMessage, int centerX, int centerY, float radius, int thickness, float startAngle, float endAngle, string color, object htmlAttributes, DrawMode drawMode)
        {
            TagBuilder tagBuilder = new TagBuilder("svg");
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

        private static MvcHtmlString CreateSvgBeizerCurve(string name, string notSupportedMessage, int startX, int startY, int endX, int endY, float radius, int thickness, string color, object htmlAttributes, DrawMode drawMode)
        {
            TagBuilder tagBuilder = new TagBuilder("svg");
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

        private static MvcHtmlString CreateSvgQuadraticCurve(string name, string notSupportedMessage, int startX, int startY, int endX, int endY, int thickness, string color, object htmlAttributes, DrawMode drawMode)
        {
            TagBuilder tagBuilder = new TagBuilder("svg");
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

        private static MvcHtmlString CreateSvgPolygon(string name, string notSupportedMessage, int[] points, int thickness, string color, object htmlAttributes, DrawMode drawMode)
        {
            TagBuilder tagBuilder = new TagBuilder("svg");
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
            for (int item = 0; item < points.Length; item++)
            {
                pointsStr += points[item] + ",";
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
    }
}