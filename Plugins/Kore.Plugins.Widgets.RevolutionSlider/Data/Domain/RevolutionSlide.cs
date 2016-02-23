using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Web.Mvc;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Web.Mvc;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.RevolutionSlider.Data.Domain
{
    public class RevolutionSlide : IEntity
    {
        private ICollection<RevolutionLayer> layers;

        #region General

        public int Id { get; set; }

        public int SliderId { get; set; }

        public short Order { get; set; }

        #endregion General

        #region <li> Element

        public string Title { get; set; }

        public string Link { get; set; }

        public PageTarget? Target { get; set; }

        public Transition? Transition { get; set; }

        public bool RandomTransition { get; set; }

        public byte? SlotAmount { get; set; }

        public short? MasterSpeed { get; set; }

        public short? Delay { get; set; }

        public byte? SlideIndex { get; set; }

        public string Thumb { get; set; }

        #endregion <li> Element

        #region Main <img> Element

        public string ImageUrl { get; set; }

        public bool LazyLoad { get; set; }

        public BackgroundRepeat? BackgroundRepeat { get; set; }

        public BackgroundFit? BackgroundFit { get; set; }

        public short? BackgroundFitCustomValue { get; set; }

        public short? BackgroundFitEnd { get; set; }

        public BackgroundPosition? BackgroundPosition { get; set; }

        public BackgroundPosition? BackgroundPositionEnd { get; set; }

        public bool KenBurnsEffect { get; set; }

        public short? Duration { get; set; }

        public EasingMethod? Easing { get; set; }

        #endregion Main <img> Element

        public virtual ICollection<RevolutionLayer> Layers
        {
            get { return layers ?? (layers = new List<RevolutionLayer>()); }
            set { layers = value; }
        }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members

        public string ToHtmlString()
        {
            var tagBuilder = new FluentTagBuilder("li", TagRenderMode.StartTag)
                .MergeAttribute("data-randomtransition", RandomTransition ? "on" : "off");

            if (Transition.HasValue)
            {
                switch (Transition.Value)
                {
                    case Domain.Transition.SlotSlideHorizontal: tagBuilder = tagBuilder.MergeAttribute("data-transition", "slotslide-horizontal"); break;
                    case Domain.Transition.SlotSlideVertical: tagBuilder = tagBuilder.MergeAttribute("data-transition", "slotslide-vertical"); break;
                    case Domain.Transition.SlotFadeHorizontal: tagBuilder = tagBuilder.MergeAttribute("data-transition", "slotfade-horizontal"); break;
                    case Domain.Transition.SlotFadeVertical: tagBuilder = tagBuilder.MergeAttribute("data-transition", "slotfade-vertical"); break;
                    case Domain.Transition.SlotZoomHorizontal: tagBuilder = tagBuilder.MergeAttribute("data-transition", "slotzoom-horizontal"); break;
                    case Domain.Transition.SlotZoomVertical: tagBuilder = tagBuilder.MergeAttribute("data-transition", "slotzoom-vertical"); break;
                    default: tagBuilder = tagBuilder.MergeAttribute("data-transition", Transition.Value.ToString().ToLowerInvariant()); break;
                }
            }

            if (SlotAmount.HasValue)
            {
                tagBuilder = tagBuilder.MergeAttribute("data-slotamount", SlotAmount.Value);
            }
            if (MasterSpeed.HasValue)
            {
                tagBuilder = tagBuilder.MergeAttribute("data-masterspeed", MasterSpeed.Value);
            }
            if (Delay.HasValue)
            {
                tagBuilder = tagBuilder.MergeAttribute("data-delay", Delay.Value);
            }
            if (!string.IsNullOrEmpty(Link))
            {
                tagBuilder = tagBuilder.MergeAttribute("data-link", Link);
            }
            if (Target.HasValue)
            {
                switch (Target.Value)
                {
                    case PageTarget.Blank: tagBuilder = tagBuilder.MergeAttribute("data-target", "_blank"); break;
                    case PageTarget.Parent: tagBuilder = tagBuilder.MergeAttribute("data-target", "_parent"); break;
                    case PageTarget.Self: tagBuilder = tagBuilder.MergeAttribute("data-target", "_self"); break;
                    case PageTarget.Top: tagBuilder = tagBuilder.MergeAttribute("data-target", "_top"); break;
                }
            }
            if (SlideIndex.HasValue)
            {
                tagBuilder = tagBuilder.MergeAttribute("data-slideindex", SlideIndex.Value);
            }
            if (!string.IsNullOrEmpty(Thumb))
            {
                tagBuilder = tagBuilder.MergeAttribute("data-thumb", Thumb);
            }
            if (!string.IsNullOrEmpty(Title))
            {
                tagBuilder = tagBuilder.MergeAttribute("data-title", Title);
            }

            return tagBuilder.ToString();
        }

        public string ImageToHtmlString()
        {
            var tagBuilder = new FluentTagBuilder("img", TagRenderMode.SelfClosing)
                .MergeAttribute("alt", Title)
                .MergeAttribute("data-kenburns", KenBurnsEffect ? "on" : "off");

            if (LazyLoad)
            {
                tagBuilder = tagBuilder
                    .MergeAttribute("src", "http://placehold.it/350x150")
                    .MergeAttribute("data-lazyload", ImageUrl);
            }
            else
            {
                tagBuilder = tagBuilder.MergeAttribute("src", ImageUrl);
            }
            if (BackgroundRepeat.HasValue)
            {
                switch (BackgroundRepeat.Value)
                {
                    case Domain.BackgroundRepeat.NoRepeat: tagBuilder = tagBuilder.MergeAttribute("data-bgrepeat", "no-repeat"); break;
                    case Domain.BackgroundRepeat.Repeat: tagBuilder = tagBuilder.MergeAttribute("data-bgrepeat", "repeat"); break;
                    case Domain.BackgroundRepeat.RepeatX: tagBuilder = tagBuilder.MergeAttribute("data-bgrepeat", "repeat-x"); break;
                    case Domain.BackgroundRepeat.RepeatY: tagBuilder = tagBuilder.MergeAttribute("data-bgrepeat", "repeat-y"); break;
                }
            }
            if (BackgroundFit.HasValue)
            {
                switch (BackgroundFit.Value)
                {
                    case Domain.BackgroundFit.Contain: tagBuilder = tagBuilder.MergeAttribute("data-bgfit", "contain"); break;
                    case Domain.BackgroundFit.Cover: tagBuilder = tagBuilder.MergeAttribute("data-bgfit", "cover"); break;
                    case Domain.BackgroundFit.Normal: tagBuilder = tagBuilder.MergeAttribute("data-bgfit", "normal"); break;
                    case Domain.BackgroundFit.Custom:
                        {
                            if (BackgroundFitCustomValue.HasValue)
                            {
                                tagBuilder = tagBuilder.MergeAttribute("data-bgfit", BackgroundFitCustomValue.Value);
                            }
                        }
                        break;
                }
            }
            if (BackgroundFitEnd.HasValue)
            {
                tagBuilder = tagBuilder.MergeAttribute("data-bgfitend", BackgroundFitEnd.Value);
            }
            if (BackgroundPosition.HasValue)
            {
                switch (BackgroundPosition.Value)
                {
                    case Domain.BackgroundPosition.LeftTop: tagBuilder = tagBuilder.MergeAttribute("data-bgposition", "left top"); break;
                    case Domain.BackgroundPosition.LeftCenter: tagBuilder = tagBuilder.MergeAttribute("data-bgposition", "left center"); break;
                    case Domain.BackgroundPosition.LeftBottom: tagBuilder = tagBuilder.MergeAttribute("data-bgposition", "left bottom"); break;
                    case Domain.BackgroundPosition.CenterTop: tagBuilder = tagBuilder.MergeAttribute("data-bgposition", "center top"); break;
                    case Domain.BackgroundPosition.CenterCenter: tagBuilder = tagBuilder.MergeAttribute("data-bgposition", "center center"); break;
                    case Domain.BackgroundPosition.CenterBottom: tagBuilder = tagBuilder.MergeAttribute("data-bgposition", "center bottom"); break;
                    case Domain.BackgroundPosition.RightTop: tagBuilder = tagBuilder.MergeAttribute("data-bgposition", "right top"); break;
                    case Domain.BackgroundPosition.RightCenter: tagBuilder = tagBuilder.MergeAttribute("data-bgposition", "right center"); break;
                    case Domain.BackgroundPosition.RightBottom: tagBuilder = tagBuilder.MergeAttribute("data-bgposition", "right bottom"); break;
                }
            }
            if (BackgroundPositionEnd.HasValue)
            {
                switch (BackgroundPositionEnd.Value)
                {
                    case Domain.BackgroundPosition.LeftTop: tagBuilder = tagBuilder.MergeAttribute("data-bgpositionend", "left top"); break;
                    case Domain.BackgroundPosition.LeftCenter: tagBuilder = tagBuilder.MergeAttribute("data-bgpositionend", "left center"); break;
                    case Domain.BackgroundPosition.LeftBottom: tagBuilder = tagBuilder.MergeAttribute("data-bgpositionend", "left bottom"); break;
                    case Domain.BackgroundPosition.CenterTop: tagBuilder = tagBuilder.MergeAttribute("data-bgpositionend", "center top"); break;
                    case Domain.BackgroundPosition.CenterCenter: tagBuilder = tagBuilder.MergeAttribute("data-bgpositionend", "center center"); break;
                    case Domain.BackgroundPosition.CenterBottom: tagBuilder = tagBuilder.MergeAttribute("data-bgpositionend", "center bottom"); break;
                    case Domain.BackgroundPosition.RightTop: tagBuilder = tagBuilder.MergeAttribute("data-bgpositionend", "right top"); break;
                    case Domain.BackgroundPosition.RightCenter: tagBuilder = tagBuilder.MergeAttribute("data-bgpositionend", "right center"); break;
                    case Domain.BackgroundPosition.RightBottom: tagBuilder = tagBuilder.MergeAttribute("data-bgpositionend", "right bottom"); break;
                }
            }
            if (Duration.HasValue)
            {
                tagBuilder = tagBuilder.MergeAttribute("data-duration", Duration.Value);
            }
            if (Easing.HasValue)
            {
                tagBuilder = tagBuilder.MergeAttribute("data-easing", Easing.ToString());
            }

            return tagBuilder.ToString();
        }
    }

    public class SlideMap : EntityTypeConfiguration<RevolutionSlide>, IEntityTypeConfiguration
    {
        public SlideMap()
        {
            ToTable(Constants.Tables.Slides);
            HasKey(x => x.Id);
            Property(x => x.SliderId).IsRequired();
            Property(x => x.Order).IsRequired();
            Property(x => x.Title).HasMaxLength(255).IsUnicode(true);
            Property(x => x.Link).HasMaxLength(255).IsUnicode(true);
            Property(x => x.RandomTransition).IsRequired();
            Property(x => x.Thumb).HasMaxLength(255).IsUnicode(true);
            Property(x => x.ImageUrl).IsRequired().HasMaxLength(255).IsUnicode(true);
            Property(x => x.LazyLoad).IsRequired();
            Property(x => x.KenBurnsEffect).IsRequired();
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return PluginManager.IsPluginInstalled(Constants.PluginSystemName); }
        }

        #endregion IEntityTypeConfiguration Members
    }
}