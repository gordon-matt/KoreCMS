using System;
using Kore.ComponentModel;
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
        public string Title { get; set; }

        [ExcludeFromSerialization]
        public int Order { get; set; }

        [ExcludeFromSerialization]
        public bool Enabled { get; set; }

        [ExcludeFromSerialization]
        public abstract string Name { get; }

        [LocalizedDisplayName(KoreCmsLocalizableStrings.ContentBlocks.Model.ZoneId)]
        [ExcludeFromSerialization]
        public Guid ZoneId { get; set; }

        [LocalizedDisplayName(KoreCmsLocalizableStrings.ContentBlocks.Model.DisplayCondition)]
        [ExcludeFromSerialization]
        public string DisplayCondition { get; set; }

        [LocalizedDisplayName(KoreCmsLocalizableStrings.ContentBlocks.Model.CustomTemplatePath)]
        [ExcludeFromSerialization]
        public string CustomTemplatePath { get; set; }

        [ExcludeFromSerialization]
        public Guid? PageId { get; set; }

        public bool Localized { get; set; }

        [ExcludeFromSerialization]
        public string CultureCode { get; set; }

        [ExcludeFromSerialization]
        public Guid? RefId { get; set; }

        public abstract string DisplayTemplatePath { get; }

        public abstract string EditorTemplatePath { get; }

        #endregion IContentBlock Members

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }
}