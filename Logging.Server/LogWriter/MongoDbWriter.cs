using Logging.Server.DB;
using System.Collections.Generic;

namespace Logging.Server.Writer
{
    internal class MongoDbWriter : ILogWriter
    {
        public void Write(IList<LogEntity> logs)
        {
            var collection = MongoDataBase.GetCollection<LogEntity>();
            collection.InsertManyAsync(logs);
        }
    }
}