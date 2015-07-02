using System.Collections.Generic;
using Kore.Localization;

namespace Kore.Plugins.Widgets.OwlCarousel.Infrastructure
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
                    { LocalizableStrings.OwlCarouselBlock.AddClassActive, "Add Class Active" },
                    { LocalizableStrings.OwlCarouselBlock.AfterAction, "After Action" },
                    { LocalizableStrings.OwlCarouselBlock.AfterInit, "After Init" },
                    { LocalizableStrings.OwlCarouselBlock.AfterLazyLoad, "After Lazy Load" },
                    { LocalizableStrings.OwlCarouselBlock.AfterMove, "After Move" },
                    { LocalizableStrings.OwlCarouselBlock.AfterUpdate, "After Update" },
                    { LocalizableStrings.OwlCarouselBlock.AutoHeight, "Auto Height" },
                    { LocalizableStrings.OwlCarouselBlock.AutoPlay, "Auto Play" },
                    { LocalizableStrings.OwlCarouselBlock.AutoPlaySpeed, "Auto Play Speed" },
                    { LocalizableStrings.OwlCarouselBlock.BaseClass, "Base Class" },
                    { LocalizableStrings.OwlCarouselBlock.BeforeInit, "Before Init" },
                    { LocalizableStrings.OwlCarouselBlock.BeforeMove, "Before Move" },
                    { LocalizableStrings.OwlCarouselBlock.BeforeUpdate, "Before Update" },
                    { LocalizableStrings.OwlCarouselBlock.ControlId, "Control ID" },
                    { LocalizableStrings.OwlCarouselBlock.DragBeforeAnimFinish, "Drag Before Animation Finish" },
                    { LocalizableStrings.OwlCarouselBlock.Items, "Items" },
                    { LocalizableStrings.OwlCarouselBlock.ItemsCustom, "Items: Custom" },
                    { LocalizableStrings.OwlCarouselBlock.ItemsDesktop, "Items: Desktop" },
                    { LocalizableStrings.OwlCarouselBlock.ItemsDesktopSmall, "Items: Desktop, Small" },
                    { LocalizableStrings.OwlCarouselBlock.ItemsMobile, "Items: Mobile" },
                    { LocalizableStrings.OwlCarouselBlock.ItemsScaleUp, "Items: Scale Up" },
                    { LocalizableStrings.OwlCarouselBlock.ItemsTablet, "Items Tablet" },
                    { LocalizableStrings.OwlCarouselBlock.ItemsTabletSmall, "Items: Tablet, Small" },
                    { LocalizableStrings.OwlCarouselBlock.JsonPath, "JSON Path" },
                    { LocalizableStrings.OwlCarouselBlock.JsonSuccess, "JSON Success" },
                    { LocalizableStrings.OwlCarouselBlock.LazyEffect, "Lazy Effect" },
                    { LocalizableStrings.OwlCarouselBlock.LazyFollow, "Lazy Follow" },
                    { LocalizableStrings.OwlCarouselBlock.LazyLoad, "Lazy Load" },
                    { LocalizableStrings.OwlCarouselBlock.MediaFolder, "Media Folder" },
                    { LocalizableStrings.OwlCarouselBlock.MouseDrag, "Mouse Drag" },
                    { LocalizableStrings.OwlCarouselBlock.Navigation, "Navigation" },
                    { LocalizableStrings.OwlCarouselBlock.NavigationText, "Navigation Text" },
                    { LocalizableStrings.OwlCarouselBlock.Pagination, "Pagination" },
                    { LocalizableStrings.OwlCarouselBlock.PaginationNumbers, "Pagination Numbers" },
                    { LocalizableStrings.OwlCarouselBlock.PaginationSpeed, "Pagination Speed" },
                    { LocalizableStrings.OwlCarouselBlock.Responsive, "Responsive" },
                    { LocalizableStrings.OwlCarouselBlock.ResponsiveBaseWidth, "Responsive Base Width" },
                    { LocalizableStrings.OwlCarouselBlock.ResponsiveRefreshRate, "Responsive Refresh Rate" },
                    { LocalizableStrings.OwlCarouselBlock.RewindNav, "Rewind Nav" },
                    { LocalizableStrings.OwlCarouselBlock.RewindSpeed, "Rewind Speed" },
                    { LocalizableStrings.OwlCarouselBlock.ScrollPerPage, "Scroll Per Page" },
                    { LocalizableStrings.OwlCarouselBlock.SingleItem, "Single Item" },
                    { LocalizableStrings.OwlCarouselBlock.SlideSpeed, "Slide Speed" },
                    { LocalizableStrings.OwlCarouselBlock.StartDragging, "Start Dragging" },
                    { LocalizableStrings.OwlCarouselBlock.StopOnHover, "Stop On Hover" },
                    { LocalizableStrings.OwlCarouselBlock.Theme, "Theme" },
                    { LocalizableStrings.OwlCarouselBlock.TouchDrag, "Touch Drag" },
                    { LocalizableStrings.OwlCarouselBlock.TransitionStyle, "Transition Style" },
                    { LocalizableStrings.OwlCarouselBlock.EditorTabs.Advanced, "Advanced" },
                    { LocalizableStrings.OwlCarouselBlock.EditorTabs.Callbacks, "Callbacks" },
                    { LocalizableStrings.OwlCarouselBlock.EditorTabs.General, "General" },
                    { LocalizableStrings.OwlCarouselBlock.EditorTabs.ItemOptions, "Item Options" },
                    { LocalizableStrings.OwlCarousel, "OwlCarousel" },
                };
            }
        }

        #endregion ILanguagePack Members
    }
}