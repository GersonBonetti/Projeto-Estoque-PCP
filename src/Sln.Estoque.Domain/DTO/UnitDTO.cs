using Sln.Estoque.Domain.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sln.Estoque.Domain.DTO
{
    public class UnitDTO
    {
        [DisplayName("Id")]
        public int id { get; set; }

        [DisplayName("Nome")]
        public string name { get; set; }

        public Unit mapToEntity()
        {
            return new Unit
            {
               Id = id,
               Name = name
			};
        }

        public UnitDTO mapToDTO(Unit unit)
        {
            return new UnitDTO()
            {
                id = unit.Id,
                name = unit.Name
			};
        }
    }
}