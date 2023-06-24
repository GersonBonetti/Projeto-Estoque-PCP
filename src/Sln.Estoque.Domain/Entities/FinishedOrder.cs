using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sln.Estoque.Domain.Entities
{
	public class FinishedOrder
	{
		public int Id { get; set; }
		public int OrderId { get; set; }
        public string LayoutCode { get; set; }
        public int Quantity { get; set; }
        public DateTime? DateFinish { get; set; }
        public int UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
