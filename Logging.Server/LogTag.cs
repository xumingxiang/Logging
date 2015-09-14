using MongoDB.Bson;

namespace Logging.Server
{
    public class LogTag
    {
        public ObjectId _id { get; set; }

        public ObjectId LogId { get; set; }

        public long Tag { get; set; }

        public long Time { get; set; }
    }
}