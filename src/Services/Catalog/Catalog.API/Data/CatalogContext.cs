using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data;

public class CatalogContext : ICatalogContext
{    
    public CatalogContext(IConfiguration configuration)
    {
        string connectionString = configuration.GetValue<string>("DatabaseSettings:ConnectionString");
        string dbName = configuration.GetValue<string>("DatabaseSettings:DatabaseName");
        string collectionName = configuration.GetValue<string>("DatabaseSettings:CollectionName");

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(dbName);

        Products = database.GetCollection<Product>(collectionName);
        CatalogContextSeed.SeedData(Products);
    }

    public IMongoCollection<Product> Products { get; }
}
