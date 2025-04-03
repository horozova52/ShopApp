using ShopApp.Core;
namespace ShopApp.UseCases.Services.Category
{
    public interface ICategoryService
    {
        Task<List<Core.Category>> GetAllAsync();
        Task<Core.Category?> GetByIdAsync(string id);
        Task CreateAsync(Core.Category category);
        Task UpdateAsync(Core.Category category);
        Task DeleteAsync(string id);
    }
}
