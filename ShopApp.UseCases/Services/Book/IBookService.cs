
namespace ShopApp.UseCases.Services.Book
{
    public interface IBookService
    {
        Task<List<Core.Book>> GetAllAsync();
        Task<Core.Book?> GetByIdAsync(Guid id);
        Task CreateAsync(Core.Book book);

        Task UpdateAsync(Core.Book book);
        Task DeleteAsync(Guid id);

    }
}
