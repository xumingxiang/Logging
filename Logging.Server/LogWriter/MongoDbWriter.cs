using Logging.Server.DB;
using System;
using System.Collections.Generic;

namespace Logging.Server.Writer
{
    internal class MongoDbWriter : ILogWriter
    {
        public void Write(IList<LogEntity> logs)
        {
            var log_collection = MongoDataBase.GetCollection<LogEntity>();
            log_collection.InsertManyAsync(logs);

            List<LogTag> tags = new List<LogTag>();
            foreach (var log in logs)
            {
                if (log.Tags != null)
                {
                    foreach (var tag in log.Tags)
                    {
                        tags.Add(new LogTag { LogId = log._id, TagName = tag.Key + "=" + tag.Value, CreateTime = DateTime.Now });
                    }
                }
            }

            if (tags.Count > 0)
            {
                var tag_collection = MongoDataBase.GetCollection<LogTag>();
                tag_collection.InsertManyAsync(tags);
            }
        }
    }
}