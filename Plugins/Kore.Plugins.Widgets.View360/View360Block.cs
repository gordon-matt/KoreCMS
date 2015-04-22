using Kore.ComponentModel;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;

namespace Kore.Plugins.Widgets.View360
{
    public class View360Block : ContentBlockBase
    {
        public View360Block()
        {
            Mode = Mode.Fixed;
            ImagesPattern = "image-%COL-%ROW.jpg";
            AutoRotateDirection = AutoRotateDirection.Right;
            AutoRotateSpeed = 50;
            AutoRotateStopOnMove = true;
            //ZoomMultipliers = new List<float> { 1f, 1.2f, 1.5f, 2f, 3f };
            LoadFullSizeImagesOnZoom = true;
            LoadFullSizeImagesOnFullscreen = true;
            Width = 620;
            Height = 350;
            Rows = 1;
            Columns = 36;
            XAxisSensitivity = 10;
            YAxisSensitivity = 40;
            InertiaConstant = 10;

            ButtonWidth = 40;
            ButtonHeight = 40;
            ButtonMargin = 5;
            TurnSpeed = 40;
            ShowButtons = true;
            ShowTool = true;
            ShowPlay = true;
            ShowPause = true;
            ShowZoom = true;
            ShowTurn = true;
            ShowFullscreen = true;

            DisplayLoader = true;
            LoaderModalBackground = "#FFF";
            LoaderModalOpacity = 0.5f;
            LoaderCircleWidth = 70;
            LoaderCircleLineWidth = 10;
            LoaderCircleLineColor = "#555";
            LoaderCircleBackgroundColor = "#FFF";
        }

        #region General

        /// <summary>
        /// Sets visual appearance. Options:fixed, lightbox, fullview and responsive.
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/Mode")]
        public Mode Mode { get; set; }

        /// <summary>
        ///  If we have hundreds of product, we must standardize product names in order to dynamically display products without worry
        ///  about images names.
        ///  Imagine we have 36 images for full rotation: image-0-0.jpg, image-1-0.jpg, .... image-35-0.jpg
        ///  We can simply set pattern image-%COL-%ROW.jpg and component will auto load images without need to pass array of images names.
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/ImagesPattern")]
        public string ImagesPattern { get; set; }

        /// <summary>
        /// Path to images directory.
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/ImagesDirectory")]
        public string ImagesDirectory { get; set; }

        /// <summary>
        /// Path to full sized images. Tease images will be loaded product zoom.
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/FullSizeImagesDirectory")]
        public string FullSizeImagesDirectory { get; set; }

        #endregion General

        #region Main Configuration

        /// <summary>
        /// auto rotate on init
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/AutoRotate")]
        public bool AutoRotate { get; set; }

        /// <summary>
        /// auto rotate direction
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/AutoRotateDirection")]
        public AutoRotateDirection AutoRotateDirection { get; set; }

        /// <summary>
        /// auto rotate speed
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/AutoRotateSpeed")]
        public int AutoRotateSpeed { get; set; }

        /// <summary>
        /// stop auto rotation on user interaction
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/AutoRotateStopOnMove")]
        public bool AutoRotateStopOnMove { get; set; }

        //TODO: Changing this causes the viewer to be blank when zooming in and out. For now, leave the defaults until we
        //  can figure out how to fix this. It's not an important option to have anyway.
        ///// <summary>
        ///// array of zoom multipliers
        ///// </summary>
        //[LocalizedDisplayName("Kore.Plugins.Widgets.View360/ZoomMultipliers")]
        //public List<float> ZoomMultipliers { get; set; }

        /// <summary>
        /// If set to true, full size images will be loaded on first zoom. Also this property can set to one of the vaues
        /// from zoomMultipliers array. Full size images will be loaded on certan zoom multiplier.
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/LoadFullSizeImagesOnZoom")]
        public bool LoadFullSizeImagesOnZoom { get; set; }

        /// <summary>
        /// If set to true, fullscreen button starts full size images loading.
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/LoadFullSizeImagesOnFullscreen")]
        public bool LoadFullSizeImagesOnFullscreen { get; set; }

        /// <summary>
        /// View width
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/Width")]
        public short Width { get; set; }

        /// <summary>
        /// View height
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/Height")]
        public short Height { get; set; }

        /// <summary>
        /// View width
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/Rows")]
        public short Rows { get; set; }

        /// <summary>
        /// View height
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/Columns")]
        public short Columns { get; set; }

        /// <summary>
        /// Column change sensitivity in pixels
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/XAxisSensitivity")]
        public short XAxisSensitivity { get; set; }

        /// <summary>
        /// Row change sensitivity in pixels.
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/YAxisSensitivity ")]
        public short YAxisSensitivity { get; set; }

        /// <summary>
        /// Inertia rotation constant. Set 0 to disable.
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/InertiaConstant")]
        public short InertiaConstant { get; set; }

        #endregion Main Configuration

        #region Navigation Buttons Configuration

        /// <summary>
        /// button width
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/ButtonWidth")]
        public short ButtonWidth { get; set; }

        /// <summary>
        /// button height
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/ButtonHeight")]
        public short ButtonHeight { get; set; }

        /// <summary>
        /// distance between buttons
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/ButtonMargin")]
        public short ButtonMargin { get; set; }

        /// <summary>
        /// rotation speed for turnLeft and turnRight buttons
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/TurnSpeed")]
        public short TurnSpeed { get; set; }

        /// <summary>
        /// show navigation buttons
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/ShowButtons")]
        public bool ShowButtons { get; set; }

        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/ShowTool")]
        public bool ShowTool { get; set; }

        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/ShowPlay")]
        public bool ShowPlay { get; set; }

        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/ShowPause")]
        public bool ShowPause { get; set; }

        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/ShowZoom")]
        public bool ShowZoom { get; set; }

        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/ShowTurn")]
        public bool ShowTurn { get; set; }

        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/ShowFullscreen")]
        public bool ShowFullscreen { get; set; }

        #endregion Navigation Buttons Configuration

        #region Loader Info Config

        /// <summary>
        /// display loader info
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/DisplayLoader")]
        public bool DisplayLoader { get; set; }

        /// <summary>
        /// classname for css override
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/LoaderHolderClassName")]
        public string LoaderHolderClassName { get; set; }

        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/LoadingTitle")]
        public string LoadingTitle { get; set; }

        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/LoadingSubtitle")]
        public string LoadingSubtitle { get; set; }

        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/LoadingMessage")]
        public string LoadingMessage { get; set; }

        /// <summary>
        /// Color of loader background
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/LoaderModalBackground")]
        public string LoaderModalBackground { get; set; }

        /// <summary>
        /// Opacity of loader background (range between 0-1)
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/LoaderModalOpacity")]
        public float LoaderModalOpacity { get; set; }

        /// <summary>
        /// Loader circle width
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/LoaderCircleWidth")]
        public float LoaderCircleWidth { get; set; }

        /// <summary>
        /// Loader circle line width
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/LoaderCircleLineWidth")]
        public float LoaderCircleLineWidth { get; set; }

        /// <summary>
        /// Loader circle line color
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/LoaderCircleLineColor")]
        public string LoaderCircleLineColor { get; set; }

        /// <summary>
        /// Loader circle background color
        /// </summary>
        [LocalizedDisplayName("Kore.Plugins.Widgets.View360/LoaderCircleBackgroundColor")]
        public string LoaderCircleBackgroundColor { get; set; }

        #endregion Loader Info Config

        #region ContentBlockBase Overrides

        public override string Name
        {
            get { return "View360"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "/Plugins/Plugins.Widgets.View360/Views/Shared/DisplayTemplates/View360Block.cshtml"; }
        }

        public override string EditorTemplatePath
        {
            get { return "/Plugins/Plugins.Widgets.View360/Views/Shared/EditorTemplates/View360Block.cshtml"; }
        }

        #endregion ContentBlockBase Overrides
    }

    public enum Mode : byte
    {
        /// <summary>
        /// Used for in page display. This is default value.
        /// </summary>
        Fixed = 0,

        /// <summary>
        /// Product will be shown in lightbox (in page popup).
        /// </summary>
        Lightbox = 1,

        /// <summary>
        /// Product will be scaled 100% by width and height inside browser.
        /// </summary>
        FullView = 2,

        /// <summary>
        /// Depend on folder width, view will automatically change its size.
        /// </summary>
        Responsive = 3
    }

    public enum AutoRotateDirection : sbyte
    {
        Left = -1,
        Right = 1
    }
}