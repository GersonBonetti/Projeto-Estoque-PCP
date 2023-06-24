using Sln.Estoque.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sln.Estoque.Domain.DTO
{
	public class RoleDTO
	{
		public int id { get; set; }
		public string level { get; set; }
		public int levelId { get; set; }

		public Role mapToEntity()
		{
			return new Role
			{
				Id = id,
				Level = level,
				LevelId = levelId
			};
		}

		public RoleDTO mapToDTO(Role role)
		{
			return new RoleDTO()
			{
				id = role.Id,
				level = role.Level,
				levelId = role.LevelId
			};
		}
	}
}
