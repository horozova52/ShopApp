using ShopApp.Infrastructure;
using ShopApp.Core;
using Microsoft.EntityFrameworkCore; 

namespace ShopApp.UseCases.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Core.Category>> GetAllAsync() =>
            await _context.Categories.Include(c => c.Books).ToListAsync();

        public async Task<Core.Category?> GetByIdAsync(string id) =>
            await _context.Categories.Include(c => c.Books)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task CreateAsync(Core.Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Core.Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var category = await GetByIdAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
