using ShopApp.Core;
using ShopApp.Shared.DTO;
namespace ShopApp.UseCases.Mapping
{
    public class CategoryMapping
    {
        public static CategoryDTO ToDto(Category category)
        {
            return new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        public static Category ToEntity(CategoryDTO dto)
        {
            return new Category
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description
            };
        }
    }
}
