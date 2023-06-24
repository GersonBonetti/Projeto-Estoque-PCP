using Sln.Estoque.Domain.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sln.Estoque.Domain.DTO
{
    public class ProductDTO
    {
        [DisplayName("Id")]
        public int id { get; set; }

        [DisplayName("Código")]
		[Required(ErrorMessage = "O campo '{0}' é obrigatório.")]
		public string codeProduct { get; set; }

        /*[MaxLength(30, ErrorMessage = "O nome do produto precisa ter menos de 30 caracteres")]*/
        [DisplayName("Nome")]
		[Required(ErrorMessage = "O campo '{0}' é obrigatório.")]
		public string name { get; set; }

        [DisplayName("Nome Resumido")]
        public string? alias { get; set; }

		[DisplayName("Qtd")]
		[Required(ErrorMessage = "O campo 'Quantidade' é obrigatório.")]
        [Range(0, double.MaxValue, ErrorMessage = "A quantidade não pode ser negativa.")]
		public decimal? quantity { get; set; }

        public int? minimumQuantity { get; set; }

        [DisplayName("Unidade")]
		public int unitId { get; set; }

		[DisplayName("Preço")]
		[Required(ErrorMessage = "O campo 'Preço' é obrigatório.")]
        [Range(0.01, int.MaxValue, ErrorMessage = "O preço precisa ser maior que zero.")]
		public decimal? price { get; set; }

        [DisplayName("Categoria")]
        public int categoryId { get; set; }

		[DisplayName("Última Atualização")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yy HH:mm}")]
		public DateTime updateTime { get; set; }

        public virtual Unit? unit { get; set; }
		public virtual Category? category { get; set; }

        public Product mapToEntity()
        {
            return new Product
            {
                Id = id,
                CodeProduct = codeProduct,
                Name = name,
                Alias = alias,
                Quantity = quantity,
                UnitId = unitId,
                Price = price,
                CategoryId = categoryId,
				UpdateTime = updateTime,
                Unit = unit,
                Category = category
			};
        }

        public ProductDTO mapToDTO(Product product)
        {
            return new ProductDTO()
            {
                id = product.Id,
				codeProduct = product.CodeProduct,
                name = product.Name,
                alias = product.Alias,
                quantity = product.Quantity,
                unitId = product.UnitId,
                price = product.Price,
                categoryId = product.CategoryId,
				updateTime = product.UpdateTime,
                unit = product.Unit,
                category = product.Category
			};
        }

	}
}