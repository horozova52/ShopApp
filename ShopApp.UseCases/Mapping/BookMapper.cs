using ShopApp.Core;
using ShopApp.Shared.DTO;

namespace ShopApp.UseCases.Mapping
{
    public class BookMapper
    {
        public static BookDTO ToDto(Book book)
        {
            return new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                Price = book.Price,
                CategoryId = book.CategoryId,
                CategoryName = book.Category?.Name
            };
        }

        public static Book ToEntity(BookDTO dto)
        {
            return new Book
            {
                Id = dto.Id,
                Title = dto.Title,
                Author = dto.Author,
                Description = dto.Description,
                Price = dto.Price,
                CategoryId = dto.CategoryId 
            };
        }

    }
}
