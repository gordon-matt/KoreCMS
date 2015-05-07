namespace Kore.Web.ContentManagement.Areas.Admin.Media.Models
{
    public interface IKoreImage
    {
        string Url { get; set; }

        string ThumbnailUrl { get; set; }

        string Caption { get; set; }

        int SortOrder { get; set; }
    }
}