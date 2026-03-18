using Microsoft.AspNetCore.Mvc;
using ProductCatalogAPI.DTOs.Category;
using ProductCatalogAPI.Services.Interfaces;

namespace ProductCatalogAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET /api/categories
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllAsync();
        return Ok(categories);
    }

    // GET /api/categories/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category is null)
            return NotFound(new { message = $"Category with ID {id} was not found." });

        return Ok(category);
    }

    // POST /api/categories
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
    {
        var created = await _categoryService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT /api/categories/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto dto)
    {
        var updated = await _categoryService.UpdateAsync(id, dto);

        if (updated is null)
            return NotFound(new { message = $"Category with ID {id} was not found." });

        return Ok(updated);
    }

    // DELETE /api/categories/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _categoryService.DeleteAsync(id);

        if (!deleted)
            return NotFound(new { message = $"Category with ID {id} was not found." });

        return NoContent();
    }
}