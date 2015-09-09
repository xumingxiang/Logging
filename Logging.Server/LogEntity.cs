using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Logging.Server
{
    public  class LogEntity
    {
        public ObjectId _id { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public string IP { get; set; }

        public LogLevel Level { get; set; }

        public int AppId { get; set; }

        public long Time { get; set; }

        public string Source { get; set; }

        public int Thread { get; set; }

        public Dictionary<string, string> Tags { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateTime { get; set; }

    }

    public class LogTag 
    {
        public ObjectId _id { get; set; }

        public ObjectId LogId { get; set; }

        public string TagName { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateTime { get; set; }
    }

}
