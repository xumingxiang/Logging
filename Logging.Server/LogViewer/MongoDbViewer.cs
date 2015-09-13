using Logging.Server.DB;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logging.Server.Viewer
{
    internal class MongoDbViewer : ILogViewer
    {
        public List<LogEntity> GetLogs(long start, long end, int appId, int[] level, string title, string msg, string source, int ip, List<string> tags, int limit = 100)
        {
            if (limit <= 0) { limit = 100; }

            List<LogEntity> result = new List<LogEntity>(); 

            if (tags != null && tags.Count > 0)
            {
                result= this.GetLogsInTags(start, end, appId, level, title, msg, source, ip, tags, limit);
            }
            else
            {
                result= this.GetLogsNoTags(start, end, appId, level, title, msg, source, ip, limit);
            }

            //result = result.OrderByDescending(x => x.Time).ToList();
            return result;
        }


        private List<LogEntity> GetLogsNoTags(long start, long end, int appId, int[] level, string title, string msg, string source, int ip, int limit = 100)
        {
            if (limit <= 0) { limit = 100; }

            var filterBuilder = Builders<LogEntity>.Filter;
            var filter = filterBuilder.Gte("Time", start) &
            filterBuilder.Lt("Time", end);

            if (level != null & level.Length > 0)
            {
                if (level.Length == 1)
                {
                    filter = filter & filterBuilder.Eq("Level", level[0]);
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
            var collection = MongoDataBase.GetCollection<LogEntity>();
            return collection.Find(filter).SortByDescending(x => x.Time).Limit(limit).ToListAsync<LogEntity>().Result;
        }


        private List<LogEntity> GetLogsInTags(long start, long end, int appId, int[] level, string title, string msg, string source, int ip, List<string> tags, int limit = 100)
        {
            if (limit <= 0) { limit = 100; }

            List<long> _tags = new List<long>();
            foreach (var tag in tags)
            {
                _tags.Add(Utils.BKDRHash(tag));

            }

            var tagFilterBuilder = Builders<LogTag>.Filter;
            var tagFilter = tagFilterBuilder.Gte("Time", start) &
                tagFilterBuilder.Lt("Time", end) &
                tagFilterBuilder.In<long>("Tag", _tags);

            var tagCollection = MongoDataBase.GetCollection<LogTag>();
            var tagEntity = tagCollection.Find(tagFilter).Project(x=>x.LogId).ToListAsync().Result;

            if (tagEntity == null || tagEntity.Count == 0) { return new List<LogEntity>(); }

            var filterBuilder = Builders<LogEntity>.Filter;
            var filter = filterBuilder.Gte("Time", start) &
            filterBuilder.Lt("Time", end);

            if (level != null & level.Length > 0)
            {
                if (level.Length == 1)
                {
                    filter = filter & filterBuilder.Eq("Level", level[0]);
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

            if (tagEntity.Count == 1)
            {
                filter = filter & filterBuilder.Eq("_id", tagEntity.First());
            }
            else
            {
                filter = filter & filterBuilder.In("_id", tagEntity);
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


            var collection = MongoDataBase.GetCollection<LogEntity>();
            return collection.Find(filter)
                .SortByDescending(x=>x.Time)
                .Limit(limit)
                .ToListAsync<LogEntity>()
                .Result;
        }


        public List<LogStatistics> GetStatistics(long start, long end, int appId)
        {
            // if (limit <= 0) { limit = 100; }

            var filterBuilder = Builders<LogStatistics>.Filter;
            var filter = filterBuilder.Gte("Time", start) &
            filterBuilder.Lt("Time", end);

            if (appId > 0)
            {
                filter = filter &  filterBuilder.Eq("AppId", appId);
            }

            var collection = MongoDataBase.GetCollection<LogStatistics>();
            var s = collection.Find(filter).ToListAsync<LogStatistics>().Result;

            var appIds = s.Select(x => x.AppId).Distinct();

            List<LogStatistics> result = new List<LogStatistics>();

            foreach (var _appId in appIds)
            {
                var lst = s.Where(x => x.AppId == _appId);

                LogStatistics item = new LogStatistics();
                item.AppId = _appId;
                item.Debug = lst.Sum(x => x.Debug);
                item.Info = lst.Sum(x => x.Info);
                item.Warm = lst.Sum(x => x.Warm);
                item.Error = lst.Sum(x => x.Error);
                result.Add(item);
            }

            return result
                 .OrderByDescending(x => x.Error)
                 .ThenByDescending(x => x.Warm)
                 .ThenByDescending(x => x.Info)
                 .ThenByDescending(x => x.Debug)
                 .ToList();
        }
    }
}