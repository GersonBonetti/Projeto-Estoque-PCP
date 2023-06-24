using Sln.Estoque.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sln.Estoque.Domain.DTO
{
	public class UserDTO
	{
		public int id { get; set; }
		[DisplayName("Nome do usuário")]
        public string name { get; set; }

        [DisplayName("Username")]
        public string username { get; set; }

        [DisplayName("Senha")]
        public string password { get; set; }

        [DisplayName("Nível de acesso")]
        public int roleId { get; set; }
		public virtual Role? role { get; set; }


		public User mapToEntity()
		{
			return new User
			{
				Id = id,
				Name = name,
				Username = username,
				Password = password,
				RoleId = roleId,
				Role = role
			};
		}

		public UserDTO mapToDTO(User user)
		{
			return new UserDTO()
			{
				id = user.Id,
				name = user.Name,
				username = user.Username,
				password = user.Password,
				roleId = user.RoleId,
				role = user.Role
			};
		}
	}
}
