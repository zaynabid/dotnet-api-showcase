using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Data;
using ProductCatalogAPI.Helpers;
using ProductCatalogAPI.Models;
using ProductCatalogAPI.Repositories.Interfaces;

namespace ProductCatalogAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync(PaginationParams pagination)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive);

            if (!string.IsNullOrWhiteSpace(pagination.Search))
                query = query.Where(p => p.Name.Contains(pagination.Search));

            if (pagination.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == pagination.CategoryId.Value);

            return await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId && p.IsActive)
                .ToListAsync();
        }

        public async Task<bool> IsSkuTakenByAnotherProductAsync(string sku, int? excludeProductId = null)
        {
            var query = _context.Products.Where(p => p.SKU == sku);

            if (excludeProductId.HasValue)
                query = query.Where(p => p.Id != excludeProductId.Value);

            return await query.AnyAsync();
        }

        public async Task<Product> CreateAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        // assuming product is already tracked by the EF in the service layer
        public async Task<Product> UpdateAsync(Product product)
        {
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product is null) return false;

            product.IsActive = false;
            product.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
