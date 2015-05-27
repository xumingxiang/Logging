using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging.Client
{
    internal class LogEntity
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public string IP { get; set; }

        /// <summary>
        /// 日志等级：4，error；3，warm；2，info；1：debug
        /// </summary>
        public LogLevel Level { get; set; }

        public Dictionary<string, string> Tags { get; set; }

        public long Time { get; set; }

    }
}
