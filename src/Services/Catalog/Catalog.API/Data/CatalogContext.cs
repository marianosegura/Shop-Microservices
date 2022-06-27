using Catalog.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;


namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public IMongoCollection<Product> Products { get; }
        

        public CatalogContext(IConfiguration config)  // inkect config to access db string vars
        {
            string connectionString = config.GetValue<string>("DatabaseSettings:ConnectionString");
            string databaseName = config.GetValue<string>("DatabaseSettings:DatabaseName");
            string collectionName = config.GetValue<string>("DatabaseSettings:CollectionName");
            
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);

            Products = database.GetCollection<Product>(collectionName);  // provide entity class to map objects
            CatalogContextSeed.SeedData(Products);  // seed data if not empty
        }
    }
}
