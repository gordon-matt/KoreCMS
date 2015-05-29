using System.Collections.Generic;
using Kore.Localization;

namespace Kore.Plugins.Ecommerce.Simple.Infrastructure
{
    public class LanguagePackInvariant : ILanguagePack
    {
        #region ILanguagePack Members

        public string CultureCode
        {
            get { return null; }
        }

        public IDictionary<string, string> LocalizedStrings
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { LocalizableStrings.AddressModel.AddressLine1, "Address Line 1" },
                    { LocalizableStrings.AddressModel.AddressLine2, "Address Line 2" },
                    { LocalizableStrings.AddressModel.AddressLine3, "Address Line 3" },
                    { LocalizableStrings.AddressModel.City, "City" },
                    { LocalizableStrings.AddressModel.Country, "Country" },
                    { LocalizableStrings.AddressModel.Email, "E-mail" },
                    { LocalizableStrings.AddressModel.FamilyName, "Family Name" },
                    { LocalizableStrings.AddressModel.GivenNames, "Given Name/s" },
                    { LocalizableStrings.AddressModel.PhoneNumber, "Phone Number" },
                    { LocalizableStrings.AddressModel.PostalCode, "Zip/Postal Code" },
                    { LocalizableStrings.AddToCart, "Add to Cart" },
                    { LocalizableStrings.AddToCartSuccess, "The specified product has been added to your cart." },
                    { LocalizableStrings.BillingAddress, "Billing Address" },
                    { LocalizableStrings.CartUpdated, "Successfully updated cart." },
                    { LocalizableStrings.Categories, "Categories" },
                    { LocalizableStrings.CategoryModel.Description, "Description" },
                    { LocalizableStrings.CategoryModel.ImageUrl, "Image URL" },
                    { LocalizableStrings.CategoryModel.MetaDescription, "Meta Description" },
                    { LocalizableStrings.CategoryModel.MetaKeywords, "Meta Keywords" },
                    { LocalizableStrings.CategoryModel.Name, "Name" },
                    { LocalizableStrings.CategoryModel.Order, "Order" },
                    { LocalizableStrings.CategoryModel.Slug, "Slug" },
                    { LocalizableStrings.Checkout, "Checkout" },
                    { LocalizableStrings.CheckoutCompleted.ReturnButtonText, "Return to Store" },
                    { LocalizableStrings.CheckoutCompleted.ThankYouMessage, "Thank you for your purchase. Your Order ID is: {0}" },
                    { LocalizableStrings.CheckoutCompleted.Title, "Checkout Completed" },
                    { LocalizableStrings.CheckoutTable.Price, "Price" },
                    { LocalizableStrings.CheckoutTable.Product, "Product" },
                    { LocalizableStrings.CheckoutTable.Quantity, "Quantity" },
                    { LocalizableStrings.CircularRelationshipError, "That action would cause a circular relationship!" },
                    { LocalizableStrings.Completed, "Completed" },
                    { LocalizableStrings.ConnectingToPayPal, "Connecting to PayPal, please wait..." },
                    { LocalizableStrings.ContentBlocks.CartLinkBlock.CssClass, "CSS Class" },
                    { LocalizableStrings.ContentBlocks.CartLinkBlock.IconCssClass, "Icon CSS Class" },
                    { LocalizableStrings.ContentBlocks.CheckoutLinkBlock.CssClass, "CSS Class" },
                    { LocalizableStrings.ContentBlocks.CheckoutLinkBlock.IconCssClass, "Icon CSS Class" },
                    { LocalizableStrings.ContinueShopping, "Continue Shopping..." },
                    { LocalizableStrings.CouldNotFindProduct, "Could not find specified product." },
                    { LocalizableStrings.Filter.AllCategoryFormat, "[All {0}]" },
                    { LocalizableStrings.ItemRemovedFromCart, "Item removed from cart." },
                    { LocalizableStrings.MadeChanges, "Made Changes?" },
                    { LocalizableStrings.OrderModel.Id, "Order ID" },
                    { LocalizableStrings.OrderModel.OrderDateUtc, "Date (UTC)" },
                    { LocalizableStrings.OrderModel.OrderTotal, "Total" },
                    { LocalizableStrings.OrderModel.PaymentStatus, "Payment Status" },
                    { LocalizableStrings.OrderModel.Status, "Status" },
                    { LocalizableStrings.Orders, "Orders" },
                    { LocalizableStrings.Price, "Price" },
                    { LocalizableStrings.ProductModel.FullDescription, "Full Description" },
                    { LocalizableStrings.ProductModel.MainImageUrl, "Main Image URL" },
                    { LocalizableStrings.ProductModel.MetaDescription, "Meta Description" },
                    { LocalizableStrings.ProductModel.MetaKeywords, "Meta Keywords" },
                    { LocalizableStrings.ProductModel.Name, "Name" },
                    { LocalizableStrings.ProductModel.Price, "Price" },
                    { LocalizableStrings.ProductModel.ShippingCost, "Shipping Cost" },
                    { LocalizableStrings.ProductModel.ShortDescription, "Short Description" },
                    { LocalizableStrings.ProductModel.Slug, "Slug" },
                    { LocalizableStrings.ProductModel.Tax, "Tax" },
                    { LocalizableStrings.Products, "Products" },
                    { LocalizableStrings.QuantityUpdated, "Quantity Updated" },
                    { LocalizableStrings.SelectCategoryToBeginEdit, "Select a category to begin editing." },
                    { LocalizableStrings.Settings.PayPal.CurrencyCode, "Currency Code" },
                    { LocalizableStrings.Settings.PayPal.Merchant, "Merchant" },
                    { LocalizableStrings.Settings.PayPal.PdtToken, "PDT Token" },
                    { LocalizableStrings.Settings.PayPal.ProductionUrl, "Production URL" },
                    { LocalizableStrings.Settings.PayPal.SandboxUrl, "Sandbox URL" },
                    { LocalizableStrings.Settings.PayPal.UseSandboxMode, "Use Sandbox Mode" },
                    { LocalizableStrings.Settings.Store.CategoriesPerPage, "Categories Per Page" },
                    { LocalizableStrings.Settings.Store.Currency, "Currency" },
                    { LocalizableStrings.Settings.Store.LayoutPathOverride, "Layout Path (Override)" },
                    { LocalizableStrings.Settings.Store.MenuPosition, "Menu Position" },
                    { LocalizableStrings.Settings.Store.PageTitle, "Page Title" },
                    { LocalizableStrings.Settings.Store.ProductsPerPage, "Products Per Page" },
                    { LocalizableStrings.Settings.Store.ShowOnMenus, "Show on Menus" },
                    { LocalizableStrings.Settings.Store.UseAjax, "Use Ajax" },
                    { LocalizableStrings.Shipping, "Shipping" },
                    { LocalizableStrings.ShippingAddress, "Shipping Address" },
                    { LocalizableStrings.ShippingAddressSameAsBillingAddress, "Same as Billing Address" },
                    { LocalizableStrings.ShippingTotal, "Shipping Total" },
                    { LocalizableStrings.ShoppingCart, "Shopping Cart" },
                    { LocalizableStrings.Store, "Store" },
                    { LocalizableStrings.SubTotal, "Sub Total" },
                    { LocalizableStrings.Tax, "Tax" },
                    { LocalizableStrings.TaxTotal, "Tax Total" },
                    { LocalizableStrings.Total, "Total" },
                    { LocalizableStrings.UpdateCart, "Update Cart" },
                };
            }
        }

        #endregion ILanguagePack Members
    }
}