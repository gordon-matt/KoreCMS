using System.Text;
using System.Web.Mvc;
using Kore.ComponentModel;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;
using Kore.Web.Mvc.Html;

namespace Kore.Plugins.Widgets.RevolutionSlider.ContentBlocks
{
    public class RevolutionSliderBlock : ContentBlockBase
    {
        #region General

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.SliderId)]
        public int SliderId { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.ControlId)]
        public string ControlId { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.Delay)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.Delay)]
        public short Delay { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.StartHeight)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.StartHeight)]
        public short StartHeight { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.StartWidth)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.StartWidth)]
        public short StartWidth { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.Shuffle)]
        public bool Shuffle { get; set; }

        #endregion General

        #region Navigation

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.KeyboardNavigation)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.KeyboardNavigation)]
        public bool KeyboardNavigation { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.OnHoverStop)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.OnHoverStop)]
        public bool OnHoverStop { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.ThumbWidth)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.ThumbWidth)]
        public short ThumbWidth { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.ThumbHeight)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.ThumbHeight)]
        public short ThumbHeight { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.ThumbAmount)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.ThumbAmount)]
        public byte ThumbAmount { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.HideThumbs)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.HideThumbs)]
        public short HideThumbs { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.NavigationType)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.NavigationType)]
        public NavigationType NavigationType { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.NavigationArrows)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.NavigationArrows)]
        public NavigationArrows NavigationArrows { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.NavigationStyle)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.NavigationStyle)]
        public NavigationStyle NavigationStyle { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.NavigationHAlign)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.NavigationHAlign)]
        public NavigationHAlign NavigationHAlign { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.NavigationVAlign)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.NavigationVAlign)]
        public NavigationVAlign NavigationVAlign { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.NavigationHOffset)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.NavigationHOffset)]
        public short NavigationHOffset { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.NavigationVOffset)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.NavigationVOffset)]
        public short NavigationVOffset { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.SoloArrowLeftHAlign)]
        public NavigationHAlign SoloArrowLeftHAlign { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.SoloArrowRightHAlign)]
        public NavigationHAlign SoloArrowRightHAlign { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.SoloArrowLeftVAlign)]
        public NavigationVAlign SoloArrowLeftVAlign { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.SoloArrowRightVAlign)]
        public NavigationVAlign SoloArrowRightVAlign { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.SoloArrowLeftHOffset)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.SoloArrowLeftHOffset)]
        public short SoloArrowLeftHOffset { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.SoloArrowRightHOffset)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.SoloArrowRightHOffset)]
        public short SoloArrowRightHOffset { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.SoloArrowLeftVOffset)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.SoloArrowLeftVOffset)]
        public short SoloArrowLeftVOffset { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.SoloArrowRightVOffset)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.SoloArrowRightVOffset)]
        public short SoloArrowRightVOffset { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.TouchEnabled)]
        public bool TouchEnabled { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.SwipeVelocity)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.SwipeVelocity)]
        public float SwipeVelocity { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.SwipeMaxTouches)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.SwipeMaxTouches)]
        public byte SwipeMaxTouches { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.SwipeMinTouches)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.SwipeMinTouches)]
        public byte SwipeMinTouches { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.DragBlockVertical)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.DragBlockVertical)]
        public bool DragBlockVertical { get; set; }

        #endregion Navigation

        #region Loops

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.StartWithSlide)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.StartWithSlide)]
        public byte StartWithSlide { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.StopAtSlide)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.StopAtSlide)]
        public sbyte StopAtSlide { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.StopAfterLoops)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.StopAfterLoops)]
        public sbyte StopAfterLoops { get; set; }

        #endregion Loops

        #region Mobile Visibility

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.HideCaptionAtLimit)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.HideCaptionAtLimit)]
        public short HideCaptionAtLimit { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.HideAllCaptionAtLimit)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.HideAllCaptionAtLimit)]
        public short HideAllCaptionAtLimit { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.HideSliderAtLimit)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.HideSliderAtLimit)]
        public short HideSliderAtLimit { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.HideNavDelayOnMobile)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.HideNavDelayOnMobile)]
        public short HideNavDelayOnMobile { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.HideThumbsOnMobile)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.HideThumbsOnMobile)]
        public bool HideThumbsOnMobile { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.HideBulletsOnMobile)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.HideBulletsOnMobile)]
        public bool HideBulletsOnMobile { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.HideArrowsOnMobile)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.HideArrowsOnMobile)]
        public bool HideArrowsOnMobile { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.HideThumbsUnderResolution)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.HideThumbsUnderResolution)]
        public short HideThumbsUnderResolution { get; set; }

        #endregion Mobile Visibility

        #region Layout Styles

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.Spinner)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.Spinner)]
        public Spinner Spinner { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.HideTimerBar)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.HideTimerBar)]
        public bool HideTimerBar { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.FullWidth)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.FullWidth)]
        public bool FullWidth { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.AutoHeight)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.AutoHeight)]
        public bool AutoHeight { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.MinHeight)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.MinHeight)]
        public short MinHeight { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.FullScreenAlignForce)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.FullScreenAlignForce)]
        public bool FullScreenAlignForce { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.ForceFullWidth)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.ForceFullWidth)]
        public bool ForceFullWidth { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.FullScreen)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.FullScreen)]
        public bool FullScreen { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.FullScreenOffsetContainer)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.FullScreenOffsetContainer)]
        public string FullScreenOffsetContainer { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.FullScreenOffset)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.FullScreenOffset)]
        public short FullScreenOffset { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.FullScreenOffsetUnit)]
        public CssUnit FullScreenOffsetUnit { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.Shadow)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.Shadow)]
        public Shadow Shadow { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.DottedOverlay)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.DottedOverlay)]
        public DottedOverlay DottedOverlay { get; set; }

        #endregion Layout Styles

        #region Parallax

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.Parallax)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.Parallax)]
        public ParallaxMode Parallax { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.ParallaxBgFreeze)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.ParallaxBgFreeze)]
        public bool ParallaxBgFreeze { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.ParallaxLevels)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.ParallaxLevels)]
        public string ParallaxLevels { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.ParallaxDisableOnMobile)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.ParallaxDisableOnMobile)]
        public bool ParallaxDisableOnMobile { get; set; }

        #endregion Parallax

        #region Pan Zoom

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.PanZoomDisableOnMobile)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.PanZoomDisableOnMobile)]
        public bool PanZoomDisableOnMobile { get; set; }

        #endregion Pan Zoom

        #region Other

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.SimplifyAll)]
        [LocalizedHelpText(LocalizableStrings.RevolutionSliderBlock.HelpText.SimplifyAll)]
        public bool SimplifyAll { get; set; }

        #endregion Other

        public MvcHtmlString ToHtmlString()
        {
            var sb = new StringBuilder(512);

            sb.AppendFormat(@"$('#{0}').revolution({{", ControlId);
            sb.Append("lazyLoad: 'on',");
            sb.AppendFormat("delay: {0},", Delay);
            sb.AppendFormat("startheight: {0},", StartHeight);
            sb.AppendFormat("startwidth: {0},", StartWidth);

            sb.AppendFormat("keyboardNavigation: '{0}',", KeyboardNavigation ? "on" : "off");
            sb.AppendFormat("onHoverStop: '{0}',", OnHoverStop ? "on" : "off");
            sb.AppendFormat("thumbWidth: {0},", ThumbWidth);
            sb.AppendFormat("thumbHeight: {0},", ThumbHeight);
            sb.AppendFormat("thumbAmount: {0},", ThumbAmount);
            sb.AppendFormat("hideThumbs: {0},", HideThumbs);
            sb.AppendFormat("navigationType: '{0}',", NavigationType.ToString().ToLowerInvariant());
            sb.AppendFormat("navigationArrows: '{0}',", NavigationArrows.ToString().ToLowerInvariant());
            sb.AppendFormat("navigationStyle: '{0}',", ConvertNavigationStyle(NavigationStyle));
            sb.AppendFormat("navigationHAlign: '{0}',", NavigationHAlign.ToString().ToLowerInvariant());
            sb.AppendFormat("navigationVAlign: '{0}',", NavigationVAlign.ToString().ToLowerInvariant());
            sb.AppendFormat("navigationHOffset: {0},", NavigationHOffset);
            sb.AppendFormat("navigationVOffset: {0},", NavigationVOffset);
            sb.AppendFormat("soloArrowLeftHalign: '{0}',", SoloArrowLeftHAlign.ToString().ToLowerInvariant());
            sb.AppendFormat("soloArrowRightHalign: '{0}',", SoloArrowRightHAlign.ToString().ToLowerInvariant());
            sb.AppendFormat("soloArrowLeftValign: '{0}',", SoloArrowLeftVAlign.ToString().ToLowerInvariant());
            sb.AppendFormat("soloArrowRightValign: '{0}',", SoloArrowRightVAlign.ToString().ToLowerInvariant());
            sb.AppendFormat("soloArrowLeftHOffset: {0},", SoloArrowLeftHOffset);
            sb.AppendFormat("soloArrowLeftVOffset: {0},", SoloArrowLeftVOffset);
            sb.AppendFormat("soloArrowRightVOffset: {0},", SoloArrowRightVOffset);
            sb.AppendFormat("soloArrowRightVOffset: {0},", SoloArrowRightVOffset);
            sb.AppendFormat("touchenabled: '{0}',", TouchEnabled ? "on" : "off");
            sb.AppendFormat("swipe_velocity: {0},", SwipeVelocity.ToString("N1"));
            sb.AppendFormat("swipe_max_touches: {0},", SwipeMaxTouches);
            sb.AppendFormat("swipe_min_touches: {0},", SwipeMinTouches);
            sb.AppendFormat("drag_block_vertical: {0},", DragBlockVertical.ToString().ToLowerInvariant());

            sb.AppendFormat("startWithSlide: {0},", StartWithSlide);
            sb.AppendFormat("stopAtSlide: {0},", StopAtSlide);
            sb.AppendFormat("stopAfterLoops: {0},", StopAfterLoops);

            sb.AppendFormat("hideCaptionAtLimit: {0},", HideCaptionAtLimit);
            sb.AppendFormat("hideAllCaptionAtLimit: {0},", HideAllCaptionAtLimit);
            sb.AppendFormat("hideSliderAtLimit: {0},", HideSliderAtLimit);
            sb.AppendFormat("hideNavDelayOnMobile: {0},", HideNavDelayOnMobile);
            sb.AppendFormat("hideThumbsOnMobile: '{0}',", HideThumbsOnMobile ? "on" : "off");
            sb.AppendFormat("hideBulletsOnMobile: '{0}',", HideBulletsOnMobile ? "on" : "off");
            sb.AppendFormat("hideArrowsOnMobile: '{0}',", HideArrowsOnMobile ? "on" : "off");
            sb.AppendFormat("hideThumbsUnderResoluition: {0},", HideThumbsUnderResolution);

            sb.AppendFormat("spinner: '{0}',", Spinner.ToString().ToLowerInvariant());
            sb.AppendFormat("hideTimerBar: '{0}',", HideTimerBar ? "on" : "off");
            sb.AppendFormat("fullWidth: '{0}',", FullWidth ? "on" : "off");
            sb.AppendFormat("autoHeight: '{0}',", AutoHeight ? "on" : "off");
            sb.AppendFormat("minHeight: {0},", MinHeight);
            sb.AppendFormat("fullScreenAlignForce: '{0}',", FullScreenAlignForce ? "on" : "off");
            sb.AppendFormat("forceFullWidth: '{0}',", ForceFullWidth ? "on" : "off");
            sb.AppendFormat("fullScreen: '{0}',", FullScreen ? "on" : "off");

            if (!string.IsNullOrEmpty(FullScreenOffsetContainer))
            {
                sb.AppendFormat("fullScreenOffsetContainer: '{0}',", FullScreenOffsetContainer);
            }

            sb.AppendFormat("fullScreenOffset: '{0}{1}',", FullScreenOffset, FullScreenOffsetUnit == CssUnit.Percentage ? "%" : "px");
            sb.AppendFormat("shadow: {0},", ConvertShadow(Shadow));
            sb.AppendFormat("dottedOverlay: '{0}',", DottedOverlay.ToString().ToLowerInvariant());

            sb.AppendFormat("parallax: '{0}',", Parallax.ToString().ToLowerInvariant());
            sb.AppendFormat("parallaxBgFreeze: '{0}',", ParallaxBgFreeze ? "on" : "off");
            sb.AppendFormat("parallaxLevels: [{0}],", ParallaxLevels);
            sb.AppendFormat("parallaxDisableOnMobile: '{0}',", ParallaxDisableOnMobile ? "on" : "off");

            sb.AppendFormat("panZoomDisableOnMobile: '{0}',", PanZoomDisableOnMobile ? "on" : "off");

            sb.AppendFormat("simplifyAll: '{0}',", SimplifyAll ? "on" : "off");

            sb.Remove(sb.Length - 1, 1); // Remove last comma

            sb.Append("});");

            return new MvcHtmlString(sb.ToString());
        }

        private static string ConvertNavigationStyle(NavigationStyle navigationStyle)
        {
            switch (navigationStyle)
            {
                case NavigationStyle.Preview1: return "preview1";
                case NavigationStyle.Preview2: return "preview2";
                case NavigationStyle.Preview3: return "preview3";
                case NavigationStyle.Preview4: return "preview4";
                case NavigationStyle.Round: return "round";
                case NavigationStyle.Square: return "square";
                case NavigationStyle.RoundOld: return "round-old";
                case NavigationStyle.SquareOld: return "square-old";
                case NavigationStyle.NavBarOld: return "navbar-old";
                default: return "round";
            }
        }

        private static byte ConvertShadow(Shadow shadow)
        {
            switch (shadow)
            {
                case Shadow.None: return 0;
                case Shadow.Shadow1: return 1;
                case Shadow.Shadow2: return 2;
                case Shadow.Shadow3: return 3;
                default: return 0;
            }
        }

        #region ContentBlockBase Overrides

        public override string Name
        {
            get { return "Revolution Slider"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "/Plugins/Widgets.RevolutionSlider/Views/Shared/DisplayTemplates/RevolutionSliderBlock.cshtml"; }
        }

        public override string EditorTemplatePath
        {
            get { return "/Plugins/Widgets.RevolutionSlider/Views/Shared/EditorTemplates/RevolutionSliderBlock.cshtml"; }
        }

        #endregion ContentBlockBase Overrides
    }

    #region Enums

    public enum NavigationType : byte
    {
        None = 0,
        Bullet = 1,
        Thumb = 2
    }

    public enum NavigationArrows : byte
    {
        /// <summary>
        /// Arrows added next to the bullets left and right
        /// </summary>
        NextToBullets = 0,

        /// <summary>
        /// Arrows can be independently positioned, see further options
        /// </summary>
        Solo = 1
    }

    public enum NavigationStyle : byte
    {
        Preview1 = 0,
        Preview2 = 1,
        Preview3 = 2,
        Preview4 = 3,
        Round = 4,
        Square = 5,
        RoundOld = 6,
        SquareOld = 7,
        NavBarOld = 8
    }

    public enum NavigationHAlign : byte
    {
        Left = 0,
        Center = 1,
        Right = 2
    }

    public enum NavigationVAlign : byte
    {
        Top = 0,
        Center = 1,
        Bottom = 2
    }

    public enum Spinner : byte
    {
        Spinner1 = 0,
        Spinner2 = 1,
        Spinner3 = 2,
        Spinner4 = 3,
        Spinner5 = 4
    }

    public enum Shadow : byte
    {
        None = 0,
        Shadow1 = 1,
        Shadow2 = 2,
        Shadow3 = 3
    }

    public enum DottedOverlay : byte
    {
        None = 0,
        TwoXTwo = 1,
        ThreeXThree = 2,
        TwoXTwoWhite = 3,
        ThreeXThreeWhite = 4
    }

    public enum ParallaxMode : byte
    {
        Mouse = 0,
        Scroll = 1
    }

    #endregion Enums
}