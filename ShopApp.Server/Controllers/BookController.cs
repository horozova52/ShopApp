using Microsoft.AspNetCore.Mvc;
using ShopApp.Shared.DTO;
using ShopApp.UseCases.Mapping;
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
        public async Task<ActionResult<List<BookDTO>>> GetAll()
        {
            var books = await _service.GetAllAsync();
            var bookDto = books.Select(BookMapper.ToDto).ToList();
            return Ok(bookDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> Get(Guid id)
        {
            var book = await _service.GetByIdAsync(id);
            if (book == null) return NotFound();
            return Ok(BookMapper.ToDto(book));
        }
        [HttpPost]
        public async Task<IActionResult> Create(BookDTO dto)
        {
            var book = BookMapper.ToEntity(dto);
            await _service.CreateAsync(book);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, BookDTO dto)
        {
            if (id != dto.Id) return BadRequest();
            var book = BookMapper.ToEntity(dto);
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
