using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Stark.Starter.Mongodb.Config;

namespace Stark.Starter.Mongodb.Context
{
    public class MongoContext: IMongoContext
    {
        // 基于MongoDB的最佳实践对于MongoClient最好设置为单例注入，因为在MongoDB.Driver中MongoClient已经被设计为线程安全可以被多线程共享，
        //  这样可还以避免反复实例化MongoClient带来的开销，避免在极端情况下的性能低下。
        public MongoContext(IOptions<MongoOptions> options,
            ILogger<MongoContext> logger)
        {
            try
            {
                var mongoConfig = options.Value;
                var settings = MongoClientSettings.FromUrl(
                    new MongoUrl(mongoConfig.ConnectionString)
                );
                var mongoClient = new MongoClient(settings);
                Db = mongoClient.GetDatabase(mongoConfig.DatabaseName);
            }
            catch(Exception ex)
            {
                logger.LogError(ex,"Mongodb Initialized failed.");
                throw ex;
            }
        }

        public IMongoDatabase Db { get; }
    }
}