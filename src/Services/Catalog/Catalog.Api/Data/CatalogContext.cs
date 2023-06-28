﻿using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Data;

public class CatalogContext : ICatalogContext
{
    public IMongoCollection<Product> Products { get; }

    public CatalogContext(IConfiguration configuration)
    {
        var mongoClient = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var database = mongoClient.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

        Products = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
        CatalogContextSeed.SeedData(Products);
    }
}