using Logging.Server.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using Logging.Server.Alerting;

namespace Logging.Server.Writer
{
    internal class MongoDbWriter : ILogWriter
    {
        public void Write(IList<LogEntity> logs)
        {
            #region 写日志主体
            var log_collection = MongoDataBase.GetCollection<LogEntity>();
            log_collection.InsertManyAsync(logs);
            #endregion

            #region 写Tag

            List<LogTag> tags = new List<LogTag>();
            foreach (var log in logs)
            {
                if (log.Tags != null)
                {
                    foreach (var tag in log.Tags)
                    {
                        tags.Add(new LogTag
                        {
                            LogId = log._id,
                            Tag = Utils.BKDRHash(tag),
                            Time = log.Time
                        });
                    }
                }
            }

            if (tags.Count > 0)
            {
                var tag_collection = MongoDataBase.GetCollection<LogTag>();
                tag_collection.InsertManyAsync(tags);

                //foreach (var item in tags)
                //{
                //    var result = tag_collection.InsertOneAsync(item);
                //}
            }
            #endregion


            List<LogStatistics> lss = new List<LogStatistics>();
            var app_grp = from a in logs group a by a.AppId into g select g;
            long statisticsTime = Utils.GetTimeStamp(DateTime.Now);
            foreach (var grp in app_grp)
            {
                int debug = grp.Count(x => x.Level == LogLevel.Debug);
                int info = grp.Count(x => x.Level == LogLevel.Info);
                int warm = grp.Count(x => x.Level == LogLevel.Warm);
                int error = grp.Count(x => x.Level == LogLevel.Error);

                LogStatistics ls = new LogStatistics();
                ls.AppId = grp.Key;
                ls.Debug = debug;
                ls.Info = info;
                ls.Warm = warm;
                ls.Error = error;
                ls.Time = statisticsTime;
                lss.Add(ls);
            }

            var log_ls_collection = MongoDataBase.GetCollection<LogStatistics>();
            log_ls_collection.InsertManyAsync(lss);


        }

        public void SetLogOnOff(LogOnOff on_off)
        {
            var collection = MongoDataBase.GetCollection<LogOnOff>();

            var old_on_off = collection.Find(a => a.AppId == on_off.AppId).Limit(1).FirstOrDefaultAsync().Result;

            if (old_on_off == null)
            {
                collection.InsertOneAsync(on_off);
            }
            else {
                on_off._id = old_on_off._id;
                collection.ReplaceOneAsync(a => a.AppId == on_off.AppId, on_off);
            }
        }

        public void SetOptions(List<Options> lst)
        {
            var log_ls_collection = MongoDataBase.GetCollection<Options>();

            foreach (var item in lst)
            {
                var filterBuilder = Builders<Options>.Filter;
                var filter = filterBuilder.Eq("Key", item.Key);
                var opt = log_ls_collection.Find(filter).FirstOrDefault();

                if (opt == null)
                {
                    log_ls_collection.InsertOneAsync(item);
                }
                else
                {
                    var updateBuilder = Builders<Options>.Update;
                    var update = updateBuilder.Set("Value", item.Value)
                                              .Set("UpdateTime", item.UpdateTime);
                    log_ls_collection.UpdateOneAsync(filter, update);
                }
            }
        }

        public void RecordAlerting(AlertingHistory alert)
        {
            var log_ls_collection = MongoDataBase.GetCollection<AlertingHistory>();
            log_ls_collection.InsertOneAsync(alert);
        }
    }
}