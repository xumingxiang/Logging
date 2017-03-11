using Logging.Server.DB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Logging.Server
{
    /// <summary>
    /// 日志开关
    /// </summary>
    public class LogOnOff
    {
        public ObjectId _id { get; set; }

        public int AppId { get; set; }

        public string AppName { get; set; }

        public byte Debug { get; set; }

        public byte Info { get; set; }

        public byte Warn { get; set; }

        public byte Error { get; set; }


     
    }
}
