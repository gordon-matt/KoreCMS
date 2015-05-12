using System;
using System.ComponentModel.DataAnnotations;
using Kore.Data;
using Kore.Serialization;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks
{
    public abstract class ContentBlockBase : IEntity, IContentBlock
    {
        #region IContentBlock Members

        [ExcludeFromSerialization]
        public Guid Id { get; set; }

        [ExcludeFromSerialization]
        public abstract string Name { get; }

        public bool Localized { get; set; }

        [ExcludeFromSerialization]
        public string Title { get; set; }

        [Display(Name = "Show Title On Page")]
        public bool ShowTitleOnPage { get; set; }

        public virtual bool HasTitle { get { return true; } }

        [Display(Name = "Display Condition")]
        [ExcludeFromSerialization]
        public string DisplayCondition { get; set; }

        [ExcludeFromSerialization]
        public bool Enabled { get; set; }

        [Display(Name = "Zone")]
        [ExcludeFromSerialization]
        public Guid ZoneId { get; set; }

        [ExcludeFromSerialization]
        public Guid? PageId { get; set; }

        [ExcludeFromSerialization]
        public int Order { get; set; }

        [ExcludeFromSerialization]
        public bool IsMoveable { get; set; }

        [ExcludeFromSerialization]
        public string CultureCode { get; set; }

        [ExcludeFromSerialization]
        public Guid? RefId { get; set; }

        public abstract string DisplayTemplatePath { get; }

        public abstract string EditorTemplatePath { get; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members

        #endregion IContentBlock Members
    }
}