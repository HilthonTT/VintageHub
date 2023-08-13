using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Server.Library.DataAccess.MongoDb.Interfaces;
public interface IMongoDbConnection
{
    GridFSBucket Bucket { get; }
    MongoClient Client { get; }
    string DbName { get; }
}