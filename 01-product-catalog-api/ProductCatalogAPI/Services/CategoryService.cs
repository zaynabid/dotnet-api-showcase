using ProductCatalogAPI.DTOs.Category;
using ProductCatalogAPI.Models;
using ProductCatalogAPI.Repositories.Interfaces;
using ProductCatalogAPI.Services.Interfaces;

namespace ProductCatalogAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryService(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync()
        {
            var categories = await _categoryRepo.GetAllAsync();
            return categories.Select(MapToResponseDto);
        }

        public async Task<CategoryResponseDto?> GetByIdAsync(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            return category is null ? null : MapToResponseDto(category);
        }

        public async Task<CategoryResponseDto> CreateAsync(CreateCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };

            var created = await _categoryRepo.CreateAsync(category);
            return MapToResponseDto(created);
        }

        public async Task<CategoryResponseDto?> UpdateAsync(int id, UpdateCategoryDto dto)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category is null) return null;

            category.Name = dto.Name;
            category.Description = dto.Description;

            var updated = await _categoryRepo.UpdateAsync(category);
            return MapToResponseDto(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var hasProducts = await _categoryRepo.HasProductsAsync(id);
            if (hasProducts)
                throw new InvalidOperationException("Cannot delete a category that has active products.");

            return await _categoryRepo.DeleteAsync(id);
        }



        private static CategoryResponseDto MapToResponseDto(Category category) => new()
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CreatedAt = category.CreatedAt
        };
    }
}