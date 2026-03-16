using System.ComponentModel.DataAnnotations;

namespace ProductCatalogAPI.DTOs.Product
{
    public class CreateProductDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be 0 or greater.")]
        public int Stock { get; set; }

        [Required]
        [MaxLength(100)]
        public string SKU { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }
    }
}
