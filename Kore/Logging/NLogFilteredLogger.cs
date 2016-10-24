using System;
using System.Globalization;
using Castle.Core.Logging;
using Kore.Infrastructure;
using NLog;

namespace Kore.Logging
{
    public class NLogFilteredLogger : LevelFilteredLogger
    {
        private readonly Logger logger;
        private readonly IWorkContext workContext;

        #region Constructors

        public NLogFilteredLogger(string name)
        {
            logger = LogManager.GetLogger(name);
            workContext = EngineContext.Current.Resolve<IWorkContext>();

            if (logger.IsDebugEnabled)
            {
                Level = LoggerLevel.Debug;
            }
            else if (logger.IsInfoEnabled)
            {
                Level = LoggerLevel.Info;
            }
            else if (logger.IsWarnEnabled)
            {
                Level = LoggerLevel.Warn;
            }
            else if (logger.IsErrorEnabled)
            {
                Level = LoggerLevel.Error;
            }
            else if (logger.IsFatalEnabled)
            {
                Level = LoggerLevel.Fatal;
            }
        }

        public NLogFilteredLogger(Type type)
            : this(type.FullName)
        {
        }

        public NLogFilteredLogger(string name, LoggerLevel level)
        {
            logger = LogManager.GetLogger(name);
            Level = level;
        }

        public NLogFilteredLogger(Type type, LoggerLevel level)
            : this(type.FullName, level)
        {
        }

        #endregion Constructors

        public override ILogger CreateChildLogger(string loggerName)
        {
            throw new NotSupportedException("NLog does not support child loggers.");
        }

        protected override void Log(LoggerLevel loggerLevel, string loggerName, string message, Exception exception)
        {
            LogLevel logLevel = null;
            switch (loggerLevel)
            {
                case LoggerLevel.Off: break;
                case LoggerLevel.Fatal: logLevel = LogLevel.Fatal; break;
                case LoggerLevel.Error: logLevel = LogLevel.Error; break;
                case LoggerLevel.Warn: logLevel = LogLevel.Warn; break;
                case LoggerLevel.Info: logLevel = LogLevel.Info; break;
                case LoggerLevel.Debug: logLevel = LogLevel.Debug; break;
                default: throw new ArgumentOutOfRangeException("loggerLevel");
            }

            var logEventInfo = new LogEventInfo(logLevel, loggerName, CultureInfo.InvariantCulture, message, null, exception);

            int? tenantId = null;
            if (workContext.CurrentTenant != null)
            {
                tenantId = workContext.CurrentTenant.Id;
            }

            logEventInfo.Properties["TenantId"] = tenantId;

            logger.Log(logEventInfo);
        }

        //protected override void Log(LoggerLevel loggerLevel, string loggerName, string message, Exception exception)
        //{
        //    switch (loggerLevel)
        //    {
        //        case LoggerLevel.Off: break;
        //        case LoggerLevel.Fatal: logger.Log(LogLevel.Fatal, message, exception); break;
        //        case LoggerLevel.Error: logger.Log(LogLevel.Error, message, exception); break;
        //        case LoggerLevel.Warn: logger.Log(LogLevel.Warn, message, exception); break;
        //        case LoggerLevel.Info: logger.Log(LogLevel.Info, message, exception); break;
        //        case LoggerLevel.Debug: logger.Log(LogLevel.Debug, message, exception); break;
        //        default: throw new ArgumentOutOfRangeException("loggerLevel");
        //    }
        //}
    }
}