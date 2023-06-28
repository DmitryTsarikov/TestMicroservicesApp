using System.Linq.Expressions;
using Catalog.Api.Data;
using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Repository;

public class ProductRepository : IProductRepository
{
    private readonly ICatalogContext _catalogContext;
    
    public ProductRepository(ICatalogContext context)
    {
        _catalogContext = context;
    }

    public async Task<Product> GetProduct(string id)
    {
        return await _catalogContext.Products.Find(GetByIdProductFilter(id)).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetProducts(string? name = null, string? categoryName = null)
    {
        return await _catalogContext.Products.Find(x =>
            (name == null || string.Equals(x.Name.Trim(), name.Trim(), StringComparison.CurrentCultureIgnoreCase))
            && (categoryName == null || string.Equals(x.Category.Trim(), categoryName.Trim(),
                StringComparison.CurrentCultureIgnoreCase))).ToListAsync();
    }

    public async Task CreateProduct(Product product)
    {
        await _catalogContext.Products.InsertOneAsync(product);
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        var result = await _catalogContext.Products.ReplaceOneAsync(filter: GetByIdProductFilter(product.Id),
            replacement: product);

        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProduct(string id)
    {
        var result = await _catalogContext.Products.DeleteOneAsync(GetByIdProductFilter(id));

        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    private static FilterDefinition<Product> GetByIdProductFilter(string id)
    {
        return Builders<Product>.Filter.Eq(p => p.Id, id);
    }
}