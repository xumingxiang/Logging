using Logging.Server.DB;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Logging.Server.Viewer
{
    internal class MongoDbViewer : ILogViewer
    {
        public List<LogEntity> GetLogs(long start, long end, int appId, int[] level, string title, string msg, string source, int ip, int limit = 100)
        {
            if (limit <= 0) { limit = 100; }

            var filterBuilder = Builders<LogEntity>.Filter;
            var filter = filterBuilder.Gte("Time", start) &
            filterBuilder.Lt("Time", end);

            if (level != null & level.Length > 0)
            {
                if (level.Length == 1)
                {
                    filter = filter & filterBuilder.Eq("Level", level);
                }
                else
                {
                    Array.Sort(level);
                    if (string.Join(",", level) != "1,2,3,4")
                    {
                        filter = filter & filterBuilder.In<int>("Level", level);
                    }
                }
            }

            if (appId > 0)
            {
                filter = filter & filterBuilder.Eq("AppId", appId);
            }

            if (ip > 0)
            {
                filter = filter & filterBuilder.Eq("IP", ip);
            }

            if (!string.IsNullOrWhiteSpace(source))
            {
                filter = filter & filterBuilder.Eq("Source", source);
            }

            if (!string.IsNullOrWhiteSpace(title))
            {
                var re = new BsonRegularExpression(".*" + title + ".*", "i");
                filter = filter & filterBuilder.Regex("Title", re);
            }

            if (!string.IsNullOrWhiteSpace(msg))
            {
                var re = new BsonRegularExpression(".*" + msg + ".*", "i");
                filter = filter & filterBuilder.Regex("Message", re);
            }
            //  filterBuilder.li
            var collection = MongoDataBase.GetCollection<LogEntity>();
            return collection.Find(filter).Limit(limit).ToListAsync<LogEntity>().Result;
        }
    }
}