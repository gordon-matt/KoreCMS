using System;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks
{
    public interface IContentBlock
    {
        Guid Id { get; set; }

        string Name { get; }

        string Title { get; set; }

        bool ShowTitleOnPage { get; set; }

        bool HasTitle { get; }

        Guid ZoneId { get; set; }

        Guid? PageId { get; set; }

        int Order { get; set; }

        bool Enabled { get; set; }

        bool Localized { get; set; }

        string DisplayCondition { get; set; }

        string CssClass { get; set; }

        bool IsMoveable { get; set; }

        string CultureCode { get; set; }

        Guid? RefId { get; set; }

        string DisplayTemplatePath { get; }

        string EditorTemplatePath { get; }
    }
}