using System;
using Kore.Web.ContentManagement.Areas.Admin.Widgets;

namespace Kore.Plugins.Widgets.Google.Widgets
{
    public class GoogleMapWidget : WidgetBase
    {
        public GoogleMapWidget()
        {
            Zoom = 8;
        }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public byte Zoom { get; set; }

        public short Height { get; set; }

        #region WidgetBase Overrides

        public override string Name
        {
            get { return "Google: Map"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "/Plugins/Plugins.Widgets.Google/Views/Shared/DisplayTemplates/GoogleMapWidget.cshtml"; }
        }

        public override string EditorTemplatePath
        {
            get { return "/Plugins/Plugins.Widgets.Google/Views/Shared/EditorTemplates/GoogleMapWidget.cshtml"; }
        }

        #endregion WidgetBase Overrides
    }
}