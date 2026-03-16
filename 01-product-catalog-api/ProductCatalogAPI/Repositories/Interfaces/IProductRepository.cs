using ProductCatalogAPI.Helpers;
using ProductCatalogAPI.Models;

namespace ProductCatalogAPI.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync(PaginationParams pagination);
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);
        Task<bool> IsSkuTakenByAnotherProductAsync(string sku, int? excludeProductId = null);
        Task<Product> CreateAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task<bool> SoftDeleteAsync(int id);
    }
}
