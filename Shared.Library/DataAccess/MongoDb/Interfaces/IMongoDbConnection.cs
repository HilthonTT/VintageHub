namespace Shared.Library.DataAccess.MongoDb.Interfaces;
public interface IMongoDbConnection
{
    GridFSBucket Bucket { get; }
    MongoClient Client { get; }
    string DbName { get; }
}