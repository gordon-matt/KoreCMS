using System;
using Castle.Core.Logging;

namespace Kore.Logging
{
    internal class NLogLoggerFactory : ILoggerFactory
    {
        public ILogger Create(Type type)
        {
            if (type != null)
            {
                return new NLogFilteredLogger(type);
            }

            throw new ArgumentNullException("type");
        }

        public ILogger Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                return new NLogFilteredLogger(name);
            }

            throw new ArgumentNullException("name");
        }

        public ILogger Create(Type type, LoggerLevel level)
        {
            if (type != null)
            {
                return new NLogFilteredLogger(type, level);
            }

            throw new ArgumentNullException("type");
        }

        public ILogger Create(string name, LoggerLevel level)
        {
            if (!string.IsNullOrEmpty(name))
            {
                return new NLogFilteredLogger(name, level);
            }

            throw new ArgumentNullException("name");
        }
    }
}