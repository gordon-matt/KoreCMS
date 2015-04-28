using Kore.Web.Configuration;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog
{
    public class BlogSettings : ISettings
    {
        public BlogSettings()
        {
            DateFormat = "YYYY-MM-DD HH:mm:ss";
            ItemsPerPage = 5;
            PageTitle = "Blog";
        }

        public string PageTitle { get; set; }

        public string DateFormat { get; set; }

        public byte ItemsPerPage { get; set; }

        #region ISettings Members

        public string Name
        {
            get { return "CMS: Blog Settings"; }
        }

        public string EditorTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.Blog.Views.Shared.EditorTemplates.BlogSettings"; }
        }

        #endregion ISettings Members
    }
}