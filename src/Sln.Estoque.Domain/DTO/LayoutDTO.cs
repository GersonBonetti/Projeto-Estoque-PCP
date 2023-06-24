using Sln.Estoque.Domain.Entities;
using System.ComponentModel;

namespace Sln.Estoque.Domain.DTO
{
    public class LayoutDTO
    {
        public int id { get; set; }

        [DisplayName("Nome do Layout")]
        public string viewName { get; set; }

		[DisplayName("String de Busca")]
		public string fileName { get; set; }

		[DisplayName("Multiplicador")]
		public int multiplier { get; set; }

		[DisplayName("Método")]
		public int method { get; set; }

		[DisplayName("Posição da Quantidade")]
		public int quantityPosition { get; set; }

        public Layout mapToEntity()
        {
            return new Layout
            {
                Id = id,
                ViewName = viewName,
                FileName = fileName,
                Multiplier = multiplier,
                Method = method,
                QuantityPosition = quantityPosition
            };
        }

        public LayoutDTO mapToDTO(Layout layout)
        {
            return new LayoutDTO()
            {
                id = layout.Id,
                viewName = layout.ViewName,
                fileName = layout.FileName,
                multiplier = layout.Multiplier,
                method = layout.Method,
                quantityPosition = layout.QuantityPosition
            };
        }
    }
}
