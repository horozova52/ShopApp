using Microsoft.AspNetCore.Mvc;
using ShopApp.UseCases.Services.Book;

namespace ShopApp.Server.Controller
{
    
        [ApiController]
        [Route("api/[controller]")]
        public class BookController : ControllerBase
        {
            private readonly IBookService _service;
            public BookController(IBookService service)
            {
                _service = service;
            }
            [HttpGet]
            public async Task<ActionResult<List<Core.Book>>> GetAll() =>
                await _service.GetAllAsync();
            
            [HttpGet("{id}")]
            public async Task<ActionResult<Core.Book>> Get(Guid id)
            {
                var result = await _service.GetByIdAsync(id);
                return result == null ? NotFound() : Ok(result);
            }
            [HttpPost]
            public async Task<IActionResult> Create(Core.Book book)
            {
                await _service.CreateAsync(book);
                return Ok();
            }
            [HttpPut("{id}")]
            public async Task<IActionResult> Update(Guid id, Core.Book book)
            {
                if (id != book.Id) return BadRequest();
                await _service.UpdateAsync(book);
                return Ok();
            }
            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(Guid id)
            {
                await _service.DeleteAsync(id);
                return Ok();
            }
        }
    
}
