using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Logging.Server
{
    public class LogEntity
    {



        //public ObjectId _id { get; set; }

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


        //const int MaxIncrement = 16777216;

        //public ObjectId CreateObjectId()
        //{

        //    int machine = Convert.ToInt32(this.Time.ToString().ToString().Substring(10,4));

        //    short pid = Convert.ToInt16(this.Time.ToString().ToString().Substring(14, 3));

        //    int increment = Convert.ToInt32(this.Time.ToString().Substring(9, 8)) % MaxIncrement;

        //    return new ObjectId(Utils.GetDateTime(this.Time), machine, pid, increment);

        //    //return ObjectId.GenerateNewId(new DateTime(this.Time));
        //}

    }

    public class LogTag
    {
        public ObjectId _id { get; set; }

        public ObjectId LogId { get; set; }

        public long Tag { get; set; }


        public long Time { get; set; }
    }

}
