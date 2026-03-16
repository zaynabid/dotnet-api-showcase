using System.ComponentModel.DataAnnotations;

namespace ProductCatalogAPI.DTOs.Category
{
    public class CreateCategoryDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
