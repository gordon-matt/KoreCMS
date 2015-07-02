using System.Text;
using System.Web.Mvc;
using Kore.ComponentModel;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;

namespace Kore.Plugins.Widgets.OwlCarousel.ContentBlocks
{
    public class OwlCarouselBlock : ContentBlockBase
    {
        public OwlCarouselBlock()
        {
            Items = 5;
            ItemsDesktop = "[1199,4]";
            ItemsDesktopSmall = "[979,3]";
            ItemsTablet = "[768,2]";
            ItemsTabletSmall = "false";
            ItemsMobile = "[479,1]";
            ItemsCustom = "false";
            SlideSpeed = 200;
            PaginationSpeed = 800;
            RewindSpeed = 1000;
            AutoPlaySpeed = 5000;
            NavigationText = @"[""prev"",""next""]";
            RewindNav = true;
            Pagination = true;
            Responsive = true;
            ResponsiveRefreshRate = 200;
            ResponsiveBaseWidth = "window";
            BaseClass = "owl-carousel";
            Theme = "owl-theme";
            LazyFollow = true;
            LazyEffect = "fade";
            DragBeforeAnimFinish = true;
            MouseDrag = true;
            TouchDrag = true;
        }

        #region General

        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.MediaFolder)]
        public string MediaFolder { get; set; }

        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.ControlId)]
        public string ControlId { get; set; }

        /// <summary>
        /// Slide speed in milliseconds.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.SlideSpeed)]
        public int SlideSpeed { get; set; }

        /// <summary>
        /// Pagination speed in milliseconds.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.PaginationSpeed)]
        public int PaginationSpeed { get; set; }

        /// <summary>
        /// Rewind speed in milliseconds.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.RewindSpeed)]
        public int RewindSpeed { get; set; }

        /// <summary>
        /// If you set autoPlay: true default speed will be 5 seconds.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.AutoPlay)]
        public bool AutoPlay { get; set; }

        /// <summary>
        /// Change to any integer for example 5000 to play every 5 seconds.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.AutoPlaySpeed)]
        public int AutoPlaySpeed { get; set; }

        /// <summary>
        /// Stop autoplay on mouse hover.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.StopOnHover)]
        public bool StopOnHover { get; set; }

        /// <summary>
        /// Display "next" and "prev" buttons.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.Navigation)]
        public bool Navigation { get; set; }

        /// <summary>
        /// You can cusomize your own text for navigation. To get empty buttons use navigationText : false. Also HTML can be used here.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.NavigationText)]
        public string NavigationText { get; set; }

        /// <summary>
        /// Slide to first item. Use rewindSpeed to change animation speed.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.RewindNav)]
        public bool RewindNav { get; set; }

        /// <summary>
        /// Scroll per page not per item. This affect next/prev buttons and mouse/touch dragging.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.ScrollPerPage)]
        public bool ScrollPerPage { get; set; }

        /// <summary>
        /// Show pagination.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.Pagination)]
        public bool Pagination { get; set; }

        /// <summary>
        /// Show numbers inside pagination buttons.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.PaginationNumbers)]
        public bool PaginationNumbers { get; set; }

        /// <summary>
        /// You can use Owl Carousel on desktop-only websites too! Just change that to "false" to disable resposive capabilities.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.Responsive)]
        public bool Responsive { get; set; }

        /// <summary>
        /// Check window width changes every (n)ms for responsive actions.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.ResponsiveRefreshRate)]
        public int ResponsiveRefreshRate { get; set; }

        /// <summary>
        /// <para>Owl Carousel check window for browser width changes. You can use any other jQuery element to check</para>
        /// <para>width changes for example ".owl-demo". Owl will change only if ".owl-demo" get new width.</para>
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.ResponsiveBaseWidth)]
        public string ResponsiveBaseWidth { get; set; }

        /// <summary>
        /// Automaticly added class for base CSS styles. Best not to change it if you don't need to.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.BaseClass)]
        public string BaseClass { get; set; }

        /// <summary>
        /// Default Owl CSS styles for navigation and buttons. Change it to match your own theme.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.Theme)]
        public string Theme { get; set; }

        /// <summary>
        /// <para>Delays loading of images. Images outside of viewport won't be loaded before user scrolls to them</para>
        /// <para>Great for mobile devices to speed up page loadings. IMG need special markup class="lazyOwl"</para>
        /// <para>and data-src="your img path".</para>
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.LazyLoad)]
        public bool LazyLoad { get; set; }

        /// <summary>
        /// Add height to owl-wrapper-outer so you can use diffrent heights on slides. Use it only for one item per page setting.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.AutoHeight)]
        public bool AutoHeight { get; set; }

        /// <summary>
        /// Turn off/on mouse events.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.MouseDrag)]
        public bool MouseDrag { get; set; }

        /// <summary>
        /// Turn off/on touch events.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.TouchDrag)]
        public bool TouchDrag { get; set; }

        #endregion General

        #region Item Options

        /// <summary>
        /// This variable allows you to set the maximum amount of items displayed at a time with the widest browser width.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.Items)]
        public int Items { get; set; }

        /// <summary>
        /// <para>This allows you to preset the number of slides visible with a particular browser width.</para>
        /// <para>The format is [x,y] whereby x=browser width and y=number of slides displayed.</para>
        /// <para>For example [1199,4] means that if(window&lt;=1199){ show 4 slides per page}.</para>
        /// <para>Alternatively use itemsDesktop: false to override these settings.</para>
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.ItemsDesktop)]
        public string ItemsDesktop { get; set; }

        /// <summary>
        /// Same as ItemsDesktop.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.ItemsDesktopSmall)]
        public string ItemsDesktopSmall { get; set; }

        /// <summary>
        /// Same as ItemsDesktop.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.ItemsTablet)]
        public string ItemsTablet { get; set; }

        /// <summary>
        /// Same as ItemsDesktop.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.ItemsTabletSmall)]
        public string ItemsTabletSmall { get; set; }

        /// <summary>
        /// Same as ItemsDesktop
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.ItemsMobile)]
        public string ItemsMobile { get; set; }

        /// <summary>
        /// <para>This allow you to add custom variations of items depending from the width If this option is set, itemsDeskop,</para>
        /// <para>itemsDesktopSmall, itemsTablet, itemsMobile etc. are disabled For better preview, order the arrays by screen size,</para>
        /// <para>but it's not mandatory Don't forget to include the lowest available screen size, otherwise it will take the default</para>
        /// <para>one for screens lower than lowest available.</para>
        /// <para>Example: [[0, 2], [400, 4], [700, 6], [1000, 8], [1200, 10], [1600, 16]]</para>
        /// <para>For more information about structure of the internal arrays see ItemsDesktop.</para>
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.ItemsCustom)]
        public string ItemsCustom { get; set; }

        /// <summary>
        /// Display only one item.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.SingleItem)]
        public bool SingleItem { get; set; }

        /// <summary>
        /// Option to not stretch items when it is less than the supplied items.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.ItemsScaleUp)]
        public bool ItemsScaleUp { get; set; }

        #endregion Item Options

        #region Advanced

        /// <summary>
        /// <para>Allows you to load directly from a jSon file. The JSON structure you use needs to match the owl JSON structure.</para>
        /// <para>To use custom JSON structure see jsonSuccess option.</para>
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.JsonPath)]
        public string JsonPath { get; set; }

        /// <summary>
        /// Success callback for $.getJSON build in into carousel.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.JsonSuccess)]
        public string JsonSuccess { get; set; }

        /// <summary>
        /// <para>When pagination used, it skips loading the images from pages that got skipped. It only loads the images</para>
        /// <para>that get displayed in viewport. If set to false, all images get loaded when pagination used. It is a sub</para>
        /// <para>setting of the lazy load function.</para>
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.LazyFollow)]
        public bool LazyFollow { get; set; }

        /// <summary>
        /// Default is fadeIn on 400ms speed. Use false to remove that effect.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.LazyEffect)]
        public string LazyEffect { get; set; }

        /// <summary>
        /// Ignore whether a transition is done or not (only dragging).
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.DragBeforeAnimFinish)]
        public bool DragBeforeAnimFinish { get; set; }

        /// <summary>
        /// Add "active" classes on visible items. Works with any numbers of items on screen.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.AddClassActive)]
        public bool AddClassActive { get; set; }

        /// <summary>
        /// Add CSS3 transition style. Works only with one item on screen.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.TransitionStyle)]
        public string TransitionStyle { get; set; }

        #endregion Advanced

        #region Callbacks

        /// <summary>
        /// Before responsive update callback.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.BeforeUpdate)]
        public string BeforeUpdate { get; set; }

        /// <summary>
        /// After responsive update callback.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.AfterUpdate)]
        public string AfterUpdate { get; set; }

        /// <summary>
        /// Before initialization callback.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.BeforeInit)]
        public string BeforeInit { get; set; }

        /// <summary>
        /// After initialization callback.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.AfterInit)]
        public string AfterInit { get; set; }

        /// <summary>
        /// Before move callback.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.BeforeMove)]
        public string BeforeMove { get; set; }

        /// <summary>
        /// After move callback.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.AfterMove)]
        public string AfterMove { get; set; }

        /// <summary>
        /// After startup, move and update callback.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.AfterAction)]
        public string AfterAction { get; set; }

        /// <summary>
        /// Call this function while dragging.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.StartDragging)]
        public string StartDragging { get; set; }

        /// <summary>
        /// Call this function after lazyLoad.
        /// </summary>
        [LocalizedDisplayName(LocalizableStrings.OwlCarouselBlock.AfterLazyLoad)]
        public string AfterLazyLoad { get; set; }

        #endregion Callbacks

        public MvcHtmlString ToHtmlString()
        {
            var sb = new StringBuilder(512);

            sb.AppendFormat(@"$('#{0}').owlCarousel({{", ControlId);

            if (Items != 5)
            {
                sb.AppendFormat("items: {0},", Items);
            }
            if (!string.IsNullOrWhiteSpace(ItemsDesktop) && ItemsDesktop != "[1199,4]")
            {
                sb.AppendFormat("itemsDesktop : {0},", ItemsDesktop);
            }
            if (!string.IsNullOrWhiteSpace(ItemsDesktopSmall) && ItemsDesktop != "[979,3]")
            {
                sb.AppendFormat("itemsDesktopSmall : {0},", ItemsDesktopSmall);
            }
            if (!string.IsNullOrWhiteSpace(ItemsTablet) && ItemsDesktop != "[768,2]")
            {
                sb.AppendFormat("itemsTablet : {0},", ItemsTablet);
            }
            if (!string.IsNullOrWhiteSpace(ItemsTabletSmall) && ItemsDesktop != "false")
            {
                sb.AppendFormat("itemsTabletSmall : {0},", ItemsTabletSmall);
            }
            if (!string.IsNullOrWhiteSpace(ItemsMobile) && ItemsDesktop != "[479,1]")
            {
                sb.AppendFormat("itemsMobile : {0},", ItemsMobile);
            }
            if (!string.IsNullOrWhiteSpace(ItemsCustom) && ItemsDesktop != "false")
            {
                sb.AppendFormat("itemsCustom: {0},", ItemsCustom);
            }
            if (SingleItem)
            {
                sb.Append("singleItem: true,");
            }
            if (ItemsScaleUp)
            {
                sb.Append("itemsScaleUp: true,");
            }
            if (SlideSpeed != 200)
            {
                sb.AppendFormat("slideSpeed: {0},", SlideSpeed);
            }
            if (PaginationSpeed != 800)
            {
                sb.AppendFormat("paginationSpeed: {0},", PaginationSpeed);
            }
            if (RewindSpeed != 1000)
            {
                sb.AppendFormat("rewindSpeed: {0},", RewindSpeed);
            }
            if (AutoPlay && AutoPlaySpeed != 5000)
            {
                sb.AppendFormat("autoPlay: {0},", AutoPlaySpeed);
            }
            if (StopOnHover)
            {
                sb.Append("stopOnHover: true,");
            }
            if (Navigation)
            {
                sb.Append("navigation: true,");
            }
            if (!string.IsNullOrWhiteSpace(NavigationText) && NavigationText != @"[""prev"",""next""]")
            {
                sb.AppendFormat("navigationText: {0},", NavigationText);
            }
            if (!RewindNav)
            {
                sb.Append("rewindNav: false,");
            }
            if (ScrollPerPage)
            {
                sb.Append("scrollPerPage: true,");
            }
            if (!Pagination)
            {
                sb.Append("pagination: false,");
            }
            if (PaginationNumbers)
            {
                sb.Append("paginationNumbers: true,");
            }
            if (!Responsive)
            {
                sb.Append("responsive: false,");
            }
            if (ResponsiveRefreshRate != 200)
            {
                sb.AppendFormat("responsiveRefreshRate: {0},", ResponsiveRefreshRate);
            }
            if (!string.IsNullOrWhiteSpace(ResponsiveBaseWidth) && ResponsiveBaseWidth != "window")
            {
                sb.AppendFormat("responsiveBaseWidth: '{0}',", ResponsiveBaseWidth);
            }
            if (!string.IsNullOrWhiteSpace(BaseClass) && BaseClass != "owl-carousel")
            {
                sb.AppendFormat("baseClass: '{0}',", BaseClass);
            }
            if (!string.IsNullOrWhiteSpace(Theme) && Theme != "owl-theme")
            {
                sb.AppendFormat("theme: '{0}',", Theme);
            }
            if (LazyLoad)
            {
                sb.Append("lazyLoad: true,");
            }
            if (!LazyFollow)
            {
                sb.Append("lazyLoad: false,");
            }
            if (!string.IsNullOrWhiteSpace(LazyEffect) && LazyEffect != "fade")
            {
                sb.AppendFormat("lazyEffect: '{0}',", LazyEffect);
            }
            if (AutoHeight)
            {
                sb.Append("autoHeight: true,");
            }
            if (!string.IsNullOrWhiteSpace(JsonPath) && JsonPath != "false")
            {
                sb.AppendFormat("jsonPath: '{0}',", JsonPath);
            }
            if (!string.IsNullOrWhiteSpace(JsonSuccess) && JsonSuccess != "false")
            {
                sb.AppendFormat("jsonSuccess: function() {{ {0} }},", JsonSuccess);
            }
            if (!DragBeforeAnimFinish)
            {
                sb.Append("dragBeforeAnimFinish: false,");
            }
            if (MouseDrag)
            {
                sb.Append("mouseDrag: true,");
            }
            if (TouchDrag)
            {
                sb.Append("touchDrag: true,");
            }
            if (!AddClassActive)
            {
                sb.Append("addClassActive: false,");
            }
            if (!string.IsNullOrWhiteSpace(TransitionStyle) && TransitionStyle != "false")
            {
                sb.AppendFormat("transitionStyle: {0},", TransitionStyle);
            }

            if (!string.IsNullOrWhiteSpace(BeforeUpdate) && BeforeUpdate != "false")
            {
                sb.AppendFormat("beforeUpdate: function() {{ {0} }},", BeforeUpdate);
            }
            if (!string.IsNullOrWhiteSpace(AfterUpdate) && AfterUpdate != "false")
            {
                sb.AppendFormat("afterUpdate: function() {{ {0} }},", AfterUpdate);
            }
            if (!string.IsNullOrWhiteSpace(BeforeInit) && BeforeInit != "false")
            {
                sb.AppendFormat("beforeInit: function() {{ {0} }},", BeforeInit);
            }
            if (!string.IsNullOrWhiteSpace(AfterInit) && AfterInit != "false")
            {
                sb.AppendFormat("afterInit: function() {{ {0} }},", AfterInit);
            }
            if (!string.IsNullOrWhiteSpace(BeforeMove) && BeforeMove != "false")
            {
                sb.AppendFormat("beforeMove: function() {{ {0} }},", BeforeMove);
            }
            if (!string.IsNullOrWhiteSpace(AfterMove) && AfterMove != "false")
            {
                sb.AppendFormat("afterMove: function() {{ {0} }},", AfterMove);
            }
            if (!string.IsNullOrWhiteSpace(AfterAction) && AfterAction != "false")
            {
                sb.AppendFormat("afterAction: function() {{ {0} }},", AfterAction);
            }
            if (!string.IsNullOrWhiteSpace(StartDragging) && StartDragging != "false")
            {
                sb.AppendFormat("startDragging: function() {{ {0} }},", StartDragging);
            }
            if (!string.IsNullOrWhiteSpace(AfterLazyLoad) && AfterLazyLoad != "false")
            {
                sb.AppendFormat("afterLazyLoad: function() {{ {0} }},", AfterLazyLoad);
            }

            sb.Remove(sb.Length - 1, 1); // Remove last comma

            sb.Append("});");

            return new MvcHtmlString(sb.ToString());
        }

        #region ContentBlockBase Overrides

        public override string Name
        {
            get { return "Owl Carousel"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "/Plugins/Widgets.OwlCarousel/Views/Shared/DisplayTemplates/OwlCarouselBlock.cshtml"; }
        }

        public override string EditorTemplatePath
        {
            get { return "/Plugins/Widgets.OwlCarousel/Views/Shared/EditorTemplates/OwlCarouselBlock.cshtml"; }
        }

        #endregion ContentBlockBase Overrides
    }
}