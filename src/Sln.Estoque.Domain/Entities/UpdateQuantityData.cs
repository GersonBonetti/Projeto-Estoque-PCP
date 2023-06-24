using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sln.Estoque.Domain.Entities
{
	public class UpdateQuantityData
	{
        public int Id { get; set; }
        public decimal QtyInput { get; set; }
        public string Operation { get; set; }
    }
}
