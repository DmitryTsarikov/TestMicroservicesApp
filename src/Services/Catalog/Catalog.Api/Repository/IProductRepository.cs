using Catalog.Api.Entities;

namespace Catalog.Api.Repository;

public interface IProductRepository
{
    Task<Product> GetProduct(string id);

    Task<IEnumerable<Product>> GetProducts(string? name = null, string? categoryName = null);

    Task CreateProduct(Product product);

    Task<bool> UpdateProduct(Product product);

    Task<bool> DeleteProduct(string id);
}