/*
 * Many thanks to Saleth Prakash: http://www.codeproject.com/Articles/237411/Html-Controls-for-ASP-Net-MVC
*/

namespace Kore.Web.Mvc.Html
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// A Html5 helper interface which contains all the media related elements.
    /// </summary>
    public interface IHtml5MediaHelper
    {
        /// <summary>
        /// Renders a empty audio element in the client browser.
        /// </summary>
        /// <param name="name">The name for the audio, but this renders as ID for the audio element.</param>
        /// <param name="source">The source to be played in the audio element.</param>
        /// <param name="notSupportedMessage">The message appears when the audio tag is not supported by the client browser.</param>
        /// <returns>A audio element.</returns>
        MvcHtmlString Audio(string name, string source, string notSupportedMessage);

        /// <summary>
        /// Renders a empty audio element in the client browser.
        /// </summary>
        /// <param name="name">The name for the audio, but this renders as ID for the audio element.</param>
        /// <param name="source">The source to be played in the audio element.</param>
        /// <param name="notSupportedMessage">The message appears when the audio tag is not supported by the client browser.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the audio element.</param>
        /// <returns>A audio element.</returns>
        MvcHtmlString Audio(string name, string source, string notSupportedMessage, object htmlAttributes);

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
        MvcHtmlString Audio(string name, string source, string notSupportedMessage, bool showControls, bool autoPlay, bool playInLoop);

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
        MvcHtmlString Audio(string name, string source, string notSupportedMessage, bool showControls, bool autoPlay, bool playInLoop, object htmlAttributes);

        /// <summary>
        /// Renders a empty audio element in the client browser.
        /// </summary>
        /// <param name="name">The name for the audio, but this renders as ID for the audio element.</param>
        /// <param name="sourceList">The sources list to be played in the audio element.</param>
        /// <param name="notSupportedMessage">The message appears when the audio tag is not supported by the client browser.</param>
        /// <returns>A audio element.</returns>
        MvcHtmlString Audio(string name, IEnumerable<SourceListItem> sourceList, string notSupportedMessage);

        /// <summary>
        /// Renders a empty audio element in the client browser.
        /// </summary>
        /// <param name="name">The name for the audio, but this renders as ID for the audio element.</param>
        /// <param name="sourceList">The sources list to be played in the audio element.</param>
        /// <param name="notSupportedMessage">The message appears when the audio tag is not supported by the client browser.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the audio element.</param>
        /// <returns>A audio element.</returns>
        MvcHtmlString Audio(string name, IEnumerable<SourceListItem> sourceList, string notSupportedMessage, object htmlAttributes);

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
        MvcHtmlString Audio(string name, IEnumerable<SourceListItem> sourceList, string notSupportedMessage, bool showControls, bool autoPlay, bool playInLoop);

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
        MvcHtmlString Audio(string name, IEnumerable<SourceListItem> sourceList, string notSupportedMessage, bool showControls, bool autoPlay, bool playInLoop, object htmlAttributes);

        /// <summary>
        /// Renders a empty video element in the client browser.
        /// </summary>
        /// <param name="name">The name for the video, but this renders as ID for the video element.</param>
        /// <param name="source">The source to be played in the video element.</param>
        /// <param name="notSupportedMessage">The message appears when the video tag is not supported by the client browser.</param>
        /// <returns>A video element.</returns>
        MvcHtmlString Video(string name, string source, string notSupportedMessage);

        /// <summary>
        /// Renders a empty video element in the client browser.
        /// </summary>
        /// <param name="name">The name for the video, but this renders as ID for the video element.</param>
        /// <param name="source">The source to be played in the video element.</param>
        /// <param name="notSupportedMessage">The message appears when the video tag is not supported by the client browser.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the video element.</param>
        /// <returns>A video element.</returns>
        MvcHtmlString Video(string name, string source, string notSupportedMessage, object htmlAttributes);

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
        MvcHtmlString Video(string name, string source, string notSupportedMessage, bool showControls, bool autoPlay, bool playInLoop);

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
        MvcHtmlString Video(string name, string source, string notSupportedMessage, bool showControls, bool autoPlay, bool playInLoop, object htmlAttributes);

        /// <summary>
        /// Renders a empty video element in the client browser.
        /// </summary>
        /// <param name="name">The name for the video, but this renders as ID for the video element.</param>
        /// <param name="sourceList">The sources list to be played in the video element.</param>
        /// <param name="notSupportedMessage">The message appears when the video tag is not supported by the client browser.</param>
        /// <returns>A video element.</returns>
        MvcHtmlString Video(string name, IEnumerable<SourceListItem> sourceList, string notSupportedMessage);

        /// <summary>
        /// Renders a empty video element in the client browser.
        /// </summary>
        /// <param name="name">The name for the video, but this renders as ID for the video element.</param>
        /// <param name="sourceList">The sources list to be played in the video element.</param>
        /// <param name="notSupportedMessage">The message appears when the video tag is not supported by the client browser.</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the video element.</param>
        /// <returns>A video element.</returns>
        MvcHtmlString Video(string name, IEnumerable<SourceListItem> sourceList, string notSupportedMessage, object htmlAttributes);

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
        MvcHtmlString Video(string name, IEnumerable<SourceListItem> sourceList, string notSupportedMessage, bool showControls, bool autoPlay, bool playInLoop);

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
        MvcHtmlString Video(string name, IEnumerable<SourceListItem> sourceList, string notSupportedMessage, bool showControls, bool autoPlay, bool playInLoop, object htmlAttributes);

        /// <summary>
        /// Renders a empty video element in the client browser.
        /// </summary>
        /// <param name="name">The name for the video, but this renders as ID for the video element.</param>
        /// <param name="sourceList">The sources list to be played in the video element.</param>
        /// <param name="objectType">The object type (flash, silverlight) to be rendered for fallback. </param>
        /// <param name="objectSource">The source to be played in the object element.(.swf for flash, .xap for silverlight).</param>
        /// <returns>A video element containing a inner object element.</returns>
        MvcHtmlString Video(string name, IEnumerable<SourceListItem> sourceList, ObjectType objectType, string objectSource);

        /// <summary>
        /// Renders a empty video element in the client browser.
        /// </summary>
        /// <param name="name">The name for the video, but this renders as ID for the video element.</param>
        /// <param name="sourceList">The sources list to be played in the video element.</param>
        /// <param name="objectType">The object type (flash, silverlight) to be rendered for fallback. </param>
        /// <param name="objectSource">The source to be played in the object element.(.swf for flash, .xap for silverlight).</param>
        /// <param name="htmlAttributes">The html attributes to be rendered with the video element.</param>
        /// <returns>A video element containing a inner object element.</returns>
        MvcHtmlString Video(string name, IEnumerable<SourceListItem> sourceList, ObjectType objectType, string objectSource, object htmlAttributes);
    }
}