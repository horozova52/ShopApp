using Microsoft.AspNetCore.Mvc;
using ShopApp.UseCases.Services.Category;
using ShopApp.Core;


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
    public async Task<ActionResult<List<Category>>> GetAll() =>
        await _service.GetAllAsync();

    [HttpGet("ping")]
    public string Ping() => "pong";

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> Get(string id)
    {
        var result = await _service.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Category category)
    {
        await _service.CreateAsync(category);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Category category)
    {
        if (id != category.Id) return BadRequest();
        await _service.UpdateAsync(category);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _service.DeleteAsync(id);
        return Ok();
    }
}
