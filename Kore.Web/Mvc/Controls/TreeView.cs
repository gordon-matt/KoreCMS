// Original From: https://github.com/NuGet/NuGetGallery/blob/master/src/NuGetGallery/Helpers/TreeView.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Kore.Collections.Generic;

namespace Kore.Web.Mvc.Controls
{
    /// <summary>
    /// Create an HTML tree from a resursive collection of items
    /// </summary>
    public class TreeView<T> : IHtmlString
    {
        private readonly IEnumerable<IRelationship<T>> items;
        private Func<IRelationship<T>, string> displayProperty = item => item.ToString();
        private string emptyContent = "No children";
        private IDictionary<string, object> htmlAttributes = new Dictionary<string, object>();
        private IDictionary<string, object> childHtmlAttributes = new Dictionary<string, object>();
        private IDictionary<string, object> itemWithChildrenHtmlAttributes = new Dictionary<string, object>();
        private Func<IRelationship<T>, HelperResult> itemTemplate;

        public TreeView(IEnumerable<IRelationship<T>> items)
        {
            this.items = items;

            // The ItemTemplate will default to rendering the DisplayProperty
            itemTemplate = item => new HelperResult(writer => writer.Write(displayProperty(item)));
        }

        /// <summary>
        /// The property which will display the text rendered for each item
        /// </summary>
        public TreeView<T> ItemText(Func<IRelationship<T>, string> selector)
        {
            if (selector == null) throw new ArgumentNullException("selector");
            displayProperty = selector;
            return this;
        }

        /// <summary>
        /// The template used to render each item in the tree view
        /// </summary>
        public TreeView<T> ItemTemplate(Func<IRelationship<T>, HelperResult> template)
        {
            if (template == null) throw new ArgumentNullException("template");
            itemTemplate = template;
            return this;
        }

        /// <summary>
        /// Content displayed if the list is empty
        /// </summary>
        public TreeView<T> EmptyContent(string content)
        {
            if (content == null) throw new ArgumentNullException("content");
            emptyContent = content;
            return this;
        }

        /// <summary>
        /// HTML attributes appended to the root ul node
        /// </summary>
        public TreeView<T> HtmlAttributes(object attributes)
        {
            HtmlAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(attributes));
            return this;
        }

        /// <summary>
        /// HTML attributes appended to the root ul node
        /// </summary>
        public TreeView<T> HtmlAttributes(IDictionary<string, object> attributes)
        {
            if (attributes == null) throw new ArgumentNullException("attributes");
            htmlAttributes = attributes;
            return this;
        }

        /// <summary>
        /// HTML attributes appended to the children items
        /// </summary>
        public TreeView<T> ChildHtmlAttributes(object attributes)
        {
            ChildHtmlAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(attributes));
            return this;
        }

        /// <summary>
        /// HTML attributes appended to the children items
        /// </summary>
        public TreeView<T> ChildHtmlAttributes(IDictionary<string, object> attributes)
        {
            if (attributes == null) throw new ArgumentNullException("attributes");
            childHtmlAttributes = attributes;
            return this;
        }

        /// <summary>
        /// HTML attributes appended to the item with children items
        /// </summary>
        public TreeView<T> ItemWithChildrenHtmlAttributes(object attributes)
        {
            ItemWithChildrenHtmlAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(attributes));
            return this;
        }

        /// <summary>
        /// HTML attributes appended to the item with children items
        /// </summary>
        public TreeView<T> ItemWithChildrenHtmlAttributes(IDictionary<string, object> attributes)
        {
            if (attributes == null) throw new ArgumentNullException("attributes");
            itemWithChildrenHtmlAttributes = attributes;
            return this;
        }

        public string ToHtmlString()
        {
            return ToString();
        }

        public override string ToString()
        {
            var listItems = items.ToList();

            var ul = new TagBuilder("ul");
            ul.MergeAttributes(htmlAttributes);

            if (listItems.Count == 0)
            {
                var li = new TagBuilder("li")
                {
                    InnerHtml = emptyContent
                };
                ul.InnerHtml += li.ToString();
            }

            foreach (var item in listItems)
            {
                BuildNestedTag(ul, item);
            }

            return ul.ToString();
        }

        private void AppendChildren(TagBuilder parentTag, IRelationship<T> parentItem)
        {
            if (!parentItem.Any())
            {
                return;
            }

            var innerUl = new TagBuilder("ul");
            innerUl.MergeAttributes(childHtmlAttributes);

            foreach (var item in parentItem)
            {
                BuildNestedTag(innerUl, item);
            }

            parentTag.InnerHtml += innerUl.ToString();
        }

        private void BuildNestedTag(TagBuilder parentTag, IRelationship<T> parentItem)
        {
            var li = GetLi(parentItem);
            parentTag.InnerHtml += li.ToString(TagRenderMode.StartTag);
            AppendChildren(li, parentItem);
            parentTag.InnerHtml += li.InnerHtml + li.ToString(TagRenderMode.EndTag);
        }

        private TagBuilder GetLi(IRelationship<T> item)
        {
            var li = new TagBuilder("li")
            {
                InnerHtml = itemTemplate(item).ToHtmlString()
            };

            if (item.Any())
            {
                li.MergeAttributes(itemWithChildrenHtmlAttributes);
            }

            return li;
        }
    }
}