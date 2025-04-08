using Microsoft.EntityFrameworkCore;
using ShopApp.Infrastructure;

namespace ShopApp.UseCases.Services.Book
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;
        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }
        // This method retrieves all books from the database, including their associated categories.
        public async Task<List<Core.Book>> GetAllAsync() =>
            await _context.Books.Include(b => b.Category).ToListAsync();
        // This method retrieves a book by its ID from the database, including its associated category.
        public async Task<Core.Book?> GetByIdAsync(Guid id) =>
            await _context.Books.Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);
        // This method creates a new book in the database.
        public async Task CreateAsync(Core.Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }
        // This method updates an existing book in the database.
        public async Task UpdateAsync(Core.Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }
        // This method deletes a book by its ID from the database.
        public async Task DeleteAsync(Guid id)
        {
            var book = await GetByIdAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }
    }
}
