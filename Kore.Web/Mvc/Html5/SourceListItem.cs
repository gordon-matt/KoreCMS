/*
 * Many thanks to Saleth Prakash: http://www.codeproject.com/Articles/237411/Html-Controls-for-ASP-Net-MVC
*/

namespace Kore.Web.Mvc
{
    /// <summary>
    /// Represents the source item in an instance of the System.Web.Mvc.SourceListItem class.
    /// </summary>
    public class SourceListItem
    {
        /// <summary>
        /// Gets or sets a value that indicates url of the System.Web.Mvc.SourceListItem.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates url type of the System.Web.Mvc.SourceListItem.
        /// </summary>
        public string SourceType { get; set; }
    }
}