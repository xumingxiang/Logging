using System;
using System.Collections.Concurrent;

namespace Logging.Client
{
    public sealed class LogManager
    {
        private static ConcurrentDictionary<string, ILog> _logs = new ConcurrentDictionary<string, ILog>();

        private LogManager()
        { }

        /// <summary>
        /// 通过类型名获取ILog实例。
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>ILog instance</returns>
        public static ILog GetLogger(Type type)
        {
            if (type == null)
            {
                return GetLogger("NoName");
            }
            else
            {
                return GetLogger(type.FullName);
            }
        }

        /// <summary>
        /// 通过字符串名获取ILog实例。
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>ILog instance</returns>
        public static ILog GetLogger(string name)
        {
            ILog log;
            var has = _logs.TryGetValue(name, out log);
            if (!has)
            {
                log = new SimpleLogger(name);
                _logs.TryAdd(name, log);
            }
            return log;
        }
    }
}