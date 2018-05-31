using Kore.ComponentModel;
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

        [LocalizedDisplayName(KoreCmsLocalizableStrings.Settings.Blog.PageTitle)]
        public string PageTitle { get; set; }

        [LocalizedDisplayName(KoreCmsLocalizableStrings.Settings.Blog.DateFormat)]
        public string DateFormat { get; set; }

        [LocalizedDisplayName(KoreCmsLocalizableStrings.Settings.Blog.ItemsPerPage)]
        public byte ItemsPerPage { get; set; }

        [LocalizedDisplayName(KoreCmsLocalizableStrings.Settings.Blog.ShowOnMenus)]
        public bool ShowOnMenus { get; set; }

        [LocalizedDisplayName(KoreCmsLocalizableStrings.Settings.Blog.MenuPosition)]
        public byte MenuPosition { get; set; }

        [LocalizedDisplayName(KoreCmsLocalizableStrings.Settings.Blog.AccessRestrictions)]
        public string AccessRestrictions { get; set; }

        [LocalizedDisplayName(KoreCmsLocalizableStrings.Settings.Blog.LayoutPathOverride)]
        public string LayoutPathOverride { get; set; }

        #region ISettings Members

        public string Name => "CMS: Blog Settings";

        public bool IsTenantRestricted => false;

        public string EditorTemplatePath => "Kore.Web.ContentManagement.Areas.Admin.Blog.Views.Shared.EditorTemplates.BlogSettings";

        #endregion ISettings Members
    }
}