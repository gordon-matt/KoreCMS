using System.ComponentModel.DataAnnotations;
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
            ShowOnMenus = true;
            MenuPosition = 0;
        }

        [Display(Name = "Page Title")]
        public string PageTitle { get; set; }

        [Display(Name = "Date Format")]
        public string DateFormat { get; set; }

        [Display(Name = "# Items Per Page")]
        public byte ItemsPerPage { get; set; }

        [Display(Name = "Show on Menus")]
        public bool ShowOnMenus { get; set; }

        [Display(Name = "Menu Position")]
        public byte MenuPosition { get; set; }

        [Display(Name = "Use Ajax")]
        public bool UseAjax { get; set; }

        public string AccessRestrictions { get; set; }

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