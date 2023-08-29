namespace Shared.Library.DataAccess.MongoDb;
public class MongoDbConnection : IMongoDbConnection
{
    private readonly IConfiguration _config;
    private readonly IMongoDatabase _db;
    private static readonly string _connectionId = "MongoDB";
    public string DbName { get; private set; }
    public MongoClient Client { get; private set; }
    public GridFSBucket Bucket { get; private set; }

    public MongoDbConnection(IConfiguration config)
    {
        _config = config;
        Client = new MongoClient(_config.GetConnectionString(_connectionId));
        DbName = _config["MongoDbName"];
        _db = Client.GetDatabase(DbName);
        Bucket = new GridFSBucket(_db);
    }
}
