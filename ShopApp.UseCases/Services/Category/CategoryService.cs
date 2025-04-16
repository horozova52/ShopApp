using Microsoft.EntityFrameworkCore;
using ShopApp.Infrastructure;
using ShopApp.Shared.DTO;
using ShopApp.UseCases.Mapping;

namespace ShopApp.UseCases.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryDTO>> GetAllAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories.Select(CategoryMapping.ToDto).ToList();
        }


        public async Task<CategoryDTO?> GetByIdAsync(Guid id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            return category == null ? null : CategoryMapping.ToDto(category);
        }
        public async Task CreateAsync(CategoryDTO dto)
        {
            var category = CategoryMapping.ToEntity(dto);
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CategoryDTO dto)
        {
            var category = CategoryMapping.ToEntity(dto);
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Core.Book>> GetBooksForCategoryAsync(Guid categoryId)
        {
            var category = await _context.Categories
                .Include(c => c.Books)
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            return category?.Books ?? new List<Core.Book>();
        }

    }
}
