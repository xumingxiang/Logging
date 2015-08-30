using MongoDB.Bson;
using MongoDB.Driver;

namespace Logging.Server.DB
{

    //server=127.0.0.1:27017,127.0.0.1:27017;SafeMode=false
    internal class MongoDataBase
    {
        private const string DatabaseName = "plulog";
        private static MongoServer _server;

        public static MongoClient _client;

        internal static MongoServer GetServer()
        {
            var Mongo = System.Configuration.ConfigurationManager.AppSettings["Mongo"];

            return _server ?? (_server = new MongoClient(Mongo).GetServer());
        }

        public static MongoClient GetClient()
        {
          
            if (_client == null)
            {
                var mongo = System.Configuration.ConfigurationManager.AppSettings["Mongo"];
                _client = new MongoClient(mongo);
                _client.Settings.WriteConcern = WriteConcern.Unacknowledged;
            }
            return _client;
        }

        internal static IMongoDatabase GetDataBase()
        {
           
            return GetClient().GetDatabase(DatabaseName);
        }

        internal static IMongoCollection<BsonDocument> GetCollection()
        {
            return GetDataBase().GetCollection<BsonDocument>("LogEntity");
        }

        public static IMongoCollection<T> GetCollection<T>()
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
            var c = GetServer().GetDatabase(DatabaseName).GetCollection<T>(typeof(T).Name);
            c.CreateIndex(fileds);
        }

       
    }
}
