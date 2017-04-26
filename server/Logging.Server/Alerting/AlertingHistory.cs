using MongoDB.Bson;

namespace Logging.Server.Alerting
{
    public class AlertingHistory
    {
        public ObjectId _id { get; set; }

        public int ObjId { get; set; }

        public AlertingType AlertingType { get; set; }

        public long Time { get; set; }
    }
}