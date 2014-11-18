namespace Kore.Web.ContentManagement.Areas.Admin.Media.Models
{
    public interface IMediaPart
    {
        string Url { get; set; }

        string Caption { get; set; }

        int SortOrder { get; set; }
    }
}