using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Sln.Estoque.Domain.DTO;
using Sln.Estoque.Domain.Entities;
using Sln.Estoque.Domain.IServices;
using Sln.Estoque.Domain.Util;
using Sln.Estoque.Web.Auth;

namespace Sln.Estoque.Web.Controllers
{
	public class PcpController : Controller
	{
		private readonly IPcpService _pcpService;
		private readonly IUserService _userService;
		private readonly UserRoleService _userRoleService;
		private readonly ConnectionStringsConfig _connectionStringsConfig;

		public PcpController(IPcpService pcpService, IUserService userService, UserRoleService userRoleService, 
							 ConnectionStringsConfig connectionStringsConfig)
		{
			_pcpService = pcpService;
			_userService = userService;
			_userRoleService = userRoleService;
			_connectionStringsConfig = connectionStringsConfig;
		}

		[Authorize(Roles = "Alta,Média,Baixa")]
		public async Task<IActionResult> Index()
		{
			var orders = await FindAllToday();

			HashSet<int> orderIds = new();
            foreach (var item in orders)
            {
				orderIds.Add(item.orderId);
            }
			var ordersSorted = orders.OrderByDescending(x => x.dateFinish);

			ViewBag.Quantity = orderIds.Count;
			ViewBag.UserRole = _userRoleService.GetUserRole();

			return View(ordersSorted);
		}

		[NonAction]
		[Authorize(Roles = "Alta,Média,Baixa")]
		public async Task<IEnumerable<FinishedOrderDTO>> FindAllToday()
		{
			string dateFormat = "dd/MM/yy";
			string today = DateTime.Today.ToString(dateFormat);

			return await Task.FromResult(_pcpService.FindAll().Where(item => item.dateFinish?.ToString(dateFormat) == today));
		}

		[HttpPost]
		[Authorize(Roles = "Alta,Média,Baixa")]
		public async Task<JsonResult> Register(string orderId, string layoutCode, string quantity)
		{
			ReturnJsonGeneric ret = new()
			{
				status = "Error",
				code = "401"
			};

			FinishedOrderDTO order = new();

			var userCookie = HttpContext?.User?.Identity?.Name;

			if (userCookie != null)
			{
				UserDTO? user = _userService.FindAll().FirstOrDefault(x => x.name == userCookie.ToString());

				if (user != null) {
					order.orderId = int.Parse(orderId);
					order.layoutCode = layoutCode.Length < 4 ? layoutCode.PadLeft(4, '0') : layoutCode;
					order.quantity = int.Parse(quantity);
					order.userId = user.id;
					order.dateFinish = DateTime.Now;

					if (await _pcpService.Save(order) > 0)
					{
						ret.status = "success";
						ret.code = "200";
						return Json(ret);
					}
				}
			}
			return Json(ret);
		}

		[HttpPost]
		[Authorize(Roles = "Alta,Média")]
		public async Task<JsonResult> Edit(int id, string orderId, string layoutCode, string quantity)
		{
			ReturnJsonGeneric ret = new()
			{
				status = "Error",
				code = "401"
			};

			FinishedOrderDTO order = await _pcpService.FindById(id);
			if (order != null)
			{
				order.orderId = int.Parse(orderId);
				order.layoutCode = layoutCode.Length < 4 ? layoutCode.PadLeft(4, '0') : layoutCode;
				order.quantity = int.Parse(quantity);

				if (await _pcpService.Save(order) > 0)
				{
					ret.status = "Success";
					ret.code = "200";
					ret.info = $"{orderId}&{order.layoutCode}&{quantity}";
					return Json(ret);
				}
			}
			return Json(ret);
		}

		[HttpPost]
		[Authorize(Roles = "Alta,Média")]
		public async Task<JsonResult> Delete(int id)
		{
			ReturnJsonGeneric ret = new()
			{
				status = "Error",
				code = "400"
			};

			if (await _pcpService.Delete(id) > 0)
			{
				ret.status = "Success";
				ret.code = "200";
				return Json(ret);
			}

			return Json(ret);
		}

		[HttpPost]
		[Authorize(Roles = "Alta,Média,Baixa")]
		public List<FinishedOrderDTO> Search(int ordernumber)
		{
			List<FinishedOrderDTO> orderList = _pcpService.FindAll().Where(x => x.orderId == ordernumber).ToList();
			if (orderList.Count > 0)
			{
				return orderList;
			}
			else
			{
				List<FinishedOrderDTO> nullOrder = new()
				{
					new FinishedOrderDTO
					{
						orderId = 0,
						layoutCode = "0",
						quantity = 0,
						dateFinish = DateTime.Now,
						userId = 0,
						user = new User
						{
							Name = "N/A"
						}
					}
				};
				return nullOrder;
			}
		}

		[HttpGet]
		[Authorize(Roles = "Alta")]
		public FileResult ExportCSV(string startDate, string endDate)
		{
			string connectionString = _connectionStringsConfig.SqlConnectionString;

			string sql = "SELECT O.Id, " +
						 "       O.OrderId, " +
						 "       O.LayoutCode, " +
						 "       O.Quantity, " +
						 "       CONVERT(varchar(10), O.DateFinish, 5) AS Date, " +
						 "       CONVERT(time(0), O.DateFinish, 108) AS Time, " +
						 "       U.Name AS Name " +
						 "FROM FinishedOrders O " +
						 "INNER JOIN Users U ON O.UserId = U.Id " +
						 "WHERE CONVERT(date, O.DateFinish) >= CONVERT(date, @startDate, 103) " +
						 "  AND CONVERT(date, O.DateFinish) <= CONVERT(date, @endDate, 103) " +
						 "ORDER BY O.Id;";

			try
			{
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@startDate", startDate);
						command.Parameters.AddWithValue("@endDate", endDate);

						using (SqlDataReader reader = command.ExecuteReader())
						{
							using (MemoryStream memoryStream = new MemoryStream())
							{
								using (StreamWriter writer = new StreamWriter(memoryStream))
								{
									writer.WriteLine("Id;Número do Pedido;Código do Layout;Quantidade;Dia;Hora;Usuário");

									while (reader.Read())
									{
										writer.WriteLine(reader["Id"] + ";"
														 + reader["OrderId"] + ";"
														 + reader["LayoutCode"] + ";"
														 + reader["Quantity"] + ";"
														 + reader["Date"] + ";"
														 + reader["Time"] + ";"
														 + reader["Name"]);
									}
								}

								byte[] fileBytes = memoryStream.ToArray();
								string contentType = "text/plain";
								string fileName = $"Baixas-de-Pedidos-{DateTime.Now:dd-MM}.txt";
								return File(fileBytes, contentType, fileName);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Erro ao exportar os dados: " + ex.Message);
				return null;
			}
		}
	}
}