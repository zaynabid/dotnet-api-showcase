using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Models;

namespace ProductCatalogAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Price must be > 0
            modelBuilder.Entity<Product>()
                .ToTable(tb => tb.HasCheckConstraint("CK_Product_Price", "Price > 0"));

            // Stock must be >= 0
            modelBuilder.Entity<Product>()
                .ToTable(tb => tb.HasCheckConstraint("CK_Product_Stock", "Stock >= 0"));

            // SKU must be unique
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.SKU)
                .IsUnique();

            // One Category has many Products
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
