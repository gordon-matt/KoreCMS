using System;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks
{
    public interface IContentBlock
    {
        Guid Id { get; set; }

        string Title { get; set; }

        int Order { get; set; }

        bool Enabled { get; set; }

        string Name { get; }

        Guid ZoneId { get; set; }

        string DisplayCondition { get; set; }

        Guid? PageId { get; set; }

        bool Localized { get; set; }

        string CultureCode { get; set; }

        Guid? RefId { get; set; }

        string DisplayTemplatePath { get; }

        string EditorTemplatePath { get; }
    }
}