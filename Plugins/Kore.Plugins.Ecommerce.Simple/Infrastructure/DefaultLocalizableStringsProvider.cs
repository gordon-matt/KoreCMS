using System.Collections.Generic;
using Kore.Localization;

namespace Kore.Plugins.Ecommerce.Simple.Infrastructure
{
    public class DefaultLocalizableStringsProvider : IDefaultLocalizableStringsProvider
    {
        #region IDefaultLocalizableStringsProvider Members

        public ICollection<Translation> GetTranslations()
        {
            return new[]
            {
                new Translation
                {
                    CultureCode = null,
                    LocalizedStrings = new Dictionary<string, string>
                    {
                        { LocalizableStrings.MadeChanges, "Made Changes?" },
                        { LocalizableStrings.AddToCart, "Add to Cart" },
                        { LocalizableStrings.Categories, "Categories" },
                        { LocalizableStrings.Checkout, "Checkout" },
                        { LocalizableStrings.CircularRelationshipError, "That action would cause a circular relationship!" },
                        { LocalizableStrings.ContinueShopping, "Continue Shopping..." },
                        { LocalizableStrings.Orders, "Orders" },
                        { LocalizableStrings.Price, "Price" },
                        { LocalizableStrings.Products, "Products" },
                        { LocalizableStrings.SelectCategoryToBeginEdit, "Select a category to begin editing." },
                        { LocalizableStrings.Shipping, "Shipping" },
                        { LocalizableStrings.ShippingTotal, "Shipping Total" },
                        { LocalizableStrings.ShoppingCart, "Shopping Cart" },
                        { LocalizableStrings.Store, "Store" },
                        { LocalizableStrings.SubTotal, "Sub Total" },
                        { LocalizableStrings.Tax, "Tax" },
                        { LocalizableStrings.TaxTotal, "Tax Total" },
                        { LocalizableStrings.Total, "Total" },
                        { LocalizableStrings.UpdateCart, "Update Cart" },
                        { LocalizableStrings.CheckoutLinkBlock.CssClass, "CSS Class" },
                        { LocalizableStrings.CheckoutLinkBlock.IconCssClass, "Icon CSS Class" },
                        { LocalizableStrings.PayPalSettings.CancelUrlRedirectsToOrderDetailsPage, "Cancel URL Redirects to Order Details Page" },
                        { LocalizableStrings.PayPalSettings.CurrencyCode, "Currency Code" },
                        { LocalizableStrings.PayPalSettings.Merchant, "Merchant" },
                        { LocalizableStrings.PayPalSettings.PdtToken, "PDT Token" },
                        { LocalizableStrings.PayPalSettings.ProductionUrl, "Production URL" },
                        { LocalizableStrings.PayPalSettings.SandboxUrl, "Sandbox URL" },
                        { LocalizableStrings.PayPalSettings.UseSandboxMode, "Use Sandbox Mode" },
                        { LocalizableStrings.StoreSettings.CategoriesPerPage, "Categories Per Page" },
                        { LocalizableStrings.StoreSettings.Currency, "Currency" },
                        { LocalizableStrings.StoreSettings.MenuPosition, "Menu Position" },
                        { LocalizableStrings.StoreSettings.PageTitle, "Page Title" },
                        { LocalizableStrings.StoreSettings.ProductsPerPage, "Products Per Page" },
                        { LocalizableStrings.StoreSettings.ShowOnMenus, "Show on Menus" },
                        { LocalizableStrings.StoreSettings.UseAjax, "Use Ajax" },
                    }
                }
            };
        }

        #endregion IDefaultLocalizableStringsProvider Members
    }
}