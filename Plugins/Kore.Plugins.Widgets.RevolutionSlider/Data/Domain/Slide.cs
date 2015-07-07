using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Web.Mvc;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.RevolutionSlider.Data.Domain
{
    public class Slide : IEntity
    {
        public int Id { get; set; }

        public int SliderId { get; set; }

        public int Order { get; set; }

        /// <summary>
        /// <para>The appearance transition of this slide. You can define more than one, so in each loop the</para>
        /// <para>next slide transition type will be shown.</para>
        /// </summary>
        public Transition? Transition { get; set; }

        /// <summary>
        /// <para>This will allow a Random transitions of the Selected Transitions you choosed in the data-transitions.</para>
        /// <para>Don't use together with random premium and random flat transitions!</para>
        /// </summary>
        public bool RandomTransition { get; set; }

        /// <summary>
        /// The number of slots or boxes the slide is divided into. If you use boxfade, over 7 slots can be juggy.
        /// </summary>
        public byte? SlotAmount { get; set; }

        /// <summary>
        /// The speed of the transition in "ms". Default value is 300 (0.3 sec).
        /// </summary>
        public short? MasterSpeed { get; set; }

        /// <summary>
        /// A new Dealy value for the Slide. If no delay defined per slide, the dealy defined via Options will be used.
        /// </summary>
        public short? Delay { get; set; }

        /// <summary>
        /// A link on the whole slide pic.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// The target of the Link for the whole slide pic. (i.e. "_blank", "_self").
        /// </summary>
        public PageTarget? Target { get; set; }

        /// <summary>
        /// <para>Possible values:  next,back, 1-x (where x is the max. Amount of slide).</para>
        /// <para>If this value is set, click on slide will call the next / previous, or  n th Slide.</para>
        /// </summary>
        public byte? SlideIndex { get; set; }

        /// <summary>
        /// An Alternative Source for thumbs. If not defined a copy of the background image will be used in resized form.
        /// </summary>
        public string Thumb { get; set; }

        /// <summary>
        /// <para>In Navigation Style Preview1- preview4 mode you can show the Title of the Slides also.</para>
        /// <para>Due this option you can define for each slide its own Title.</para>
        /// </summary>
        public string Title { get; set; }

        public bool LazyLoad { get; set; }

        /// <summary>
        /// The way the image is shown in the slide main container
        /// </summary>
        public BackgroundRepeat? BackgroundRepeat { get; set; }

        /// <summary>
        /// <para>data-bgfit:cover / contain / normal / width(%) height(%). (Select one to decide how the image should fit in the</para>
        /// <para>Slide Main Container). For Ken Burn use only a Number, which is the % Zoom at start. 100 will fit with Width or height</para>
        /// <para>automatically, 200 will be double sized etc..</para>
        /// </summary>
        public BackgroundFit? BackgroundFit { get; set; }

        public short? BackgroundFitCustomValue { get; set; }

        /// <summary>
        /// Use only a Number. i.e. 300 will be a 300% Zoomed image where the basic 100% is fitting with width or height.
        /// </summary>
        public short? BackgroundFitEnd { get; set; }

        public BackgroundPosition? BackgroundPosition { get; set; }

        /// <summary>
        /// For Ken Burns Animation. This is where the IMG will be animatied.
        /// </summary>
        public BackgroundPosition? BackgroundPositionEnd { get; set; }

        /// <summary>
        /// Turn on Ken Burns Effect or keep it disabled
        /// </summary>
        public bool KenBurnsEffect { get; set; }

        /// <summary>
        /// <para>Duration for Ken Burns. The value in ms how long the animation of ken burns effect should go. i.e. 3000</para>
        /// <para>will make a 3s zoom and movement.</para>
        /// </summary>
        public short? Duration { get; set; }

        /// <summary>
        /// Easing of Ken Burns Effect.
        /// </summary>
        public EasingMethod? Easing { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class SlideMap : EntityTypeConfiguration<Slide>, IEntityTypeConfiguration
    {
        public SlideMap()
        {
            ToTable(Constants.Tables.Slides);
            HasKey(x => x.Id);
            Property(x => x.SliderId).IsRequired();
            Property(x => x.Order).IsRequired();
            Property(x => x.RandomTransition).IsRequired();
            Property(x => x.Link).HasMaxLength(255);
            Property(x => x.Thumb).HasMaxLength(255);
            Property(x => x.Title).HasMaxLength(255);
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