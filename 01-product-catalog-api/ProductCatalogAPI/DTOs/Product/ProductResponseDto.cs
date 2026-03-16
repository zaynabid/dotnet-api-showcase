namespace ProductCatalogAPI.DTOs.Product
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string SKU { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        // sending category name for better client experience and to avoid extra API call to get category details
        public string CategoryName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
