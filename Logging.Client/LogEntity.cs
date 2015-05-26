using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging.Client
{
    public class LogEntity
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public string IP { get; set; }

        /// <summary>
        /// 日志等级：1，error；2，warm；3，info；4：debug
        /// </summary>
        public sbyte Level { get; set; }

        public Dictionary<string, string> Tags { get; set; }

        public long Time { get; set; }

    }
}
