using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Plugins.Widgets.RevolutionSlider.ContentBlocks;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Html;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.RevolutionSlider.Data.Domain
{
    public class RevolutionLayer : IEntity
    {
        public int Id { get; set; }

        public int SlideId { get; set; }

        #region General

        public string CaptionText { get; set; }

        /// <summary>
        /// The timepoint in millisecond when/at the Caption should move in to the slide.
        /// </summary>
        public short Start { get; set; }

        /// <summary>
        /// The speed in milliseconds of the transition to move the Caption in the Slide at the defined timepoint.
        /// </summary>
        public short Speed { get; set; }

        /// <summary>
        /// <para>Possible Values are "left", "center", "right", or any Value between -2500  and 2500.</para>
        /// <para>If left/center/right is set, the caption will be simple aligned to the position.</para>
        /// <para>Any other "number" will simple set the left position in px of tha caption.</para>
        /// </summary>
        public NavigationHAlign X { get; set; }

        /// <summary>
        /// <para>Possible Values are "top", "center", "bottom", or any Value between -2500  and 2500.</para>
        /// <para>If top/center/bottom is set, the caption will be siple aligned to the position.</para>
        /// <para>Any other "number" will simple set the top position in px of tha caption.</para>
        /// </summary>
        public NavigationVAlign Y { get; set; }

        /// <summary>
        /// <para>(like "big_white", "big_orange", "medium_grey" etc...)</para>
        /// <para>These are Styling classes created in the settings.css  You can add unlimited amount of Styles in your own css file,</para>
        /// <para>to style your captions at the top level already.</para>
        /// </summary>
        public string StyleClass { get; set; }

        //public string ParallaxClass { get; set; }

        /// <summary>
        /// Defines the start animation on Captions
        /// </summary>
        public IncomingAnimation? IncomingAnimation { get; set; }

        /// <summary>
        /// Defines the end animation on Captions
        /// </summary>
        public OutgoingAnimation? OutgoingAnimation { get; set; }

        /// <summary>
        /// <para>Only works if data-x set to left/center/right. It will move the Caption with the defined "px" from the</para>
        /// <para>aligned position. i.e. data-x="center" data-hoffset="-100" will put the caption 100px left from the</para>
        /// <para>slide center horizontaly.</para>
        /// </summary>
        public short? HorizontalOffset { get; set; }

        /// <summary>
        /// <para>Only works if data-y set to top/center/bottom. It will move the Caption with the defined "px" from the</para>
        /// <para>aligned position. i.e. data-x="center" data-hoffset="-100" will put the caption 100px left from the</para>
        /// <para>slide center vertically.</para>
        /// </summary>
        public short? VerticalOffset { get; set; }

        /// <summary>
        /// <para>Split Text Animation (incoming transition) to "words", "chars" or "lines". This will create amazing</para>
        /// <para>Animation Effects on one go, without the needs to create more captions.</para>
        /// </summary>
        public CaptionSplitType SplitIn { get; set; }

        /// <summary>
        /// <para>A Value between 0 and 1 like 0.1 to make delays between the Splitted Text Element (start) Animations.</para>
        /// <para>Higher the amount of words or chars, you should decrease that number!</para>
        /// </summary>
        public float? ElementDelay { get; set; }

        /// <summary>
        /// Split Text Animation (outgoing transition) to "words", "chars" or "lines". Only available if data-end is set!
        /// </summary>
        public CaptionSplitType SplitOut { get; set; }

        /// <summary>
        /// <para>A Value between 0 and 1 like 0.1 to make delays between the Splitted Text Element (end) Animations.</para>
        /// <para>Higher the amount of words or chars, you should decrease that number!</para>
        /// </summary>
        public float? EndElementDelay { get; set; }

        /// <summary>
        /// The Easing Art how the caption is moved in to the slide (note! Animation art set via Classes!).
        /// </summary>
        public EasingMethod? Easing { get; set; }

        /// <summary>
        /// The speed in milliseconds of the transition to move the Caption out from the Slide at the defined  timepoint.
        /// </summary>
        public short? EndSpeed { get; set; }

        /// <summary>
        /// The timepoint in millisecond when/at the Caption should move out from the slide.
        /// </summary>
        public short? End { get; set; }

        /// <summary>
        /// The Easing Art how the caption is moved out from the slide (note! Animation art set via Classes!).
        /// </summary>
        public EasingMethod? EndEasing { get; set; }

        #endregion General

        #region Video

        /// <summary>
        /// Will play the Video Directly when slider has been loaded.
        /// </summary>
        public bool AutoPlay { get; set; }

        /// <summary>
        /// After first Autplay the video will not be played automatically.
        /// </summary>
        public bool AutoPlayOnlyFirstTime { get; set; }

        /// <summary>
        /// After video come to the end position, it swaps to the next slide automatically.
        /// </summary>
        public bool NextSlideAtEnd { get; set; }

        /// <summary>
        /// <para>The full path to an image which will be shown as Thumbnail for the Video. (only if autoplay set to false,</para>
        /// <para>or autoplayonlyfirsttime set to true).</para>
        /// </summary>
        public string VideoPoster { get; set; }

        /// <summary>
        /// Used only at HTML5 Videos. In case it is selected, the Videos will be resized to cover the full Slider size.
        /// </summary>
        public bool ForceCover { get; set; }

        /// <summary>
        /// Every time the Slide is shown, the Video will rewind to the start.
        /// </summary>
        public bool ForceRewind { get; set; }

        public bool Mute { get; set; }

        /// <summary>
        /// Width of Video (i.e. 500 for 500px, or 100% for 100%
        /// </summary>
        public short? VideoWidth { get; set; }

        public CssUnit VideoWidthUnit { get; set; }

        /// <summary>
        /// Height of Video (i.e. 500 for 500px, or 100% for 100%
        /// </summary>
        public short? VideoHeight { get; set; }

        public CssUnit VideoHeightUnit { get; set; }

        /// <summary>
        /// "16:9" or "4:3"
        /// </summary>
        public AspectRatio? AspectRatio { get; set; }

        /// <summary>
        /// Which content of Video should be preloaded. "none", "meta", "auto".
        /// </summary>
        public VideoPreloadOption VideoPreload { get; set; }

        public VideoType? VideoType { get; set; }

        /// <summary>
        /// The MP4 Source for the HTML5 Video.
        /// </summary>
        public string VideoMp4 { get; set; }

        /// <summary>
        /// The WEBM Source for the HTML5 Video.
        /// </summary>
        public string VideoWebM { get; set; }

        /// <summary>
        /// The OGV Source for the HTML5 Video.
        /// </summary>
        public string VideoOgv { get; set; }

        /// <summary>
        /// The YouTube ID of the Video.
        /// </summary>
        public string YouTubeId { get; set; }

        /// <summary>
        /// The Vimeo ID of the Video.
        /// </summary>
        public string VimeoId { get; set; }

        /// <summary>
        /// Show/Hide the controls of the Video.
        /// </summary>
        public bool ShowVideoControls { get; set; }

        /// <summary>
        /// <para>Further Video Attributes for not listed Options and Settings of Video.</para>
        /// <para>Used for YouTube and Vimeo like "enablejsapi=1&html5=1&hd=1&wmode=opaque&showinfo=0&rel=0".</para>
        /// </summary>
        public string VideoAttributes { get; set; }

        /// <summary>
        /// <para>Loop HTML5 Videos - "none", "loop", "loopandnoslidestop" loop and no slide stop will</para>
        /// <para>loop the video, but as soon the video Timer reached the endpoint, next slide will be started.</para>
        /// </summary>
        public VideoLoop VideoLoop { get; set; }

        #endregion Video

        public virtual RevolutionSlide Slide { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members

        public string ToHtmlString()
        {
            var tagBuilder = new FluentTagBuilder("div")
            .AddCssClass("caption")
            .AddCssClass(StyleClass)
            .MergeAttribute("data-x", X.ToString().ToLowerInvariant())
            .MergeAttribute("data-y", Y.ToString().ToLowerInvariant())
            .MergeAttribute("data-speed", Speed)
            .MergeAttribute("data-start", Start);

            // General
            if (!string.IsNullOrEmpty(CaptionText))
            {
                tagBuilder = tagBuilder.SetInnerText(CaptionText);
            }

            if (IncomingAnimation.HasValue)
            {
                switch (IncomingAnimation.Value)
                {
                    case Domain.IncomingAnimation.ShortFromTop: tagBuilder = tagBuilder.AddCssClass("sft"); break;
                    case Domain.IncomingAnimation.ShortFromBottom: tagBuilder = tagBuilder.AddCssClass("sfb"); break;
                    case Domain.IncomingAnimation.ShortFromRight: tagBuilder = tagBuilder.AddCssClass("sfr"); break;
                    case Domain.IncomingAnimation.ShortFromLeft: tagBuilder = tagBuilder.AddCssClass("sfl"); break;
                    case Domain.IncomingAnimation.LongFromTop: tagBuilder = tagBuilder.AddCssClass("lft"); break;
                    case Domain.IncomingAnimation.LongFromBottom: tagBuilder = tagBuilder.AddCssClass("lfb"); break;
                    case Domain.IncomingAnimation.LongFromRight: tagBuilder = tagBuilder.AddCssClass("lfr"); break;
                    case Domain.IncomingAnimation.LongFromLeft: tagBuilder = tagBuilder.AddCssClass("lfl"); break;
                    case Domain.IncomingAnimation.SkewFromLeft: tagBuilder = tagBuilder.AddCssClass("skewfromleft"); break;
                    case Domain.IncomingAnimation.SkewFromRight: tagBuilder = tagBuilder.AddCssClass("skewfromright"); break;
                    case Domain.IncomingAnimation.SkewFromLeftShort: tagBuilder = tagBuilder.AddCssClass("skewfromleftshort"); break;
                    case Domain.IncomingAnimation.SkewFromRightShort: tagBuilder = tagBuilder.AddCssClass("skewfromrightshort"); break;
                    case Domain.IncomingAnimation.Fade: tagBuilder = tagBuilder.AddCssClass("fade"); break;
                    case Domain.IncomingAnimation.RandomRotate: tagBuilder = tagBuilder.AddCssClass("randomrotate"); break;
                }
            }

            if (OutgoingAnimation.HasValue)
            {
                switch (OutgoingAnimation.Value)
                {
                    case Domain.OutgoingAnimation.ShortToTop: tagBuilder = tagBuilder.AddCssClass("stt"); break;
                    case Domain.OutgoingAnimation.ShortToBottom: tagBuilder = tagBuilder.AddCssClass("stb"); break;
                    case Domain.OutgoingAnimation.ShortToRight: tagBuilder = tagBuilder.AddCssClass("str"); break;
                    case Domain.OutgoingAnimation.ShortToLeft: tagBuilder = tagBuilder.AddCssClass("stl"); break;
                    case Domain.OutgoingAnimation.LongToTop: tagBuilder = tagBuilder.AddCssClass("ltt"); break;
                    case Domain.OutgoingAnimation.LongToBottom: tagBuilder = tagBuilder.AddCssClass("ltb"); break;
                    case Domain.OutgoingAnimation.LongToRight: tagBuilder = tagBuilder.AddCssClass("ltr"); break;
                    case Domain.OutgoingAnimation.LongToLeft: tagBuilder = tagBuilder.AddCssClass("ltl"); break;
                    case Domain.OutgoingAnimation.SkewToLeft: tagBuilder = tagBuilder.AddCssClass("skewtoleft"); break;
                    case Domain.OutgoingAnimation.SkewToRight: tagBuilder = tagBuilder.AddCssClass("skewtoright"); break;
                    case Domain.OutgoingAnimation.SkewToLeftShort: tagBuilder = tagBuilder.AddCssClass("skewtoleftshort"); break;
                    case Domain.OutgoingAnimation.SkewToRightShort: tagBuilder = tagBuilder.AddCssClass("skewtorightshort"); break;
                    case Domain.OutgoingAnimation.FadeOut: tagBuilder = tagBuilder.AddCssClass("fadeout"); break;
                    case Domain.OutgoingAnimation.RandomRotateOut: tagBuilder = tagBuilder.AddCssClass("randomrotateout"); break;
                }
            }

            if (HorizontalOffset.HasValue)
            {
                tagBuilder = tagBuilder.MergeAttribute("data-hoffset", HorizontalOffset.Value);
            }
            if (VerticalOffset.HasValue)
            {
                tagBuilder = tagBuilder.MergeAttribute("data-voffset", VerticalOffset.Value);
            }
            if (SplitIn != CaptionSplitType.None)
            {
                tagBuilder = tagBuilder.MergeAttribute("data-splitin", SplitIn.ToString().ToLowerInvariant());
            }
            if (ElementDelay.HasValue)
            {
                tagBuilder = tagBuilder.MergeAttribute("data-elementdelay", ElementDelay.Value);
            }
            if (SplitOut != CaptionSplitType.None)
            {
                tagBuilder = tagBuilder.MergeAttribute("data-splitout", SplitOut.ToString().ToLowerInvariant());
            }
            if (EndElementDelay.HasValue)
            {
                tagBuilder = tagBuilder.MergeAttribute("data-endelementdelay", EndElementDelay.Value);
            }
            if (Easing.HasValue)
            {
                tagBuilder = tagBuilder.MergeAttribute("data-easing", Easing.Value.ToString());
            }
            if (EndSpeed.HasValue)
            {
                tagBuilder = tagBuilder.MergeAttribute("data-endspeed", EndSpeed.Value);
            }
            if (End.HasValue)
            {
                tagBuilder = tagBuilder.MergeAttribute("data-end", End.Value);
            }
            if (EndEasing.HasValue)
            {
                tagBuilder = tagBuilder.MergeAttribute("data-endeasing", EndEasing.Value.ToString());
            }

            // Video
            if (VideoType.HasValue)
            {
                tagBuilder = tagBuilder
                    .AddCssClass("tp-videolayer")
                    .AddCssClass("tp-caption");

                tagBuilder = tagBuilder
                    .MergeAttribute("data-autoplay", AutoPlay.ToString().ToLowerInvariant())
                    .MergeAttribute("data-autoplayonlyfirsttime", AutoPlayOnlyFirstTime.ToString().ToLowerInvariant())
                    .MergeAttribute("data-nextslideatend", NextSlideAtEnd.ToString().ToLowerInvariant())
                    .MergeAttribute("data-forcecover", ForceCover.ToString().ToLowerInvariant())
                    .MergeAttribute("data-forcerewind", ForceRewind.ToString().ToLowerInvariant())
                    .MergeAttribute("data-videocontrols", ShowVideoControls ? "controls" : "none");

                if (!string.IsNullOrEmpty(VideoPoster))
                {
                    tagBuilder = tagBuilder.MergeAttribute("data-videoposter", VideoPoster);
                }

                if (Mute)
                {
                    tagBuilder = tagBuilder.MergeAttribute("data-volume", "mute");
                }
                if (VideoWidth.HasValue)
                {
                    tagBuilder = tagBuilder.MergeAttribute(
                        "data-videowidth",
                        string.Format("{0}{1}", VideoWidth, VideoWidthUnit == CssUnit.Percentage ? "%" : "px"));
                }
                if (VideoHeight.HasValue)
                {
                    tagBuilder = tagBuilder.MergeAttribute(
                        "data-videoheight",
                        string.Format("{0}{1}", VideoHeight, VideoHeightUnit == CssUnit.Percentage ? "%" : "px"));
                }
                if (AspectRatio.HasValue)
                {
                    tagBuilder = tagBuilder.MergeAttribute("data-aspectratio", AspectRatio == Domain.AspectRatio._16x9 ? "16:9" : "4:3");
                }

                tagBuilder = tagBuilder.MergeAttribute("data-videopreload", VideoPreload.ToString().ToLowerInvariant());

                switch (VideoType.Value)
                {
                    case Domain.VideoType.Html5:
                        {
                            if (!string.IsNullOrEmpty(VideoMp4))
                            {
                                tagBuilder = tagBuilder.MergeAttribute("data-videomp4", VideoMp4);
                            }
                            if (!string.IsNullOrEmpty(VideoWebM))
                            {
                                tagBuilder = tagBuilder.MergeAttribute("data-videowebm", VideoWebM);
                            }
                            if (!string.IsNullOrEmpty(VideoOgv))
                            {
                                tagBuilder = tagBuilder.MergeAttribute("data-videoogv", VideoOgv);
                            }
                        }
                        break;
                    case Domain.VideoType.YouTube: tagBuilder = tagBuilder.MergeAttribute("data-ytid", YouTubeId); break;
                    case Domain.VideoType.Vimeo: tagBuilder = tagBuilder.MergeAttribute("data-vimeoid", VimeoId); break;
                }

                if (!string.IsNullOrEmpty(VideoAttributes))
                {
                    tagBuilder = tagBuilder.MergeAttribute("data-videoattributes", VideoAttributes);
                }

                tagBuilder = tagBuilder.MergeAttribute("data-videoloop", VideoLoop.ToString().ToLowerInvariant());
            }

            return tagBuilder.ToString();
        }
    }

    public class LayerMap : EntityTypeConfiguration<RevolutionLayer>, IEntityTypeConfiguration
    {
        public LayerMap()
        {
            ToTable(Constants.Tables.Layers);
            HasKey(x => x.Id);
            Property(x => x.SlideId).IsRequired();
            Property(x => x.CaptionText).HasMaxLength(255);
            Property(x => x.X).IsRequired();
            Property(x => x.Y).IsRequired();
            Property(x => x.Speed).IsRequired();
            Property(x => x.Start).IsRequired();
            Property(x => x.StyleClass).HasMaxLength(50);
            Property(x => x.SplitIn).IsRequired();
            Property(x => x.SplitOut).IsRequired();
            Property(x => x.AutoPlay).IsRequired();
            Property(x => x.AutoPlayOnlyFirstTime).IsRequired();
            Property(x => x.NextSlideAtEnd).IsRequired();
            Property(x => x.VideoPoster).HasMaxLength(255);
            Property(x => x.ForceCover).IsRequired();
            Property(x => x.ForceRewind).IsRequired();
            Property(x => x.Mute).IsRequired();
            Property(x => x.VideoWidthUnit).IsRequired();
            Property(x => x.VideoHeightUnit).IsRequired();
            Property(x => x.VideoPreload).IsRequired();
            Property(x => x.VideoMp4).HasMaxLength(255);
            Property(x => x.VideoWebM).HasMaxLength(255);
            Property(x => x.VideoOgv).HasMaxLength(255);
            Property(x => x.YouTubeId).HasMaxLength(50);
            Property(x => x.VimeoId).HasMaxLength(50);
            Property(x => x.ShowVideoControls).IsRequired();
            Property(x => x.VideoAttributes).HasMaxLength(128);
            Property(x => x.VideoLoop).IsRequired();
            HasRequired(x => x.Slide).WithMany(x => x.Layers).HasForeignKey(x => x.SlideId);
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return PluginManager.IsPluginInstalled(Constants.PluginSystemName); }
        }

        #endregion IEntityTypeConfiguration Members
    }
}