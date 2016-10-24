﻿using System;
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

        TimeZoneInfo DefaultTenantTimeZone { get; set; }

        TimeZoneInfo CurrentTimeZone { get; set; }
    }

    public partial class DateTimeHelper : IDateTimeHelper
    {
        private readonly IWorkContext workContext;
        private readonly IGenericAttributeService genericAttributeService;
        private readonly ISettingService settingService;
        private readonly DateTimeSettings dateTimeSettings;

        public DateTimeHelper(
            IWorkContext workContext,
            IGenericAttributeService genericAttributeService,
            ISettingService settingService,
            DateTimeSettings dateTimeSettings)
        {
            this.workContext = workContext;
            this.genericAttributeService = genericAttributeService;
            this.settingService = settingService;
            this.dateTimeSettings = dateTimeSettings;
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
            if (dateTimeSettings.AllowUsersToSetTimeZone)
            {
                string timeZoneId = string.Empty;

                if (user != null)
                {
                    timeZoneId = user.GetAttribute<string>(SystemUserAttributeNames.TimeZoneId, genericAttributeService);
                }

                try
                {
                    if (!string.IsNullOrEmpty(timeZoneId))
                    {
                        timeZoneInfo = FindTimeZoneById(timeZoneId);
                    }
                }
                catch (Exception exc)
                {
                    Debug.Write(exc.ToString());
                }
            }

            //default timezone
            if (timeZoneInfo == null)
            {
                timeZoneInfo = this.DefaultTenantTimeZone;
            }

            return timeZoneInfo;
        }

        public TimeZoneInfo DefaultTenantTimeZone
        {
            get
            {
                TimeZoneInfo timeZoneInfo = null;
                try
                {
                    if (!string.IsNullOrEmpty(dateTimeSettings.DefaultTimeZoneId))
                    {
                        timeZoneInfo = FindTimeZoneById(dateTimeSettings.DefaultTimeZoneId);
                    }
                }
                catch (Exception exc)
                {
                    Debug.Write(exc.ToString());
                }

                if (timeZoneInfo == null)
                {
                    timeZoneInfo = TimeZoneInfo.Local;

                }
                return timeZoneInfo;
            }
            set
            {
                string defaultTimeZoneId = string.Empty;
                if (value != null)
                {
                    defaultTimeZoneId = value.Id;
                }

                dateTimeSettings.DefaultTimeZoneId = defaultTimeZoneId;
                settingService.SaveSettings(dateTimeSettings);
            }
        }

        public TimeZoneInfo CurrentTimeZone
        {
            get
            {
                return GetUserTimeZone(workContext.CurrentUser);
            }
            set
            {
                if (!dateTimeSettings.AllowUsersToSetTimeZone)
                {
                    return;
                }

                string timeZoneId = string.Empty;
                if (value != null)
                {
                    timeZoneId = value.Id;
                }

                genericAttributeService.SaveAttribute(
                    workContext.CurrentUser,
                    SystemUserAttributeNames.TimeZoneId,
                    timeZoneId);
            }
        }
    }
}