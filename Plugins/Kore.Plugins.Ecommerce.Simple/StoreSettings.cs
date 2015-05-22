using Kore.ComponentModel;
using Kore.Web.Configuration;

namespace Kore.Plugins.Ecommerce.Simple
{
    public class StoreSettings : ISettings
    {
        public StoreSettings()
        {
            PageTitle = "Store";
            Currency = "USD $";
            CategoriesPerPage = 10;
            ProductsPerPage = 25;
            ShowOnMenus = true;
            MenuPosition = 0;
        }

        [LocalizedDisplayName(LocalizableStrings.StoreSettings.PageTitle)]
        public string PageTitle { get; set; }

        [LocalizedDisplayName(LocalizableStrings.StoreSettings.Currency)]
        public string Currency { get; set; }

        [LocalizedDisplayName(LocalizableStrings.StoreSettings.CategoriesPerPage)]
        public byte CategoriesPerPage { get; set; }

        [LocalizedDisplayName(LocalizableStrings.StoreSettings.ProductsPerPage)]
        public byte ProductsPerPage { get; set; }

        [LocalizedDisplayName(LocalizableStrings.StoreSettings.ShowOnMenus)]
        public bool ShowOnMenus { get; set; }

        [LocalizedDisplayName(LocalizableStrings.StoreSettings.MenuPosition)]
        public byte MenuPosition { get; set; }

        [LocalizedDisplayName(LocalizableStrings.StoreSettings.UseAjax)]
        public bool UseAjax { get; set; }

        [LocalizedDisplayName(LocalizableStrings.StoreSettings.LayoutPathOverride)]
        public string LayoutPathOverride { get; set; }

        #region ISettings Members

        public string Name
        {
            get { return "Simple Commerce: Store Settings"; }
        }

        public string EditorTemplatePath
        {
            get { return "/Plugins/Ecommerce.Simple/Views/Shared/EditorTemplates/StoreSettings.cshtml"; }
        }

        #endregion ISettings Members
    }
}