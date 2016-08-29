using Logging.Server.DB;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using Logging.Server.Alerting;

namespace Logging.Server.Viewer
{
    internal class MongoDbViewer : ILogViewer
    {
        public List<LogEntity> GetLogs(long start, long end, int appId, int[] level, string title, string msg, string source, long ip, List<string> tags, int limit = 100)
        {
            if (limit <= 0) { limit = 100; }

            List<LogEntity> result = new List<LogEntity>();

            if (tags != null && tags.Count > 0)
            {
                result = this.GetLogsInTags(start, end, appId, level, title, msg, source, ip, tags, limit);
            }
            else
            {
                result = this.GetLogsNoTags(start, end, appId, level, title, msg, source, ip, limit);
            }


            if (result != null)
            {
                result.Sort(new LogEntityComparer());
            }
            return result;
        }


        private List<LogEntity> GetLogsNoTags(long start, long end, int appId, int[] level, string title, string msg, string source, long ip, int limit = 100)
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
            var result = collection.Find(filter)
                .SortByDescending(x => x.Time)
                .Limit(limit)
                .ToListAsync<LogEntity>()
                .Result;


            return result;
        }


        private List<LogEntity> GetLogsInTags(long start, long end, int appId, int[] level, string title, string msg, string source, long ip, List<string> tags, int limit = 100)
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
            var tagEntity = tagCollection.Find(tagFilter).Project(x => x.LogId).ToListAsync().Result;

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
            var result = collection.Find(filter)
                .SortByDescending(x => x.Time)
                .Limit(limit)
                .ToListAsync<LogEntity>()
                .Result;

            return result;
        }


        public List<LogStatistics> GetStatistics(long start, long end, int appId)
        {
            // if (limit <= 0) { limit = 100; }

            var filterBuilder = Builders<LogStatistics>.Filter;
            var filter = filterBuilder.Gte("Time", start) &
            filterBuilder.Lt("Time", end);

            if (appId > 0)
            {
                filter = filter & filterBuilder.Eq("AppId", appId);
            }

            var collection = MongoDataBase.GetCollection<LogStatistics>();
            var s = collection.Find(filter).ToListAsync<LogStatistics>().Result;

            var appIds = s.Select(x => x.AppId).Distinct();

            List<LogStatistics> result = new List<LogStatistics>();

            var logOnOffs = GetALLLogOnOff();

            foreach (var _appId in appIds)
            {
                var lst = s.Where(x => x.AppId == _appId);
                var onOff = logOnOffs.FirstOrDefault(x => x.AppId == _appId);
                LogStatistics item = new LogStatistics();
                item.AppId = _appId;
                item.Debug = lst.Sum(x => x.Debug);
                item.Info = lst.Sum(x => x.Info);
                item.Warm = lst.Sum(x => x.Warm);
                item.Error = lst.Sum(x => x.Error);
                if (onOff != null)
                {
                    item.AppName = onOff.AppName;
                }
                result.Add(item);
            }

            return result
                 .OrderByDescending(x => x.Error)
                 .ThenByDescending(x => x.Warm)
                 .ThenByDescending(x => x.Info)
                 .ThenByDescending(x => x.Debug)
                 .ToList();
        }

        public List<LogOnOff> GetALLLogOnOff()
        {
            var filterBuilder = Builders<LogOnOff>.Filter;
            var filter = filterBuilder.Gt("AppId", 0);

            var collection = MongoDataBase.GetCollection<LogOnOff>();
            var result = collection.Find(filter).ToListAsync<LogOnOff>().Result;
            return result.OrderBy(x => x.AppId).ToList();
        }

        public LogOnOff GetLogOnOff(int appId)
        {
            var filterBuilder = Builders<LogOnOff>.Filter;
            var filter = filterBuilder.Eq("AppId", appId);

            var collection = MongoDataBase.GetCollection<LogOnOff>();
            var result = collection.Find(filter).FirstOrDefaultAsync().Result;
            return result;
        }


        public AlertingHistory GetLastAlertingHistory(int appId, AlertingType type)
        {
            var collection = MongoDataBase.GetCollection<AlertingHistory>();
            var filterBuilder = Builders<AlertingHistory>.Filter;
            var filter = filterBuilder.Gt("ObjId", appId) & filterBuilder.Gt("AlertingType", type);
            var result = collection.Find(filter)
                .SortByDescending(x => x.Time)
                .FirstOrDefaultAsync().Result;
            return result;
        }

        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public List<Options> GetOptions(string[] keys)
        {
            var collection = MongoDataBase.GetCollection<Options>();
            var filterBuilder = Builders<Options>.Filter;
            var filter = filterBuilder.In("Key", keys);
            var result = collection.Find(filter).ToListAsync().Result;
            return result;
        }

    }
}