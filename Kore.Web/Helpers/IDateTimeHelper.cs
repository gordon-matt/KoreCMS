using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Kore.Security.Membership;
using Kore.Web.Configuration;
using Kore.Web.Configuration.Domain;
using Kore.Web.Configuration.Services;
using Kore.Web.Security.Membership;

namespace Kore.Web.Helpers
{
    public interface IDateTimeHelper
    {
        TimeZoneInfo FindTimeZoneById(string id);

        ReadOnlyCollection<TimeZoneInfo> GetSystemTimeZones();

        DateTime ConvertToUserTime(DateTime dateTime);

        DateTime ConvertToUserTime(DateTime dateTime, DateTimeKind sourceDateTimeKind);

        DateTime ConvertToUserTime(DateTime dateTime, TimeZoneInfo sourceTimeZone);

        DateTime ConvertToUserTime(DateTime dateTime, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone);

        DateTime ConvertToUtcTime(DateTime dateTime);

        DateTime ConvertToUtcTime(DateTime dateTime, DateTimeKind sourceDateTimeKind);

        DateTime ConvertToUtcTime(DateTime dateTime, TimeZoneInfo sourceTimeZone);

        TimeZoneInfo GetUserTimeZone(KoreUser user);

        TimeZoneInfo DefaultStoreTimeZone { get; set; }

        TimeZoneInfo CurrentTimeZone { get; set; }
    }

    public partial class DateTimeHelper : IDateTimeHelper
    {
        private readonly IWorkContext _workContext;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ISettingService _settingService;
        private readonly DateTimeSettings _dateTimeSettings;

        public DateTimeHelper(
            IWorkContext workContext,
            IGenericAttributeService genericAttributeService,
            ISettingService settingService,
            DateTimeSettings dateTimeSettings)
        {
            this._workContext = workContext;
            this._genericAttributeService = genericAttributeService;
            this._settingService = settingService;
            this._dateTimeSettings = dateTimeSettings;
        }

        public TimeZoneInfo FindTimeZoneById(string id)
        {
            return TimeZoneInfo.FindSystemTimeZoneById(id);
        }

        public ReadOnlyCollection<TimeZoneInfo> GetSystemTimeZones()
        {
            return TimeZoneInfo.GetSystemTimeZones();
        }

        public DateTime ConvertToUserTime(DateTime dateTime)
        {
            return ConvertToUserTime(dateTime, dateTime.Kind);
        }

        public DateTime ConvertToUserTime(DateTime dateTime, DateTimeKind sourceDateTimeKind)
        {
            dateTime = DateTime.SpecifyKind(dateTime, sourceDateTimeKind);
            var currentUserTimeZoneInfo = this.CurrentTimeZone;
            return TimeZoneInfo.ConvertTime(dateTime, currentUserTimeZoneInfo);
        }

        public DateTime ConvertToUserTime(DateTime dateTime, TimeZoneInfo sourceTimeZone)
        {
            var currentUserTimeZoneInfo = this.CurrentTimeZone;
            return ConvertToUserTime(dateTime, sourceTimeZone, currentUserTimeZoneInfo);
        }

        public DateTime ConvertToUserTime(DateTime dateTime, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone)
        {
            return TimeZoneInfo.ConvertTime(dateTime, sourceTimeZone, destinationTimeZone);
        }

        public DateTime ConvertToUtcTime(DateTime dateTime)
        {
            return ConvertToUtcTime(dateTime, dateTime.Kind);
        }

        public DateTime ConvertToUtcTime(DateTime dateTime, DateTimeKind sourceDateTimeKind)
        {
            dateTime = DateTime.SpecifyKind(dateTime, sourceDateTimeKind);
            return TimeZoneInfo.ConvertTimeToUtc(dateTime);
        }

        public DateTime ConvertToUtcTime(DateTime dateTime, TimeZoneInfo sourceTimeZone)
        {
            if (sourceTimeZone.IsInvalidTime(dateTime))
            {
                //could not convert
                return dateTime;
            }

            return TimeZoneInfo.ConvertTimeToUtc(dateTime, sourceTimeZone);
        }

        public TimeZoneInfo GetUserTimeZone(KoreUser user)
        {
            //registered user
            TimeZoneInfo timeZoneInfo = null;
            if (_dateTimeSettings.AllowUsersToSetTimeZone)
            {
                string timeZoneId = string.Empty;
                if (user != null)
                    timeZoneId = user.GetAttribute<string>(SystemUserAttributeNames.TimeZoneId, _genericAttributeService);

                try
                {
                    if (!string.IsNullOrEmpty(timeZoneId))
                        timeZoneInfo = FindTimeZoneById(timeZoneId);
                }
                catch (Exception exc)
                {
                    Debug.Write(exc.ToString());
                }
            }

            //default timezone
            if (timeZoneInfo == null)
                timeZoneInfo = this.DefaultStoreTimeZone;

            return timeZoneInfo;
        }

        public TimeZoneInfo DefaultStoreTimeZone
        {
            get
            {
                TimeZoneInfo timeZoneInfo = null;
                try
                {
                    if (!string.IsNullOrEmpty(_dateTimeSettings.DefaultTimeZoneId))
                        timeZoneInfo = FindTimeZoneById(_dateTimeSettings.DefaultTimeZoneId);
                }
                catch (Exception exc)
                {
                    Debug.Write(exc.ToString());
                }

                if (timeZoneInfo == null)
                    timeZoneInfo = TimeZoneInfo.Local;

                return timeZoneInfo;
            }
            set
            {
                string defaultTimeZoneId = string.Empty;
                if (value != null)
                {
                    defaultTimeZoneId = value.Id;
                }

                _dateTimeSettings.DefaultTimeZoneId = defaultTimeZoneId;
                _settingService.SaveSettings(_dateTimeSettings);
            }
        }

        public TimeZoneInfo CurrentTimeZone
        {
            get
            {
                return GetUserTimeZone(_workContext.CurrentUser);
            }
            set
            {
                if (!_dateTimeSettings.AllowUsersToSetTimeZone)
                {
                    return;
                }

                string timeZoneId = string.Empty;
                if (value != null)
                {
                    timeZoneId = value.Id;
                }

                _genericAttributeService.SaveAttribute(
                    _workContext.CurrentUser,
                    SystemUserAttributeNames.TimeZoneId,
                    timeZoneId);
            }
        }
    }
}