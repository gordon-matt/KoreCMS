using Kore.ComponentModel;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;

namespace Kore.Plugins.Ecommerce.Simple.ContentBlocks
{
    public class CartLinkBlock : ContentBlockBase
    {
        public CartLinkBlock()
        {
            CssClass = "btn btn-success";
        }

        [LocalizedDisplayName(LocalizableStrings.ContentBlocks.CartLinkBlock.CssClass)]
        public string CssClass { get; set; }

        [LocalizedDisplayName(LocalizableStrings.ContentBlocks.CartLinkBlock.IconCssClass)]
        public string IconCssClass { get; set; }

        #region ContentBlockBase Overrides

        public override string Name
        {
            get { return "Simple Commerce: Cart Link"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "/Plugins/Ecommerce.Simple/Views/Shared/DisplayTemplates/CartLinkBlock.cshtml"; }
        }

        public override string EditorTemplatePath
        {
            get { return "/Plugins/Ecommerce.Simple/Views/Shared/EditorTemplates/CartLinkBlock.cshtml"; }
        }

        #endregion ContentBlockBase Overrides
    }
}