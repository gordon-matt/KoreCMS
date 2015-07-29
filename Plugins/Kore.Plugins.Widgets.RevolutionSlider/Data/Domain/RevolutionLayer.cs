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

        public short Start { get; set; }

        public short Speed { get; set; }

        public NavigationHAlign X { get; set; }

        public NavigationVAlign Y { get; set; }

        public string StyleClass { get; set; }

        //public string ParallaxClass { get; set; }

        public IncomingAnimation? IncomingAnimation { get; set; }

        public OutgoingAnimation? OutgoingAnimation { get; set; }

        public short? HorizontalOffset { get; set; }

        public short? VerticalOffset { get; set; }

        public CaptionSplitType SplitIn { get; set; }

        public float? ElementDelay { get; set; }

        public CaptionSplitType SplitOut { get; set; }

        public float? EndElementDelay { get; set; }

        public EasingMethod? Easing { get; set; }

        public short? EndSpeed { get; set; }

        public short? End { get; set; }

        public EasingMethod? EndEasing { get; set; }

        #endregion General

        #region Video

        public bool AutoPlay { get; set; }

        public bool AutoPlayOnlyFirstTime { get; set; }

        public bool NextSlideAtEnd { get; set; }

        public string VideoPoster { get; set; }

        public bool ForceCover { get; set; }

        public bool ForceRewind { get; set; }

        public bool Mute { get; set; }

        public short? VideoWidth { get; set; }

        public CssUnit VideoWidthUnit { get; set; }

        public short? VideoHeight { get; set; }

        public CssUnit VideoHeightUnit { get; set; }

        public AspectRatio? AspectRatio { get; set; }

        public VideoPreloadOption VideoPreload { get; set; }

        public VideoType? VideoType { get; set; }

        public string VideoMp4 { get; set; }

        public string VideoWebM { get; set; }

        public string VideoOgv { get; set; }

        public string YouTubeId { get; set; }

        public string VimeoId { get; set; }

        public bool ShowVideoControls { get; set; }

        public string VideoAttributes { get; set; }

        public VideoLoop VideoLoop { get; set; }

        #endregion Video

        public virtual RevolutionSlide Slide { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members

        public string ToHtmlString(bool isFullScreen)
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
                tagBuilder = tagBuilder.SetInnerHtml(CaptionText);
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

                if (isFullScreen)
                {
                    tagBuilder.AddCssClass("fullscreenvideo");
                }

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