using MongoDB.Driver;

namespace Stark.Starter.Mongodb.Context
{
    public interface IMongoContext
    {
        IMongoDatabase Db { get; }
    }
}