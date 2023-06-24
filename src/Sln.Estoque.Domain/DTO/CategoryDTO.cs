using Sln.Estoque.Domain.Entities;
using System.ComponentModel;

namespace Sln.Estoque.Domain.DTO
{
    public class CategoryDTO
    {
        [DisplayName("Id")]
        public int id { get; set; }

		[DisplayName("Nome")]
		public string name { get; set; }

        public Category mapToEntity()
        {
            return new Category
            {
                Id = id,
                Name = name
            };
        }

        public CategoryDTO mapToDTO(Category category)
        {
            return new CategoryDTO
            {
                id = category.Id,
                name = category.Name
            };
        }
    }
}