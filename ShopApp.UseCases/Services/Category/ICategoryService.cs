using ShopApp.Shared.DTO;
using ShopApp.Core;

public interface ICategoryService
{
    Task<List<CategoryDTO>> GetAllAsync();
    Task<CategoryDTO?> GetByIdAsync(Guid id);
    Task CreateAsync(CategoryDTO category);
    Task UpdateAsync(CategoryDTO category);
    Task DeleteAsync(Guid id);
    Task<List<Book>> GetBooksForCategoryAsync(Guid categoryId);
}
