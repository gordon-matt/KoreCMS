using Kore.ComponentModel;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;

namespace Kore.Plugins.Widgets.FullCalendar.ContentBlocks
{
    public class FullCalendarBlock : ContentBlockBase
    {
        public FullCalendarBlock()
        {
        }

        [LocalizedDisplayName(LocalizableStrings.ContentBlocks.FullCalendarBlock.CalendarId)]
        public int CalendarId { get; set; }

        #region ContentBlockBase Overrides

        public override string Name
        {
            get { return "Full Calendar"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "/Plugins/Widgets.FullCalendar/Views/Shared/DisplayTemplates/FullCalendarBlock.cshtml"; }
        }

        public override string EditorTemplatePath
        {
            get { return "/Plugins/Widgets.FullCalendar/Views/Shared/EditorTemplates/FullCalendarBlock.cshtml"; }
        }

        #endregion ContentBlockBase Overrides
    }
}