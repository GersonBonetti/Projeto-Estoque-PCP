using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sln.Estoque.Domain.Entities
{
	public class Role
	{
		public int Id { get; set; }
		public string Level { get; set; }
		public int LevelId { get; set; }
	}
}
