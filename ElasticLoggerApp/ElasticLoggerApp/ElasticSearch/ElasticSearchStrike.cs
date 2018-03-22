using ElasticLoggerApp.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticLoggerApp.ElasticSearch
{
    public class ElasticSearchStrike
    {
        private static readonly ConnectionSettings connSettings = new ConnectionSettings(new Uri("http://localhost:9200/"))
                      .DefaultIndex("modify-log-index");
       
                     //   .MapDefaultTypeIndices(m => m
                    //  .Add(typeof(ModifyLog), "change_history"));

        private static readonly ElasticClient elasticClient = new ElasticClient(connSettings);

        public static void CheckExistsAndInsert(ModifyLog log)
        {
            if (!elasticClient.IndexExists("change_log").Exists)
            {
                var indexSettings = new IndexSettings();
                indexSettings.NumberOfReplicas = 1;
                indexSettings.NumberOfShards = 3;


                var createIndexDescriptor = new CreateIndexDescriptor("modify-log-index")
               .Mappings(ms => ms.Map<ModifyLog>(m => m.AutoMap())                      )
               .InitializeUsing(new IndexState() { Settings = indexSettings })
               .Aliases(a => a.Alias("change_log"));

                var response = elasticClient.CreateIndex(createIndexDescriptor);
            }
            elasticClient.Index<ModifyLog>(log, idx => idx.Index("modify-log-index"));
        }

    }
}
