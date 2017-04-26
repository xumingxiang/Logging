using MongoDB.Bson;

namespace Logging.Server
{
    public class Options
    {
        public ObjectId _id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public long UpdateTime { get; set; }
    }
}