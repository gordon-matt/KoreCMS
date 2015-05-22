using Kore.ComponentModel;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;

namespace Kore.Plugins.Ecommerce.Simple.ContentBlocks
{
    public class CheckoutLinkBlock : ContentBlockBase
    {
        public CheckoutLinkBlock()
        {
            CssClass = "btn btn-success";
            IconCssClass = "kore-icon kore-icon-shopping-cart";
        }

        [LocalizedDisplayName(LocalizableStrings.CheckoutLinkBlock.CssClass)]
        public string CssClass { get; set; }

        [LocalizedDisplayName(LocalizableStrings.CheckoutLinkBlock.IconCssClass)]
        public string IconCssClass { get; set; }

        #region ContentBlockBase Overrides

        public override string Name
        {
            get { return "Simple Commerce: Checkout Link"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "/Plugins/Ecommerce.Simple/Views/Shared/DisplayTemplates/CheckoutLinkBlock.cshtml"; }
        }

        public override string EditorTemplatePath
        {
            get { return "/Plugins/Ecommerce.Simple/Views/Shared/EditorTemplates/CheckoutLinkBlock.cshtml"; }
        }

        #endregion ContentBlockBase Overrides
    }
}