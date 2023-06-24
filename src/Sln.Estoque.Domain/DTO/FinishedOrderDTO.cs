using Sln.Estoque.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sln.Estoque.Domain.DTO
{
	public class FinishedOrderDTO
	{
		public int id { get; set; }
		public int orderId { get; set; }
		public string layoutCode { get; set; }
		public int quantity { get; set; }

		[DisplayFormat(DataFormatString = "{0:dd/MM/yy HH:mm}")]
		public DateTime? dateFinish { get; set; }
		public int userId { get; set; }
        public virtual User? user { get; set; }

        public FinishedOrder mapToEntity()
		{
			return new FinishedOrder
			{
				Id = id,
				OrderId = orderId,
				LayoutCode = layoutCode,
				Quantity = quantity,
				DateFinish = dateFinish,
				UserId = userId,
				User = user
			};
		}

		public FinishedOrderDTO mapToDTO(FinishedOrder order)
		{
			return new FinishedOrderDTO()
			{
				id = order.Id,
				orderId = order.OrderId,
				layoutCode = order.LayoutCode,
				quantity = order.Quantity,
				dateFinish = order.DateFinish,
				userId = order.UserId,
				user = order.User
			};
		}
	}
}
