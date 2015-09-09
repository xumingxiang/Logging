using Logging.Server.DB;
using MongoDB.Driver;
using System;

namespace Logging.Server.Site
{
    public partial class InitMongo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // MongoDataBase.GetCollection<LogEntity>().
        }

        protected void btnCreateIndex_Click(object sender, EventArgs e)
        {
            var options = new CreateIndexOptions() { Unique = false, Background = true };


            var field1 = new StringFieldDefinition<LogEntity>("AppId");
            var indexDefinition1 = new IndexKeysDefinitionBuilder<LogEntity>().Ascending(field1);
            MongoDataBase.GetCollection<LogEntity>().Indexes.CreateOneAsync(indexDefinition1, options);

            var field2 = new StringFieldDefinition<LogEntity>("Level");
            var indexDefinition2 = new IndexKeysDefinitionBuilder<LogEntity>().Ascending(field2);
            MongoDataBase.GetCollection<LogEntity>().Indexes.CreateOneAsync(indexDefinition2, options);

            var field3 = new StringFieldDefinition<LogEntity>("CreateTime");
            var indexDefinition3 = new IndexKeysDefinitionBuilder<LogEntity>().Descending(field3);
            MongoDataBase.GetCollection<LogEntity>().Indexes.CreateOneAsync(indexDefinition3, options);


            var field4 = new StringFieldDefinition<LogTag>("TagName");
            var indexDefinition4 = new IndexKeysDefinitionBuilder<LogTag>().Ascending(field4);
            MongoDataBase.GetCollection<LogTag>().Indexes.CreateOneAsync(indexDefinition4, options);


            var field5 = new StringFieldDefinition<LogTag>("CreateTime");
            var indexDefinition5 = new IndexKeysDefinitionBuilder<LogTag>().Descending(field5);
            MongoDataBase.GetCollection<LogTag>().Indexes.CreateOneAsync(indexDefinition5, options);

            Response.Write("Create Index Success");
        }

        protected void btnViewIndex_Click(object sender, EventArgs e)
        {
            var log_indexs = MongoDataBase.GetCollection<LogEntity>().Indexes.ListAsync();

            var log_indexs_result = log_indexs.Result.ToListAsync().Result;

            foreach (var item in log_indexs_result)
            {
                Response.Write(item.ToString());
            }


            var tag_indexs = MongoDataBase.GetCollection<LogTag>().Indexes.ListAsync();

            var tag_indexs_result = tag_indexs.Result.ToListAsync().Result;

            foreach (var item in tag_indexs_result)
            {
                Response.Write(item.ToString());
            }
        }


    }
}