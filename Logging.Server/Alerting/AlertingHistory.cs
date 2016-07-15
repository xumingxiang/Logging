using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
