using Microsoft.AspNetCore.Mvc;
using ShopApp.UseCases.Services.Category;
using ShopApp.Core;
using Microsoft.EntityFrameworkCore;
using ShopApp.Shared.DTO;
using ShopApp.UseCases.Mapping;

namespace ShopApp.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryDTO>>> GetAll() =>
        await _service.GetAllAsync();


    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDTO>> Get(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryDTO category)
    {
        await _service.CreateAsync(category);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, CategoryDTO category)
    {
        if (id != category.Id) return BadRequest();
        await _service.UpdateAsync(category);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return Ok();
    }
    [HttpGet("{id}/books")]
    public async Task<IActionResult> GetBooksForCategory(Guid id)
    {
        var books = await _service.GetBooksForCategoryAsync(id);
        var bookDtos = books.Select(BookMapper.ToDto).ToList();
        return Ok(bookDtos); 
    }

}
