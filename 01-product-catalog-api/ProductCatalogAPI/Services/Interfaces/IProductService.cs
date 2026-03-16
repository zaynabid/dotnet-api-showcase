using ProductCatalogAPI.DTOs.Product;
using ProductCatalogAPI.Helpers;

namespace ProductCatalogAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDto>> GetAllAsync(PaginationParams pagination);
        Task<ProductResponseDto?> GetByIdAsync(int id);
        Task<IEnumerable<ProductResponseDto>> GetByCategoryIdAsync(int categoryId);
        Task<ProductResponseDto> CreateAsync(CreateProductDto dto);
        Task<ProductResponseDto?> UpdateAsync(int id, UpdateProductDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
