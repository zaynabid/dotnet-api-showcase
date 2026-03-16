using ProductCatalogAPI.DTOs.Product;
using ProductCatalogAPI.Helpers;
using ProductCatalogAPI.Models;
using ProductCatalogAPI.Repositories.Interfaces;
using ProductCatalogAPI.Services.Interfaces;

namespace ProductCatalogAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;

        public ProductService(IProductRepository productRepo, ICategoryRepository categoryRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllAsync(PaginationParams pagination)
        {
            var products = await _productRepo.GetAllAsync(pagination);
            return products.Select(MapToResponseDto);
        }

        public async Task<ProductResponseDto?> GetByIdAsync(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            return product is null ? null : MapToResponseDto(product);
        }

        public async Task<IEnumerable<ProductResponseDto>> GetByCategoryIdAsync(int categoryId)
        {
            var products = await _productRepo.GetByCategoryIdAsync(categoryId);
            return products.Select(MapToResponseDto);
        }

        public async Task<ProductResponseDto> CreateAsync(CreateProductDto dto)
        {
            var categoryExists = await _categoryRepo.ExistsAsync(dto.CategoryId);
            if (!categoryExists)
                throw new KeyNotFoundException($"Category with ID {dto.CategoryId} was not found.");

            var skuTaken = await _productRepo.IsSkuTakenByAnotherProductAsync(dto.SKU);
            if (skuTaken)
                throw new InvalidOperationException($"SKU '{dto.SKU}' is already in use.");

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                SKU = dto.SKU,
                CategoryId = dto.CategoryId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var created = await _productRepo.CreateAsync(product);
            return MapToResponseDto(created);
        }

        public async Task<ProductResponseDto?> UpdateAsync(int id, UpdateProductDto dto)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product is null) return null;

            var categoryExists = await _categoryRepo.ExistsAsync(dto.CategoryId);
            if (!categoryExists)
                throw new KeyNotFoundException($"Category with ID {dto.CategoryId} was not found.");

            var skuTaken = await _productRepo.IsSkuTakenByAnotherProductAsync(dto.SKU, id);
            if (skuTaken)
                throw new InvalidOperationException($"SKU '{dto.SKU}' is already in use.");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.SKU = dto.SKU;
            product.CategoryId = dto.CategoryId;
            product.UpdatedAt = DateTime.UtcNow;

            var updated = await _productRepo.UpdateAsync(product);
            return MapToResponseDto(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _productRepo.SoftDeleteAsync(id);
        }


        private static ProductResponseDto MapToResponseDto(Product product) => new()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            SKU = product.SKU,
            IsActive = product.IsActive,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? string.Empty,
            CreatedAt = product.CreatedAt
        };
    }
}