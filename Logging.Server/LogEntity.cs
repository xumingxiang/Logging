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


    public class LogEntityComparer : IComparer<LogEntity>
    {
        public int Compare(LogEntity x, LogEntity y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return 1;
            if (y == null) return -1;

            if (x.Time > y.Time) return -1;
            if (x.Time < y.Time) return 1;

            if (x.Time == y.Time)
            {
                if (x._id.Increment > y._id.Increment) return -1;
                if (x._id.Increment < y._id.Increment) return 1;
            }

            return 0;
        }
    }
}
