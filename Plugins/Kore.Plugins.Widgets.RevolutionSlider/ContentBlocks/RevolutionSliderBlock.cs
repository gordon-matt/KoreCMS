using System.Text;
using System.Web.Mvc;
using Kore.ComponentModel;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;

namespace Kore.Plugins.Widgets.RevolutionSlider.ContentBlocks
{
    public class RevolutionSliderBlock : ContentBlockBase
    {
        #region General

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.MediaFolder)]
        public string MediaFolder { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.ControlId)]
        public string ControlId { get; set; }

        /// <summary>
        /// <para>The time one slide stays on the screen in Milliseconds. Global Setting.</para>
        /// <para>You can set per Slide extra local delay time via the data-delay in the &lt;li&gt; element (Default: 9000).</para>
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.Delay)]
        public short Delay { get; set; }

        /// <summary>
        /// <para>This Height of the Grid where the Captions are displayed in Pixel. This Height is the Max height of Slider in</para>
        /// <para>Fullwidth Layout and in Responsive Layout. In Fullscreen Layout the Gird will be centered Vertically in case</para>
        /// <para>the Slider is higher then this value.</para>
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.StartHeight)]
        public short StartHeight { get; set; }

        /// <summary>
        /// <para>This Height of the Grid where the Captions are displayed in Pixel. This Width is the Max Width of Slider in</para>
        /// <para>Responsive Layout.  In Fullscreen and in FullWidth Layout the Gird will be centered Horizontally in case the</para>
        /// <para>Slider is wider then this value.</para>
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.StartWidth)]
        public short StartWidth { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.Shuffle)]
        public bool Shuffle { get; set; }

        #endregion General

        #region Navigation

        /// <summary>
        /// Possible Values: "on", "off" - Allows to use the Left/Right Arrow for Keyboard navigation when Slider is in Focus.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.KeyboardNavigation)]
        public bool KeyboardNavigation { get; set; }

        /// <summary>
        /// <para>Possible Values: "on", "off" - Stop the Timer if mouse is hovering the Slider.</para>
        /// <para>Caption animations are not stopped !! They will just play to the end.</para>
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.OnHoverStop)]
        public bool OnHoverStop { get; set; }

        /// <summary>
        /// <para>The width of the thumbs in pixels. Thumbs are not responsive from basic.</para>
        /// <para>For Responsive Thumbs you will need to create media query based thumb sizes.</para>
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.ThumbWidth)]
        public short ThumbWidth { get; set; }

        /// <summary>
        /// <para>The height of the thumbs in pixels. Thumbs are not responsive from basic.</para>
        /// <para>For Responsive Thumbs you will need to create media query based thumb sizes.</para>
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.ThumbHeight)]
        public short ThumbHeight { get; set; }

        /// <summary>
        /// The Amount of visible Thumbs in the same time. The rest of the thumbs are only visible on hover, or at slide change.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.ThumbAmount)]
        public byte ThumbAmount { get; set; }

        /// <summary>
        /// <para>0 - Never hide Thumbs.  1000- 100000 (ms) hide thumbs and navigation arrows, bullets after the predefined</para>
        /// <para>ms (1000ms == 1 sec,  1500 == 1,5 sec etc..)</para>
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.HideThumbs)]
        public short HideThumbs { get; set; }

        /// <summary>
        /// Display type of the "bullet/thumbnail" bar (Default:"none")
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.NavigationType)]
        public NavigationType NavigationType { get; set; }

        /// <summary>
        /// Display position of the Navigation Arrows (Default: "nexttobullets")
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.NavigationArrows)]
        public NavigationArrows NavigationArrows { get; set; }

        /// <summary>
        /// Look of the navigation bullets if navigationType "bullet" selected.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.NavigationStyle)]
        public NavigationStyle NavigationStyle { get; set; }

        /// <summary>
        /// Horizontal Align of the Navigation bullets / thumbs (depending on which Style has been selected).
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.NavigationHAlign)]
        public NavigationHAlign NavigationHAlign { get; set; }

        /// <summary>
        /// Vertical Align of the Navigation bullets / thumbs (depending on which Style has been selected).
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.NavigationVAlign)]
        public NavigationVAlign NavigationVAlign { get; set; }

        /// <summary>
        /// The Offset position of the navigation depending on the aligned position. From -1000 to +1000 any value in px. i.e. -30
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.NavigationHOffset)]
        public short NavigationHOffset { get; set; }

        /// <summary>
        /// The Offset position of the navigation depending on the aligned position. From -1000 to +1000 any value in px. i.e. -30
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.NavigationVOffset)]
        public short NavigationVOffset { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.SoloArrowLeftHAlign)]
        public NavigationHAlign SoloArrowLeftHAlign { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.SoloArrowRightHAlign)]
        public NavigationHAlign SoloArrowRightHAlign { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.SoloArrowLeftVAlign)]
        public NavigationVAlign SoloArrowLeftVAlign { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.SoloArrowRightVAlign)]
        public NavigationVAlign SoloArrowRightVAlign { get; set; }

        /// <summary>
        /// <para>The Offset position of the navigation depending on the aligned position. From -1000 to +1000 any value in px. i.e. -30.</para>
        /// <para>Each Arrow independent.</para>
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.SoloArrowLeftHOffset)]
        public short SoloArrowLeftHOffset { get; set; }

        /// <summary>
        /// <para>The Offset position of the navigation depending on the aligned position. From -1000 to +1000 any value in px. i.e. -30.</para>
        /// <para>Each Arrow independent.</para>
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.SoloArrowRightHOffset)]
        public short SoloArrowRightHOffset { get; set; }

        /// <summary>
        /// <para>The Offset position of the navigation depending on the aligned position. From -1000 to +1000 any value in px. i.e. -30.</para>
        /// <para>Each Arrow independent.</para>
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.SoloArrowLeftVOffset)]
        public short SoloArrowLeftVOffset { get; set; }

        /// <summary>
        /// <para>The Offset position of the navigation depending on the aligned position. From -1000 to +1000 any value in px. i.e. -30.</para>
        /// <para>Each Arrow independent.</para>
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.SoloArrowRightVOffset)]
        public short SoloArrowRightVOffset { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.TouchEnabled)]
        public bool TouchEnabled { get; set; }

        /// <summary>
        /// The Sensibility of Swipe Gesture (lower is more sensible) (Default: 0.7). Possible values: 0 - 1
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.SwipeVelocity)]
        public float SwipeVelocity { get; set; }

        /// <summary>
        /// Max Amount of Fingers to touch (Default: 1). Possible values: 1 - 5.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.SwipeMaxTouches)]
        public byte SwipeMaxTouches { get; set; }

        /// <summary>
        /// Min Amount of Fingers to touch (Default: 1). Possible values: 1 - 5.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.SwipeMinTouches)]
        public byte SwipeMinTouches { get; set; }

        /// <summary>
        /// Prevent Vertical Scroll on Drag (Default: false)
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.DragBlockVertical)]
        public bool DragBlockVertical { get; set; }

        #endregion Navigation

        #region Loops

        /// <summary>
        /// Start with a Predefined Slide (index of the slide).
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.StartWithSlide)]
        public byte StartWithSlide { get; set; }

        /// <summary>
        /// <para>Stop Timer if Slide "x" has been Reached. If stopAfterLoops set to 0, then it stops already in the first</para>
        /// <para>loop at slide X which defined. -1 means do not stop at any slide. stopAfterLoops has no sinn in this case.</para>
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.StopAtSlide)]
        public sbyte StopAtSlide { get; set; }

        /// <summary>
        /// <para>Stop Timer if All slides has been played "x" times. IT will stop at THe slide which is defined via stopAtSlide:x,</para>
        /// <para>if set to -1 slide never stop automatic.</para>
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.StopAfterLoops)]
        public sbyte StopAfterLoops { get; set; }

        #endregion Loops

        #region Mobile Visibility

        /// <summary>
        /// It Defines if a caption should be shown under a Screen Resolution ( Basod on The Width of Browser).
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.HideCaptionAtLimit)]
        public short HideCaptionAtLimit { get; set; }

        /// <summary>
        /// Hide all The Captions if Width of Browser is less then this value.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.HideAllCaptionAtLimit)]
        public short HideAllCaptionAtLimit { get; set; }

        /// <summary>
        /// Hide the whole slider, and stop also functions if Width of Browser is less than this value.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.HideSliderAtLimit)]
        public short HideSliderAtLimit { get; set; }

        /// <summary>
        /// Hide all navigation after a while on Mobile (touch and release events based).
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.HideNavDelayOnMobile)]
        public short HideNavDelayOnMobile { get; set; }

        /// <summary>
        /// Possible Values: "on", "off"  - if set to "on", Thumbs are not shown on Mobile Devices.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.HideThumbsOnMobile)]
        public bool HideThumbsOnMobile { get; set; }

        /// <summary>
        /// Possible Values: "on", "off"  - if set to "on", Bullets are not shown on Mobile Devices.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.HideBulletsOnMobile)]
        public bool HideBulletsOnMobile { get; set; }

        /// <summary>
        /// Possible Values: "on", "off"  - if set to "on", Arrows are not shown on Mobile Devices.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.HideArrowsOnMobile)]
        public bool HideArrowsOnMobile { get; set; }

        /// <summary>
        /// Possible Values: 0 - 1900. Defines under which resolution the Thumbs should be hidden. (only if hideThumbonMobile set to off.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.HideThumbsUnderResolution)]
        public short HideThumbsUnderResolution { get; set; }

        #endregion Mobile Visibility

        #region Layout Styles

        /// <summary>
        /// The Layout of Loader. If not defined, it will use the basic spinner.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.Spinner)]
        public Spinner Spinner { get; set; }

        /// <summary>
        /// It will hide or show the banner timer
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.HideTimerBar)]
        public bool HideTimerBar { get; set; }

        /// <summary>
        /// Defines if the fullwidth/autoresponsive mode is activated
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.FullWidth)]
        public bool FullWidth { get; set; }

        /// <summary>
        /// <para>Defines if in fullwidth mode the height of the Slider proportional always can grow. If it is set to "off" the</para>
        /// <para>max height is == startheight.</para>
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.AutoHeight)]
        public bool AutoHeight { get; set; }

        /// <summary>
        /// <para>Defines the min height of the Slider. The Slider container height will never be smaller than this value.</para>
        /// <para>The Content container is still shrinks linear to the browser width and original width of Container, and will</para>
        /// <para>be centered vertically in the Slider.</para>
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.MinHeight)]
        public short MinHeight { get; set; }

        /// <summary>
        /// <para>Allowed only in FullScreen Mode. It lets the Caption Grid to be the full screen, means all position should happen with</para>
        /// <para>aligns and offsets. This allow you to use the faresst corner of the slider to present any caption there.</para>
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.FullScreenAlignForce)]
        public bool FullScreenAlignForce { get; set; }

        /// <summary>
        /// <para>Force the FullWidth Size even if the slider embeded in a boxed container. It can provide higher Performance usage on CPU.</para>
        /// <para>Try first set it to "off" and use fullwidth container instead of using this option.</para>
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.ForceFullWidth)]
        public bool ForceFullWidth { get; set; }

        /// <summary>
        /// Defines if the fullscreen mode is activated
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.FullScreen)]
        public bool FullScreen { get; set; }

        /// <summary>
        /// The value is a Container ID or Class i.e. "#topheader" - The Height of Fullheight will be increased with this Container height!
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.FullScreenOffsetContainer)]
        public string FullScreenOffsetContainer { get; set; }

        /// <summary>
        /// A px or % value which will be added/reduced of the Full Height Container calculation.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.FullScreenOffset)]
        public short FullScreenOffset { get; set; }

        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.FullScreenOffsetUnit)]
        public ScreenUnit FullScreenOffsetUnit { get; set; }

        /// <summary>
        /// Possible values: 0,1,2,3 (0 == no Shadow, 1,2,3 - Different Shadow Types).
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.Shadow)]
        public Shadow Shadow { get; set; }

        /// <summary>
        /// <para>Creates a Dotted Overlay for the Background images extra. Best use for FullScreen / fullwidth sliders, where images</para>
        /// <para>are too pixaleted.</para>
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.DottedOverlay)]
        public DottedOverlay DottedOverlay { get; set; }

        #endregion Layout Styles

        #region Parallax

        /// <summary>
        /// How the Parallax should act. On Mouse Hover movements and Tilts on Mobile Devices, or on Scroll (scroll is disabled on Mobiles!)
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.Parallax)]
        public ParallaxMode Parallax { get; set; }

        /// <summary>
        /// If it is off, the Main slide images will also move with Parallax Level 1 on Scroll.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.ParallaxBgFreeze)]
        public bool ParallaxBgFreeze { get; set; }

        /// <summary>
        /// An array which defines the Parallax depths (0 - 10). Depending on the value it will define the Strength of the Parallax
        /// offsets on mousemove or scroll. In Layers you can use the class like rs-parallaxlevel-1 for the different levels.
        /// If one tp-caption layer has rs-parallaxlevel-X (x 1-10) then it activates the Parallax movements on that layer.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.ParallaxLevels)]
        public string ParallaxLevels { get; set; }

        /// <summary>
        /// Turn on/ off Parallax Effect on Mobile Devices.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.ParallaxDisableOnMobile)]
        public bool ParallaxDisableOnMobile { get; set; }

        #endregion Parallax

        #region Pan Zoom

        /// <summary>
        /// Turn on/ off Pan Zoom Effects on Mobile Devices.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.PanZoomDisableOnMobile)]
        public bool PanZoomDisableOnMobile { get; set; }

        #endregion Pan Zoom

        #region Other

        /// <summary>
        /// Set all Animation on older Browsers like IE8 and iOS4 Safari to Fade, without splitting letters to save performance.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.RevolutionSliderBlock.SimplifyAll)]
        public bool SimplifyAll { get; set; }

        #endregion Other

        public MvcHtmlString ToHtmlString()
        {
            var sb = new StringBuilder(512);

            sb.AppendFormat(@"$('#{0}').revolution({{", ControlId);
            sb.AppendFormat("delay : {0},", Delay);
            sb.AppendFormat("startheight : {0},", StartHeight);
            sb.AppendFormat("startwidth : {0},", StartWidth);

            sb.AppendFormat("keyboardNavigation : '{0}',", KeyboardNavigation ? "on" : "off");
            sb.AppendFormat("onHoverStop : '{0}',", OnHoverStop ? "on" : "off");
            sb.AppendFormat("thumbWidth : {0},", ThumbWidth);
            sb.AppendFormat("thumbHeight : {0},", ThumbHeight);
            sb.AppendFormat("thumbAmount : {0},", ThumbAmount);
            sb.AppendFormat("hideThumbs : {0},", HideThumbs);
            sb.AppendFormat("navigationType : '{0}',", NavigationType.ToString().ToLowerInvariant());
            sb.AppendFormat("navigationArrows : '{0}',", NavigationArrows.ToString().ToLowerInvariant());
            sb.AppendFormat("navigationStyle : '{0}',", ConvertNavigationStyle(NavigationStyle));
            sb.AppendFormat("navigationHAlign : '{0}',", NavigationHAlign.ToString().ToLowerInvariant());
            sb.AppendFormat("navigationVAlign : '{0}',", NavigationVAlign.ToString().ToLowerInvariant());
            sb.AppendFormat("navigationHOffset : {0},", NavigationHOffset);
            sb.AppendFormat("navigationVOffset : {0},", NavigationVOffset);
            sb.AppendFormat("soloArrowLeftHalign : '{0}',", SoloArrowLeftHAlign.ToString().ToLowerInvariant());
            sb.AppendFormat("soloArrowRightHalign : '{0}',", SoloArrowRightHAlign.ToString().ToLowerInvariant());
            sb.AppendFormat("soloArrowLeftValign : '{0}',", SoloArrowLeftVAlign.ToString().ToLowerInvariant());
            sb.AppendFormat("soloArrowRightValign : '{0}',", SoloArrowRightVAlign.ToString().ToLowerInvariant());
            sb.AppendFormat("soloArrowLeftHOffset : {0},", SoloArrowLeftHOffset);
            sb.AppendFormat("soloArrowLeftVOffset : {0},", SoloArrowLeftVOffset);
            sb.AppendFormat("soloArrowRightVOffset : {0},", SoloArrowRightVOffset);
            sb.AppendFormat("soloArrowRightVOffset : {0},", SoloArrowRightVOffset);
            sb.AppendFormat("touchenabled : '{0}',", TouchEnabled ? "on" : "off");
            sb.AppendFormat("swipe_velocity : {0},", SwipeVelocity.ToString("N1"));
            sb.AppendFormat("swipe_max_touches : {0},", SwipeMaxTouches);
            sb.AppendFormat("swipe_min_touches : {0},", SwipeMinTouches);
            sb.AppendFormat("drag_block_vertical : {0},", DragBlockVertical.ToString().ToLowerInvariant());

            sb.AppendFormat("startWithSlide : {0},", StartWithSlide);
            sb.AppendFormat("stopAtSlide : {0},", StopAtSlide);
            sb.AppendFormat("stopAfterLoops : {0},", StopAfterLoops);

            sb.AppendFormat("hideCaptionAtLimit : {0},", HideCaptionAtLimit);
            sb.AppendFormat("hideAllCaptionAtLimit : {0},", HideAllCaptionAtLimit);
            sb.AppendFormat("hideSliderAtLimit : {0},", HideSliderAtLimit);
            sb.AppendFormat("hideNavDelayOnMobile : {0},", HideNavDelayOnMobile);
            sb.AppendFormat("hideThumbsOnMobile : '{0}',", HideThumbsOnMobile ? "on" : "off");
            sb.AppendFormat("hideBulletsOnMobile : '{0}',", HideBulletsOnMobile ? "on" : "off");
            sb.AppendFormat("hideArrowsOnMobile : '{0}',", HideArrowsOnMobile ? "on" : "off");
            sb.AppendFormat("hideThumbsUnderResoluition : {0},", HideThumbsUnderResolution);

            sb.AppendFormat("spinner : '{0}',", Spinner.ToString().ToLowerInvariant());
            sb.AppendFormat("hideTimerBar : '{0}',", HideTimerBar ? "on" : "off");
            sb.AppendFormat("fullWidth : '{0}',", FullWidth ? "on" : "off");
            sb.AppendFormat("autoHeight : '{0}',", AutoHeight ? "on" : "off");
            sb.AppendFormat("minHeight : {0},", MinHeight);
            sb.AppendFormat("fullScreenAlignForce : '{0}',", FullScreenAlignForce ? "on" : "off");
            sb.AppendFormat("forceFullWidth : '{0}',", ForceFullWidth ? "on" : "off");
            sb.AppendFormat("fullScreen : '{0}',", FullScreen ? "on" : "off");

            if (!string.IsNullOrEmpty(FullScreenOffsetContainer))
            {
                sb.AppendFormat("fullScreenOffsetContainer : '{0}',", FullScreenOffsetContainer);
            }

            sb.AppendFormat("fullScreenOffset : '{0}{1}',", FullScreenOffset, FullScreenOffsetUnit == ScreenUnit.Percentage ? "%" : "px");
            sb.AppendFormat("shadow : {0},", ConvertShadow(Shadow));
            sb.AppendFormat("dottedOverlay : '{0}',", DottedOverlay.ToString().ToLowerInvariant());

            sb.AppendFormat("parallax : '{0}',", Parallax.ToString().ToLowerInvariant());
            sb.AppendFormat("parallaxBgFreeze : '{0}',", ParallaxBgFreeze ? "on" : "off");
            sb.AppendFormat("parallaxLevels : [{0}],", ParallaxLevels);
            sb.AppendFormat("parallaxDisableOnMobile : '{0}',", ParallaxDisableOnMobile ? "on" : "off");

            sb.AppendFormat("panZoomDisableOnMobile : '{0}',", PanZoomDisableOnMobile ? "on" : "off");

            sb.AppendFormat("simplifyAll : '{0}',", SimplifyAll ? "on" : "off");

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

    public enum ScreenUnit : byte
    {
        Pixels = 0,
        Percentage = 1
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