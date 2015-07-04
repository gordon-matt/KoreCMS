/*
 * Many thanks to Saleth Prakash: http://www.codeproject.com/Articles/237411/Html-Controls-for-ASP-Net-MVC
*/

using System.Web.Mvc;

namespace Kore.Web.Mvc.Html
{
    /// <summary>
    /// Represents support for HTML5 controls in an application.
    /// </summary>
    public static class Html5Extensions
    {
        /// <summary>
        /// Returns a Html5 element by using the specified HTML helper.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <returns>An Html5 element.</returns>
        public static Html5Helper Html5(this HtmlHelper htmlHelper)
        {
            return new Html5Helper(htmlHelper);
        }
    }
}