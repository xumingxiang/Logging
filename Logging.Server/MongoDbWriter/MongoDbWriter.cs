using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging.Server.Writer
{


    //server=127.0.0.1:27017,127.0.0.1:27017;SafeMode=false
    internal class MongoDataBase
    {
        private const string DatabaseName = "plulog";
        private static MongoServer _server;

        internal static MongoServer GetServer()
        {
            var Mongo = System.Configuration.ConfigurationSettings.AppSettings["Mongo"];


            return _server ?? (_server = new MongoClient(Mongo).GetServer());
        }

        internal static MongoDatabase GetDataBase()
        {
            return GetServer().GetDatabase(DatabaseName);
        }

        public static MongoCollection<T> GetCollection<T>()
        {
            return GetDataBase().GetCollection<T>(typeof(T).Name);
        }

        /// <summary>
        /// 为T的指定属性创建索引(索引会减慢写速度)
        /// </summary>
        /// <typeparam name="T">当前文档对象类型</typeparam>
        /// <param name="fileds">要添加索引的属性名</param>
        public static void CreateIndexes<T>(params string[] fileds) where T : class, new()
        {
            GetCollection<T>().CreateIndex(fileds);
        }
    }

    public class MongoDbWriter
    {
        public static void WriteLog(List<LogEntity> logs)
        {
            MongoDataBase.GetCollection<LogEntity>().InsertBatch(logs, WriteConcern.Unacknowledged);
        }
    }
}
