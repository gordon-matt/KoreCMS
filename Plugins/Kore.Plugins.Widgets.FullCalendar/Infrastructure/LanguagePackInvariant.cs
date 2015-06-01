using System.Collections.Generic;
using Kore.Localization;

namespace Kore.Plugins.Widgets.FullCalendar.Infrastructure
{
    public class LanguagePackInvariant : ILanguagePack
    {
        #region ILanguagePack Members

        public string CultureCode
        {
            get { return null; }
        }

        public IDictionary<string, string> LocalizedStrings
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { LocalizableStrings.CalendarEventModel.CalendarId, "Calendar" },
                    { LocalizableStrings.CalendarEventModel.EndDateTime, "End" },
                    { LocalizableStrings.CalendarEventModel.Name, "Name" },
                    { LocalizableStrings.CalendarEventModel.StartDateTime, "Start" },
                    { LocalizableStrings.CalendarModel.Name, "Name" },
                    { LocalizableStrings.ContentBlocks.FullCalendarBlock.CalendarId, "Calendar" },
                    { LocalizableStrings.Events, "Events" },
                    { LocalizableStrings.FullCalendar, "Full Calendar" },
                };
            }
        }

        #endregion ILanguagePack Members
    }
}