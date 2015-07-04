/*
 * Many thanks to Saleth Prakash: http://www.codeproject.com/Articles/237411/Html-Controls-for-ASP-Net-MVC
*/

namespace Kore.Web.Mvc
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a list that contains all the source items.
    /// </summary>
    public class SourceList : IEnumerable<SourceListItem>
    {
        /// <summary>
        /// The list of source items to be rendered for the audio or video elements.
        /// </summary>
        public IEnumerable<SourceListItem> Items { get; private set; }

        /// <summary>
        /// Initializes a new instance of the System.Web.Mvc.SourceList class by using specified source items for the list.
        /// </summary>
        public SourceList(IEnumerable<SourceListItem> items)
        {
            this.Items = items;
        }

        /// <summary>
        /// Defines the GetEnumerator()
        /// </summary>
        public virtual IEnumerator<SourceListItem> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        /// <summary>
        /// Defines the IEnumerable.GetEnumerator()
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}