using System.Collections.Generic;
using Kore.Localization;

namespace Kore.Plugins.Widgets.RevolutionSlider.Infrastructure
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
                    { LocalizableStrings.RevolutionSlider, "Revolution Slider" },
                    { LocalizableStrings.Sliders, "Sliders" },
                    { LocalizableStrings.Slides, "Slides" },
                    { LocalizableStrings.Models.Slide.BackgroundFit, "Background Fit" },
                    { LocalizableStrings.Models.Slide.BackgroundFitCustomValue, "Background Fit: Custom Value" },
                    { LocalizableStrings.Models.Slide.BackgroundFitEnd, "Background Fit End" },
                    { LocalizableStrings.Models.Slide.BackgroundPosition, "Background Position" },
                    { LocalizableStrings.Models.Slide.BackgroundPositionEnd, "Background Position End" },
                    { LocalizableStrings.Models.Slide.BackgroundRepeat, "Background Repeat" },
                    { LocalizableStrings.Models.Slide.Delay, "Delay" },
                    { LocalizableStrings.Models.Slide.Duration, "Duration" },
                    { LocalizableStrings.Models.Slide.Easing, "Easing" },
                    { LocalizableStrings.Models.Slide.KenBurnsEffect, "Ken Burns Effect" },
                    { LocalizableStrings.Models.Slide.LazyLoad, "Lazy Load" },
                    { LocalizableStrings.Models.Slide.Link, "Link" },
                    { LocalizableStrings.Models.Slide.MasterSpeed, "Master Speed" },
                    { LocalizableStrings.Models.Slide.Order, "Order" },
                    { LocalizableStrings.Models.Slide.RandomTransition, "Random Transition" },
                    { LocalizableStrings.Models.Slide.SlideIndex, "Slide Index" },
                    { LocalizableStrings.Models.Slide.SliderId, "Slider" },
                    { LocalizableStrings.Models.Slide.SlotAmount, "Slot Amount" },
                    { LocalizableStrings.Models.Slide.Target, "Target" },
                    { LocalizableStrings.Models.Slide.Thumb, "Thumb" },
                    { LocalizableStrings.Models.Slide.Title, "Title" },
                    { LocalizableStrings.Models.Slide.Transition, "Transition" },
                    { LocalizableStrings.Models.Slider.Name, "Name" },
                    { LocalizableStrings.RevolutionSliderBlock.EditorTabs.General, "General" },
                    { LocalizableStrings.RevolutionSliderBlock.EditorTabs.Navigation, "Navigation" },
                    { LocalizableStrings.RevolutionSliderBlock.EditorTabs.Loops, "Loops" },
                    { LocalizableStrings.RevolutionSliderBlock.EditorTabs.MobileVisibility, "Mobile Visibility" },
                    { LocalizableStrings.RevolutionSliderBlock.EditorTabs.LayoutStyles, "Layout Styles" },
                    { LocalizableStrings.RevolutionSliderBlock.EditorTabs.Parallax, "Parallax" },
                    { LocalizableStrings.RevolutionSliderBlock.EditorTabs.PanZoom, "Pan Zoom" },
                    { LocalizableStrings.RevolutionSliderBlock.EditorTabs.Other, "Other" },
                    { LocalizableStrings.RevolutionSliderBlock.Delay, "Delay" },
                    { LocalizableStrings.RevolutionSliderBlock.StartHeight, "Start Height" },
                    { LocalizableStrings.RevolutionSliderBlock.StartWidth, "Start Width" },
                    { LocalizableStrings.RevolutionSliderBlock.Shuffle, "Shuffle" },
                    { LocalizableStrings.RevolutionSliderBlock.KeyboardNavigation, "Keyboard Navigation" },
                    { LocalizableStrings.RevolutionSliderBlock.OnHoverStop, "On Hover Stop" },
                    { LocalizableStrings.RevolutionSliderBlock.ThumbWidth, "Thumb Width" },
                    { LocalizableStrings.RevolutionSliderBlock.ThumbHeight, "Thumb Height" },
                    { LocalizableStrings.RevolutionSliderBlock.ThumbAmount, "Thumb Amount" },
                    { LocalizableStrings.RevolutionSliderBlock.HideThumbs, "Hide Thumbs" },
                    { LocalizableStrings.RevolutionSliderBlock.NavigationType, "Navigation Type" },
                    { LocalizableStrings.RevolutionSliderBlock.NavigationArrows, "Navigation Arrows" },
                    { LocalizableStrings.RevolutionSliderBlock.NavigationStyle, "Navigation Style" },
                    { LocalizableStrings.RevolutionSliderBlock.NavigationHAlign, "Navigation H-Align" },
                    { LocalizableStrings.RevolutionSliderBlock.NavigationVAlign, "Navigation V-Align" },
                    { LocalizableStrings.RevolutionSliderBlock.NavigationHOffset, "Navigation H-Offset" },
                    { LocalizableStrings.RevolutionSliderBlock.NavigationVOffset, "Navigation V-Offset" },
                    { LocalizableStrings.RevolutionSliderBlock.SoloArrowLeftHAlign, "Solo Arrow Left H-Align" },
                    { LocalizableStrings.RevolutionSliderBlock.SoloArrowRightHAlign, "Solo Arrow Right H-Align" },
                    { LocalizableStrings.RevolutionSliderBlock.SoloArrowLeftVAlign, "Solo Arrow Left V-Align" },
                    { LocalizableStrings.RevolutionSliderBlock.SoloArrowRightVAlign, "Solo Arrow Right V-Align" },
                    { LocalizableStrings.RevolutionSliderBlock.SoloArrowLeftHOffset, "Solo Arrow Left H-Offset" },
                    { LocalizableStrings.RevolutionSliderBlock.SoloArrowRightHOffset, "Solo Arrow Right H-Offset" },
                    { LocalizableStrings.RevolutionSliderBlock.SoloArrowLeftVOffset, "Solo Arrow Left V-Offset" },
                    { LocalizableStrings.RevolutionSliderBlock.SoloArrowRightVOffset, "Solo Arrow Right V-Offset" },
                    { LocalizableStrings.RevolutionSliderBlock.TouchEnabled, "Touch Enabled" },
                    { LocalizableStrings.RevolutionSliderBlock.SwipeVelocity, "Swipe Velocity" },
                    { LocalizableStrings.RevolutionSliderBlock.SwipeMaxTouches, "Swipe Max Touches" },
                    { LocalizableStrings.RevolutionSliderBlock.SwipeMinTouches, "Swipe Min Touches" },
                    { LocalizableStrings.RevolutionSliderBlock.DragBlockVertical, "Drag Block Vertical" },
                    { LocalizableStrings.RevolutionSliderBlock.StartWithSlide, "Start with Slide" },
                    { LocalizableStrings.RevolutionSliderBlock.StopAtSlide, "Stop at Slide" },
                    { LocalizableStrings.RevolutionSliderBlock.StopAfterLoops, "Stop After Loops" },
                    { LocalizableStrings.RevolutionSliderBlock.HideCaptionAtLimit, "Hide Caption at Limit" },
                    { LocalizableStrings.RevolutionSliderBlock.HideAllCaptionAtLimit, "Hide All Captions at Limit" },
                    { LocalizableStrings.RevolutionSliderBlock.HideSliderAtLimit, "Hide Slider at Limit" },
                    { LocalizableStrings.RevolutionSliderBlock.HideNavDelayOnMobile, "Hide Nav Delay on Mobile" },
                    { LocalizableStrings.RevolutionSliderBlock.HideThumbsOnMobile, "Hide Thumbs on Mobile" },
                    { LocalizableStrings.RevolutionSliderBlock.HideBulletsOnMobile, "Hide Bullets on Mobile" },
                    { LocalizableStrings.RevolutionSliderBlock.HideArrowsOnMobile, "Hide Arrows on Mobile" },
                    { LocalizableStrings.RevolutionSliderBlock.HideThumbsUnderResolution, "Hide Thumbs under Resolution" },
                    { LocalizableStrings.RevolutionSliderBlock.Spinner, "Spinner" },
                    { LocalizableStrings.RevolutionSliderBlock.HideTimerBar, "Hide Timer Bar" },
                    { LocalizableStrings.RevolutionSliderBlock.FullWidth, "Full Width" },
                    { LocalizableStrings.RevolutionSliderBlock.AutoHeight, "Auto Height" },
                    { LocalizableStrings.RevolutionSliderBlock.MinHeight, "Min Height" },
                    { LocalizableStrings.RevolutionSliderBlock.FullScreenAlignForce, "Full Screen Align Force" },
                    { LocalizableStrings.RevolutionSliderBlock.ForceFullWidth, "Force Full Width" },
                    { LocalizableStrings.RevolutionSliderBlock.FullScreen, "Full Screen" },
                    { LocalizableStrings.RevolutionSliderBlock.FullScreenOffsetContainer, "Full Screen Offset Container" },
                    { LocalizableStrings.RevolutionSliderBlock.FullScreenOffset, "Full Screen Offset" },
                    { LocalizableStrings.RevolutionSliderBlock.FullScreenOffsetUnit, "Full Screen Offset Unit" },
                    { LocalizableStrings.RevolutionSliderBlock.Shadow, "Shadow" },
                    { LocalizableStrings.RevolutionSliderBlock.DottedOverlay, "Dotted Overlay" },
                    { LocalizableStrings.RevolutionSliderBlock.Parallax, "Parallax" },
                    { LocalizableStrings.RevolutionSliderBlock.ParallaxBgFreeze, "Parallax Background Freeze" },
                    { LocalizableStrings.RevolutionSliderBlock.ParallaxLevels, "Parallax Levels" },
                    { LocalizableStrings.RevolutionSliderBlock.ParallaxDisableOnMobile, "Parallax Disable on Mobile" },
                    { LocalizableStrings.RevolutionSliderBlock.PanZoomDisableOnMobile, "Pan Zoom Disable on Mobile" },
                    { LocalizableStrings.RevolutionSliderBlock.SimplifyAll, "Simplify All" },
                };
            }
        }

        #endregion ILanguagePack Members
    }
}