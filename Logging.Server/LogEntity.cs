using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Logging.Server
{
    public class LogEntity
    {

        public ObjectId _id { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public long IP { get; set; }

        public LogLevel Level { get; set; }

        public int AppId { get; set; }

        public long Time { get; set; }

        public string Source { get; set; }

        public int Thread { get; set; }

        public List<string> Tags { get; set; }
    }
}
