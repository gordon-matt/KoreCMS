using System.ComponentModel.DataAnnotations;
using Kore.Web.ContentManagement.Areas.Admin.Widgets;

namespace Kore.Plugins.Widgets.FlexSlider.Widgets
{
    public class FlexSliderWidget : WidgetBase
    {
        public FlexSliderWidget()
        {
            ControlNav = false;
            AnimationLoop = true;
            Slideshow = true;
            SlideshowSpeed = 5000;
        }

        [Display(Name = "Media Folder")]
        public string MediaFolder { get; set; }

        [Display(Name = "Control Nav")]
        public bool ControlNav { get; set; }

        [Display(Name = "Animation Loop")]
        public bool AnimationLoop { get; set; }

        public bool Slideshow { get; set; }

        [Display(Name = "Slideshow Speed")]
        public int SlideshowSpeed { get; set; }

        #region WidgetBase Overrides

        public override string Name
        {
            get { return "Flex Slider"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "/Plugins/Plugins.Widgets.FlexSlider/Views/Shared/DisplayTemplates/FlexSliderWidget.cshtml"; }
        }

        public override string EditorTemplatePath
        {
            get { return "/Plugins/Plugins.Widgets.FlexSlider/Views/Shared/EditorTemplates/FlexSliderWidget.cshtml"; }
        }

        #endregion WidgetBase Overrides
    }
}