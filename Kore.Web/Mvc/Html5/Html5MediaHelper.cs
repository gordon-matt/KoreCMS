/*
 * Many thanks to Saleth Prakash: http://www.codeproject.com/Articles/237411/Html-Controls-for-ASP-Net-MVC
*/

namespace Kore.Web.Mvc.Html
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// A Html5 helper class which contains all the media related elements.
    /// </summary>
    public class Html5MediaHelper : IHtml5MediaHelper
    {
        private HtmlHelper htmlHelper;

        /// <summary>
        /// Initializes the Html5 helper class of media related elements.
        /// </summary>
        /// <param name="htmlHelper">The html helper class instance that comes from the current view context.</param>
        public Html5MediaHelper(HtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        /// <summary>
        /// Renders a empty audio element in the client browser.
        /// </summary>
        /// <param name="name">The name for the audio, but this renders as ID for the audio element.</param>
        /// <param name="source">The source to be played in the audio element.</param>
        /// <param name="notSupportedMessage">The message appears when the audio tag is not supported by the client browser.</param>
        /// <returns>A audio element.</returns>
        public MvcHtmlString Audio(string name, string source, string notSupportedMessage)
        {
            return Audio(name, source, notSupportedMessage, true, false, false, null);
        }

        /// <summary>
        /// Renders a empty audio element in the client browser.
        /// </summary>
        /// <param name="name">The name for the audio, but this renders as ID for the audio element.</param>
        /// <param name="source">The source to be played in the audio element.</param>
        /// <param name="notSupportedMessage">The message appears when the audio tag is not supported by the client browser.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the audio element.</param>
        /// <returns>A audio element.</returns>
        public MvcHtmlString Audio(string name, string source, string notSupportedMessage, object htmlAttributes)
        {
            return Audio(name, source, notSupportedMessage, true, false, false, htmlAttributes);
        }

        /// <summary>
        /// Renders a empty audio element in the client browser.
        /// </summary>
        /// <param name="name">The name for the audio, but this renders as ID for the audio element.</param>
        /// <param name="source">The source to be played in the audio element.</param>
        /// <param name="notSupportedMessage">The message appears when the audio tag is not supported by the client browser.</param>
        /// <param name="showControls">Whether the controls to be displayed or not.</param>
        /// <param name="autoPlay">Whether the audio plays automatically or not.</param>
        /// <param name="playInLoop">Whether the audio play in loop or not.</param>
        /// <returns>A audio element.</returns>
        public MvcHtmlString Audio(string name, string source, string notSupportedMessage, bool showControls, bool autoPlay, bool playInLoop)
        {
            return Audio(name, source, notSupportedMessage, showControls, autoPlay, playInLoop, null);
        }

        /// <summary>
        /// Renders a empty audio element in the client browser.
        /// </summary>
        /// <param name="name">The name for the audio, but this renders as ID for the audio element.</param>
        /// <param name="source">The source to be played in the audio element.</param>
        /// <param name="notSupportedMessage">The message appears when the audio tag is not supported by the client browser.</param>
        /// <param name="showControls">Whether the controls to be displayed or not.</param>
        /// <param name="autoPlay">Whether the audio plays automatically or not.</param>
        /// <param name="playInLoop">Whether the audio play in loop or not.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the audio element.</param>
        /// <returns>A audio element.</returns>
        public MvcHtmlString Audio(string name, string source, string notSupportedMessage, bool showControls, bool autoPlay, bool playInLoop, object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("audio");
            string url = UrlHelper.GenerateContentUrl(source, htmlHelper.ViewContext.HttpContext);
            if (htmlAttributes != null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary(htmlAttributes);
                tagBuilder.MergeAttributes(routeValueDictionary);
            }
            if (showControls)
                tagBuilder.MergeAttribute("controls", "controls");
            if (autoPlay)
                tagBuilder.MergeAttribute("autoplay", "autoplay");
            if (playInLoop)
                tagBuilder.MergeAttribute("loop", "loop");
            tagBuilder.MergeAttribute("src", url);
            tagBuilder.MergeAttribute("id", name);
            tagBuilder.InnerHtml = notSupportedMessage;
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Renders a empty audio element in the client browser.
        /// </summary>
        /// <param name="name">The name for the audio, but this renders as ID for the audio element.</param>
        /// <param name="sourceList">The sources list to be played in the audio element.</param>
        /// <param name="notSupportedMessage">The message appears when the audio tag is not supported by the client browser.</param>
        /// <returns>A audio element.</returns>
        public MvcHtmlString Audio(string name, IEnumerable<SourceListItem> sourceList, string notSupportedMessage)
        {
            return Audio(name, sourceList, notSupportedMessage, true, false, false, null);
        }

        /// <summary>
        /// Renders a empty audio element in the client browser.
        /// </summary>
        /// <param name="name">The name for the audio, but this renders as ID for the audio element.</param>
        /// <param name="sourceList">The sources list to be played in the audio element.</param>
        /// <param name="notSupportedMessage">The message appears when the audio tag is not supported by the client browser.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the audio element.</param>
        /// <returns>A audio element.</returns>
        public MvcHtmlString Audio(string name, IEnumerable<SourceListItem> sourceList, string notSupportedMessage, object htmlAttributes)
        {
            return Audio(name, sourceList, notSupportedMessage, true, false, false, htmlAttributes);
        }

        /// <summary>
        /// Renders a empty audio element in the client browser.
        /// </summary>
        /// <param name="name">The name for the audio, but this renders as ID for the audio element.</param>
        /// <param name="sourceList">The sources list to be played in the audio element.</param>
        /// <param name="notSupportedMessage">The message appears when the audio tag is not supported by the client browser.</param>
        /// <param name="showControls">Whether the controls to be displayed or not.</param>
        /// <param name="autoPlay">Whether the audio plays automatically or not.</param>
        /// <param name="playInLoop">Whether the audio play in loop or not.</param>
        /// <returns>A audio element.</returns>
        public MvcHtmlString Audio(string name, IEnumerable<SourceListItem> sourceList, string notSupportedMessage, bool showControls, bool autoPlay, bool playInLoop)
        {
            return Audio(name, sourceList, notSupportedMessage, showControls, autoPlay, playInLoop, null);
        }

        /// <summary>
        /// Renders a empty audio element in the client browser.
        /// </summary>
        /// <param name="name">The name for the audio, but this renders as ID for the audio element.</param>
        /// <param name="sourceList">The sources list to be played in the audio element.</param>
        /// <param name="notSupportedMessage">The message appears when the audio tag is not supported by the client browser.</param>
        /// <param name="showControls">Whether the controls to be displayed or not.</param>
        /// <param name="autoPlay">Whether the audio plays automatically or not.</param>
        /// <param name="playInLoop">Whether the audio play in loop or not.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the audio element.</param>
        /// <returns>A audio element.</returns>
        public MvcHtmlString Audio(string name, IEnumerable<SourceListItem> sourceList, string notSupportedMessage, bool showControls, bool autoPlay, bool playInLoop, object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("audio");
            if (htmlAttributes != null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary(htmlAttributes);
                tagBuilder.MergeAttributes(routeValueDictionary);
            }
            if (showControls)
                tagBuilder.MergeAttribute("controls", "controls");
            if (autoPlay)
                tagBuilder.MergeAttribute("autoplay", "autoplay");
            if (playInLoop)
                tagBuilder.MergeAttribute("loop", "loop");
            tagBuilder.MergeAttribute("id", name);
            StringBuilder sourceItemBuilder = new StringBuilder();
            sourceItemBuilder.AppendLine();
            foreach (var sourceItem in sourceList)
            {
                sourceItemBuilder.AppendLine(SourceItemToSource(sourceItem));
            }
            sourceItemBuilder.AppendLine(notSupportedMessage);
            tagBuilder.InnerHtml = sourceItemBuilder.ToString();
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Renders a empty video element in the client browser.
        /// </summary>
        /// <param name="name">The name for the video, but this renders as ID for the video element.</param>
        /// <param name="source">The source to be played in the video element.</param>
        /// <param name="notSupportedMessage">The message appears when the video tag is not supported by the client browser.</param>
        /// <returns>A video element.</returns>
        public MvcHtmlString Video(string name, string source, string notSupportedMessage)
        {
            return Video(name, source, notSupportedMessage, true, false, false, null);
        }

        /// <summary>
        /// Renders a empty video element in the client browser.
        /// </summary>
        /// <param name="name">The name for the video, but this renders as ID for the video element.</param>
        /// <param name="source">The source to be played in the video element.</param>
        /// <param name="notSupportedMessage">The message appears when the video tag is not supported by the client browser.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the video element.</param>
        /// <returns>A video element.</returns>
        public MvcHtmlString Video(string name, string source, string notSupportedMessage, object htmlAttributes)
        {
            return Video(name, source, notSupportedMessage, true, false, false, htmlAttributes);
        }

        /// <summary>
        /// Renders a empty video element in the client browser.
        /// </summary>
        /// <param name="name">The name for the video, but this renders as ID for the video element.</param>
        /// <param name="source">The source to be played in the video element.</param>
        /// <param name="notSupportedMessage">The message appears when the video tag is not supported by the client browser.</param>
        /// <param name="showControls">Whether the controls to be displayed or not.</param>
        /// <param name="autoPlay">Whether the video plays automatically or not.</param>
        /// <param name="playInLoop">Whether the video play in loop or not.</param>
        /// <returns>A video element.</returns>
        public MvcHtmlString Video(string name, string source, string notSupportedMessage, bool showControls, bool autoPlay, bool playInLoop)
        {
            return Video(name, source, notSupportedMessage, showControls, autoPlay, playInLoop, null);
        }

        /// <summary>
        /// Renders a empty video element in the client browser.
        /// </summary>
        /// <param name="name">The name for the video, but this renders as ID for the video element.</param>
        /// <param name="source">The source to be played in the video element.</param>
        /// <param name="notSupportedMessage">The message appears when the video tag is not supported by the client browser.</param>
        /// <param name="showControls">Whether the controls to be displayed or not.</param>
        /// <param name="autoPlay">Whether the video plays automatically or not.</param>
        /// <param name="playInLoop">Whether the video play in loop or not.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the video element.</param>
        /// <returns>A video element.</returns>
        public MvcHtmlString Video(string name, string source, string notSupportedMessage, bool showControls, bool autoPlay, bool playInLoop, object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("video");
            string url = UrlHelper.GenerateContentUrl(source, htmlHelper.ViewContext.HttpContext);
            if (htmlAttributes != null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary(htmlAttributes);
                tagBuilder.MergeAttributes(routeValueDictionary);
            }
            if (showControls)
                tagBuilder.MergeAttribute("controls", "controls");
            if (autoPlay)
                tagBuilder.MergeAttribute("autoplay", "autoplay");
            if (playInLoop)
                tagBuilder.MergeAttribute("loop", "loop");
            tagBuilder.MergeAttribute("src", url);
            tagBuilder.MergeAttribute("id", name);
            tagBuilder.InnerHtml = notSupportedMessage;
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Renders a empty video element in the client browser.
        /// </summary>
        /// <param name="name">The name for the video, but this renders as ID for the video element.</param>
        /// <param name="sourceList">The sources list to be played in the video element.</param>
        /// <param name="notSupportedMessage">The message appears when the video tag is not supported by the client browser.</param>
        /// <returns>A video element.</returns>
        public MvcHtmlString Video(string name, IEnumerable<SourceListItem> sourceList, string notSupportedMessage)
        {
            return Video(name, sourceList, notSupportedMessage, true, false, false, null);
        }

        /// <summary>
        /// Renders a empty video element in the client browser.
        /// </summary>
        /// <param name="name">The name for the video, but this renders as ID for the video element.</param>
        /// <param name="sourceList">The sources list to be played in the video element.</param>
        /// <param name="notSupportedMessage">The message appears when the video tag is not supported by the client browser.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the video element.</param>
        /// <returns>A video element.</returns>
        public MvcHtmlString Video(string name, IEnumerable<SourceListItem> sourceList, string notSupportedMessage, object htmlAttributes)
        {
            return Video(name, sourceList, notSupportedMessage, true, false, false, htmlAttributes);
        }

        /// <summary>
        /// Renders a empty video element in the client browser.
        /// </summary>
        /// <param name="name">The name for the video, but this renders as ID for the video element.</param>
        /// <param name="sourceList">The sources list to be played in the video element.</param>
        /// <param name="notSupportedMessage">The message appears when the video tag is not supported by the client browser.</param>
        /// <param name="showControls">Whether the controls to be displayed or not.</param>
        /// <param name="autoPlay">Whether the video plays automatically or not.</param>
        /// <param name="playInLoop">Whether the video play in loop or not.</param>
        /// <returns>A video element.</returns>
        public MvcHtmlString Video(string name, IEnumerable<SourceListItem> sourceList, string notSupportedMessage, bool showControls, bool autoPlay, bool playInLoop)
        {
            return Video(name, sourceList, notSupportedMessage, showControls, autoPlay, playInLoop, null);
        }

        /// <summary>
        /// Renders a empty video element in the client browser.
        /// </summary>
        /// <param name="name">The name for the video, but this renders as ID for the video element.</param>
        /// <param name="sourceList">The sources list to be played in the video element.</param>
        /// <param name="notSupportedMessage">The message appears when the video tag is not supported by the client browser.</param>
        /// <param name="showControls">Whether the controls to be displayed or not.</param>
        /// <param name="autoPlay">Whether the video plays automatically or not.</param>
        /// <param name="playInLoop">Whether the video play in loop or not.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the video element.</param>
        /// <returns>A video element.</returns>
        public MvcHtmlString Video(string name, IEnumerable<SourceListItem> sourceList, string notSupportedMessage, bool showControls, bool autoPlay, bool playInLoop, object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("video");
            if (htmlAttributes != null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary(htmlAttributes);
                tagBuilder.MergeAttributes(routeValueDictionary);
            }
            if (showControls)
                tagBuilder.MergeAttribute("controls", "controls");
            if (autoPlay)
                tagBuilder.MergeAttribute("autoplay", "autoplay");
            if (playInLoop)
                tagBuilder.MergeAttribute("loop", "loop");
            tagBuilder.MergeAttribute("id", name);
            StringBuilder sourceItemBuilder = new StringBuilder();
            sourceItemBuilder.AppendLine();
            foreach (var sourceItem in sourceList)
            {
                sourceItemBuilder.AppendLine(SourceItemToSource(sourceItem));
            }
            sourceItemBuilder.AppendLine(notSupportedMessage);
            tagBuilder.InnerHtml = sourceItemBuilder.ToString();
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Renders a empty video element in the client browser.
        /// </summary>
        /// <param name="name">The name for the video, but this renders as ID for the video element.</param>
        /// <param name="sourceList">The sources list to be played in the video element.</param>
        /// <param name="objectType">The object type (flash, silverlight) to be rendered for fallback. </param>
        /// <param name="objectSource">The source to be played in the object element.(.swf for flash, .xap for silverlight).</param>
        /// <returns>A video element containing a inner object element.</returns>
        public MvcHtmlString Video(string name, IEnumerable<SourceListItem> sourceList, ObjectType objectType, string objectSource)
        {
            return Video(name, sourceList, objectType, objectSource, null);
        }

        /// <summary>
        /// Renders a empty video element in the client browser.
        /// </summary>
        /// <param name="name">The name for the video, but this renders as ID for the video element.</param>
        /// <param name="sourceList">The sources list to be played in the video element.</param>
        /// <param name="objectType">The object type (flash, silverlight) to be rendered for fallback. </param>
        /// <param name="objectSource">The source to be played in the object element.(.swf for flash, .xap for silverlight).</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the video element.</param>
        /// <returns>A video element containing a inner object element.</returns>
        public MvcHtmlString Video(string name, IEnumerable<SourceListItem> sourceList, ObjectType objectType, string objectSource, object htmlAttributes)
        {
            TagBuilder tagBuilder = new TagBuilder("video");
            if (htmlAttributes != null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary(htmlAttributes);
                tagBuilder.MergeAttributes(routeValueDictionary);
            }
            tagBuilder.MergeAttribute("id", name);
            StringBuilder sourceItemBuilder = new StringBuilder();
            sourceItemBuilder.AppendLine();
            foreach (var sourceItem in sourceList)
            {
                sourceItemBuilder.AppendLine(SourceItemToSource(sourceItem));
            }
            sourceItemBuilder.AppendLine();
            if (objectType == ObjectType.Flash)
            {
                sourceItemBuilder.AppendLine(CreateFlashObject(objectSource, htmlAttributes));
            }
            else
            {
                sourceItemBuilder.AppendLine(CreateSilverlightObject(sourceList, objectSource, htmlAttributes));
            }
            tagBuilder.InnerHtml = sourceItemBuilder.ToString();
            sourceItemBuilder.AppendLine();
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Creates a object element for flash source.
        /// </summary>
        private string CreateFlashObject(string flashSource, object htmlAttributes)
        {
            TagBuilder objectBuilder = new TagBuilder("object");
            if (htmlAttributes != null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary(htmlAttributes);
                objectBuilder.MergeAttributes(routeValueDictionary);
            }
            string flashUrl = UrlHelper.GenerateContentUrl(flashSource, htmlHelper.ViewContext.HttpContext);
            TagBuilder paramBuilder = new TagBuilder("param");
            paramBuilder.MergeAttribute("name", "movie");
            paramBuilder.MergeAttribute("value", flashUrl);
            string paramTag = paramBuilder.ToString(TagRenderMode.SelfClosing);

            TagBuilder embedBuilder = new TagBuilder("embed");
            if (htmlAttributes != null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary(htmlAttributes);
                embedBuilder.MergeAttributes(routeValueDictionary);
            }
            embedBuilder.MergeAttribute("src", flashUrl);
            string embedTag = embedBuilder.ToString(TagRenderMode.Normal);
            StringBuilder objectItemBuilder = new StringBuilder();
            objectItemBuilder.AppendLine();
            objectItemBuilder.AppendLine(paramTag);
            objectItemBuilder.AppendLine(embedTag);
            objectBuilder.InnerHtml = objectItemBuilder.ToString();
            return objectBuilder.ToString(TagRenderMode.Normal);
        }

        /// <summary>
        /// Creates a object element for silverlight source.
        /// </summary>
        private string CreateSilverlightObject(IEnumerable<SourceListItem> sourceList, string xapSource, object htmlAttributes)
        {
            TagBuilder objectBuilder = new TagBuilder("object");
            if (htmlAttributes != null)
            {
                RouteValueDictionary routeValueDictionary = new RouteValueDictionary(htmlAttributes);
                objectBuilder.MergeAttributes(routeValueDictionary);
            }
            objectBuilder.MergeAttribute("type", "application/x-silverlight-2");

            string xapUrl = UrlHelper.GenerateContentUrl(xapSource, htmlHelper.ViewContext.HttpContext);
            TagBuilder paramBuilder1 = new TagBuilder("param");
            paramBuilder1.MergeAttribute("name", "source");
            paramBuilder1.MergeAttribute("value", xapUrl);
            string paramTag1 = paramBuilder1.ToString(TagRenderMode.SelfClosing);

            TagBuilder paramBuilder2 = new TagBuilder("param");
            paramBuilder2.MergeAttribute("name", "initParams");
            string sourceUrl = sourceList.Single(s => s.Source.ToLower().Contains("mp4")).Source;
            sourceUrl = UrlHelper.GenerateContentUrl(sourceUrl, htmlHelper.ViewContext.HttpContext);
            string initParamsString = "deferredLoad=true, duration=0, m=" + sourceUrl + ", autostart=false, autohide=true, showembed=true, postid=0";
            paramBuilder2.MergeAttribute("value", initParamsString);
            string paramTag2 = paramBuilder2.ToString(TagRenderMode.SelfClosing);

            TagBuilder paramBuilder3 = new TagBuilder("param");
            paramBuilder3.MergeAttribute("name", "background");
            paramBuilder3.MergeAttribute("value", "#00FFFFFF");
            string paramTag3 = paramBuilder3.ToString(TagRenderMode.SelfClosing);

            StringBuilder objectItemBuilder = new StringBuilder();
            objectItemBuilder.AppendLine();
            objectItemBuilder.AppendLine(paramTag1);
            objectItemBuilder.AppendLine(paramTag2);
            objectItemBuilder.AppendLine(paramTag3);
            objectBuilder.InnerHtml = objectItemBuilder.ToString();
            return objectBuilder.ToString(TagRenderMode.Normal);
        }

        /// <summary>
        /// Converts the source items to a source.
        /// </summary>
        private string SourceItemToSource(SourceListItem sourceItem)
        {
            TagBuilder builder = new TagBuilder("source");
            builder.MergeAttribute("type", sourceItem.SourceType);
            builder.MergeAttribute("src", UrlHelper.GenerateContentUrl(sourceItem.Source, htmlHelper.ViewContext.HttpContext));
            return builder.ToString(TagRenderMode.SelfClosing);
        }
    }
}