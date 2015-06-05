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

        [LocalizedDisplayName(LocalizableStrings.Settings.Store.PageTitle)]
        public string PageTitle { get; set; }

        [LocalizedDisplayName(LocalizableStrings.Settings.Store.Currency)]
        public string Currency { get; set; }

        [LocalizedDisplayName(LocalizableStrings.Settings.Store.ShippingFlatRate)]
        public float ShippingFlatRate { get; set; }

        [LocalizedDisplayName(LocalizableStrings.Settings.Store.CategoriesPerPage)]
        public byte CategoriesPerPage { get; set; }

        [LocalizedDisplayName(LocalizableStrings.Settings.Store.ProductsPerPage)]
        public byte ProductsPerPage { get; set; }

        [LocalizedDisplayName(LocalizableStrings.Settings.Store.ShowOnMenus)]
        public bool ShowOnMenus { get; set; }

        [LocalizedDisplayName(LocalizableStrings.Settings.Store.MenuPosition)]
        public byte MenuPosition { get; set; }

        [LocalizedDisplayName(LocalizableStrings.Settings.Store.UseAjax)]
        public bool UseAjax { get; set; }

        [LocalizedDisplayName(LocalizableStrings.Settings.Store.LayoutPathOverride)]
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