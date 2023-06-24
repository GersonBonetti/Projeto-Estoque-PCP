using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Sln.Estoque.Domain.DTO;
using Sln.Estoque.Domain.Entities;
using Sln.Estoque.Domain.IServices;
using Sln.Estoque.Domain.Util;
using Sln.Estoque.Web.Auth;
using System.IO;

namespace Sln.Estoque.Web.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductService _service;
		private readonly ICategoryService _categoryService;
		private readonly IUnitService _unitService;
		private readonly UserRoleService _userRoleService;
		private readonly ConnectionStringsConfig _connectionStringsConfig;

		public ProductController(IProductService service, ICategoryService categoryService, IUnitService unitService, UserRoleService userRoleService, ConnectionStringsConfig connectionStringsConfig)
		{
			_service = service;
			_categoryService = categoryService;
			_unitService = unitService;
			_userRoleService = userRoleService;
			_connectionStringsConfig = connectionStringsConfig;
		}

		[Authorize(Roles = "Alta,Média,Baixa,Compras")]
		public ActionResult Index()
		{
			ViewBag.UserRole = _userRoleService.GetUserRole();
			var listItens = _service.FindAll();
			if ((ViewBag.UserRole == "Alta") || (ViewBag.UserRole == "Compras"))
			{
				var totalPrice = listItens.Sum(p => (p.quantity ?? 0) * (p.price ?? 0m));
				ViewBag.TotalPrice = totalPrice.ToString("N2");
			}
			return View(listItens);
		}

		[Authorize(Roles = "Alta,Média,Baixa,Compras")]
		public JsonResult GetCategory(string category)
		{
			var retCategory = new ReturnJsonGeneric
			{
				status = "Success",
				code = "200",
				id = "0"
			};

			int id = 0;
			var categoryId = _categoryService.FindAll();
			foreach (var item in categoryId)
			{
				if (item.name == category)
				{
					id = item.id;
					retCategory.id = item.id.ToString();
				}
			}
			return Json(retCategory);
		}

		[Authorize(Roles = "Alta,Média,Baixa,Compras")]
		public JsonResult GetUnit(string unit)
		{
			var retUnit = new ReturnJsonGeneric
			{
				status = "Success",
				code = "200",
				id = "0"
			};

			int id = 0;
			var unitId = _unitService.FindAll();
			foreach (var item in unitId)
			{
				if (item.name == unit)
				{
					id = item.id;
					retUnit.id = item.id.ToString();
				}
			}
			return Json(retUnit);
		}

		[Authorize(Roles = "Alta,Média,Compras")]
		public IActionResult Create()
		{
			ViewData["categoryId"] = new SelectList(_categoryService.FindAll(), "id", "name", "Select...");
			ViewBag.UnitId = new SelectList(_unitService.FindAll(), "id", "name", "Select...");
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "Alta,Média,Compras")]
		public async Task<IActionResult> Create([Bind("id, codeProduct, name, alias, quantity, unitId, price, categoryId")] ProductDTO product)
		{
			product.updateTime = DateTime.Now;
			if (ModelState.IsValid)
			{
				if (await _service.Save(product) > 0)
				{
					return RedirectToAction(nameof(Index));
				}
			}
			ViewData["categoryId"] = new SelectList(_categoryService.FindAll(), "id", "name", product.categoryId);
			ViewBag.UnitId = new SelectList(_unitService.FindAll(), "id", "name", "Select...");
			return View(product);
		}

		[Authorize(Roles = "Alta,Média,Compras")]
		public async Task<IActionResult> Edit(int id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var product = await _service.FindById(id);
			ViewBag.Categorias = new SelectList(_categoryService.FindAll(), "id", "name", product.categoryId);
			ViewBag.Unidades = new SelectList(_unitService.FindAll(), "id", "name", product.unitId);

			return View(product);
		}

		[HttpPost]
		[Authorize(Roles = "Alta,Média,Compras")]
		public async Task<IActionResult> Edit(int? id, [Bind("id, codeProduct, name, alias, quantity, unitId, price, categoryId")] ProductDTO product)
		{
			var sPreco = product.price.ToString();
			if (!(id == product.id))
			{
				return NotFound();
			}
			if (sPreco.Length > 2)
			{
				sPreco.Insert(2, ".");
				product.price = Decimal.Parse(sPreco);
			}

			product.updateTime = DateTime.Now;

			if (ModelState.IsValid)
			{
				if (await _service.Save(product) > 0)
				{
					return RedirectToAction(nameof(Index));
				}
			}
			return View(product);
		}

		[HttpPost]
		[Authorize(Roles = "Alta")]
		public async Task<JsonResult> Delete(int? id)
		{
			var retDel = new ReturnJsonGeneric
			{
				status = "Success",
				code = "200"
			};

			try
			{
				if (await _service.Delete(id ?? 0) <= 0)
				{
					retDel = new ReturnJsonGeneric
					{
						status = "Error",
						code = "400"
					};
				}
			}
			catch (Exception ex)
			{
				retDel = new ReturnJsonGeneric
				{
					status = ex.Message,
					code = "500"
				};
			}
			return Json(retDel);
		}

		[HttpPost]
		[Authorize(Roles = "Alta,Média,Baixa,Compras")]
		public async Task<JsonResult> UpdateQuantity([FromBody] UpdateQuantityData data)
		{
			var retDel = new ReturnJsonGeneric
			{
				status = "Error",
				code = "400"
			};

			var product = await _service.FindById(data.Id);

			if (data.Operation == "add")
			{
				product.quantity += data.QtyInput;
			}
			else if (data.Operation == "subtract")
			{
				if (product.quantity >= data.QtyInput)
				{
					product.quantity -= data.QtyInput;
				}
				else
				{
					retDel.status = "Error";
					retDel.code = "410";
					return Json(retDel);
				}
			}

			if (ModelState.IsValid)
			{
				product.updateTime = DateTime.Now;
				if (await _service.Save(product) > 0)
				{
					retDel.status = "Success";
					retDel.code = "200";
					retDel.info = $"{product.quantity}";
					retDel.time = product.updateTime.ToString("dd/MM/yy HH:mm");
				}
			}
			return Json(retDel);
		}

		[HttpPost]
		[Authorize(Roles = "Alta,Média,Baixa,Compras")]
		public async Task<ProductDTO> FindById(int id)
		{
			var productToUpdt = await _service.FindById(id);
			return productToUpdt;
		}

		[Authorize(Roles = "Alta,Compras")]
		public FileResult ExportCSV()
		{
			string connectionString = _connectionStringsConfig.SqlConnectionString;

			string sql = "SELECT P.Id, " +
						"       P.CodeProduct, " +
						"       P.Name, " +
						"       P.Price, " +
						"       P.Quantity, " +
						"       (P.Price * P.Quantity) AS TotalPrice, " +
						"       U.Name AS UnitName, " +
						"       C.Name AS CategoryName, " +
						"       CONVERT(varchar(10), P.UpdateTime, 5) AS UpdateDate, " +
						"       CONVERT(time(0), P.UpdateTime, 108) AS UpdateTime " +
						" FROM Products P " +
						"       INNER JOIN Units U ON P.UnitId = U.Id " +
						"       INNER JOIN Categories C ON P.CategoryId = C.Id " +
						" ORDER BY P.Id";

			try
			{
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							using (MemoryStream memoryStream = new MemoryStream())
							{
								using (StreamWriter writer = new StreamWriter(memoryStream))
								{
									writer.WriteLine("Id;Produto;Nome;Preço;Quantidade;Preço Total;Unidade;Categoria;Dia;Hora");

									while (reader.Read())
									{
										writer.WriteLine(reader["Id"] + ";"
													   + reader["CodeProduct"] + ";"
													   + reader["Name"] + ";"
													   + reader["Price"] + ";"
													   + reader["Quantity"] + ";"
													   + reader["TotalPrice"] + ";"
													   + reader["UnitName"] + ";"
													   + reader["CategoryName"] + ";"
													   + reader["UpdateDate"] + ";"
													   + reader["UpdateTime"]);
									}
								}

								byte[] fileBytes = memoryStream.ToArray();
								string contentType = "text/plain";
								string fileName = $"Estoque-{DateTime.Now:dd-MM}.txt";
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