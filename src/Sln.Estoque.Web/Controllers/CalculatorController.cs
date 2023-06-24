using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Sln.Estoque.Application.Service.SQLServerServices;
using Sln.Estoque.Domain.DTO;
using Sln.Estoque.Domain.IServices;
using Sln.Estoque.Domain.Util;
using Sln.Estoque.Web.Auth;
using System.Text.RegularExpressions;
using FileInput = Sln.Estoque.Domain.Entities.FileInput;

namespace Sln.Estoque.Web.Controllers
{
	public class CalculatorController : Controller
	{
		private readonly ILayoutService _layoutService;

		public CalculatorController(ILayoutService layoutService)
		{
			_layoutService = layoutService;
		}

		[Authorize]
		public IActionResult Calculate()
		{
			return View();
		}

		[Authorize(Roles = "Alta")]
		public IActionResult Index()
		{
			return View(_layoutService.FindAll());
		}

		[Authorize(Roles = "Alta")]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "Alta")]
		public async Task<IActionResult> Create([Bind("id, viewName, fileName, multiplier, method, quantityPosition")] LayoutDTO layout)
		{
			if (ModelState.IsValid)
			{
				if (await _layoutService.Save(layout) > 0)
				{
					return RedirectToAction(nameof(Index));
				}
			}
			return View(layout);
		}

		[Authorize(Roles = "Alta")]
		public async Task<IActionResult> Edit(int id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var product = await _layoutService.FindById(id);
			return View(product);
		}

		[HttpPost]
		[Authorize(Roles = "Alta")]
		public async Task<IActionResult> Edit(int? id, [Bind("id, viewName, fileName, multiplier, method, quantityPosition")] LayoutDTO layout)
		{
			if (ModelState.IsValid)
			{
				if (await _layoutService.Save(layout) > 0)
				{
					return RedirectToAction(nameof(Index));
				}
			}
			return View(layout);
		}

		[HttpPost]
		[Authorize]
		public List<FileInput> UploadFile(string json)
		{
			var files = JsonConvert.DeserializeObject<List<FileInput>>(json);
			var layouts = _layoutService.FindAll();
			var idLayoutDict = layouts.ToDictionary(l => l.fileName, l => l.id);

			foreach (var file in files)
			{
				var containsName = idLayoutDict.Keys.FirstOrDefault(k => file.FileName.Contains(k));

				bool isCeA = false;
				if (Regex.IsMatch(file.FileName, @"PT\d{7}"))
				{
					file.Layout = new LayoutDTO { id = 999, viewName = "Tag C&A", fileName = "-/-", multiplier = 1, method = 3, quantityPosition = 0 };
					isCeA = true;
				}
				else if (Regex.IsMatch(file.FileName, @"PA\d{7}"))
				{
					file.Layout = new LayoutDTO { id = 998, viewName = "Pack C&A", fileName = "-/-", multiplier = 1, method = 3, quantityPosition = 0 };
					isCeA = true;
				}

				if (containsName != null && isCeA != true)
				{
					file.IdLayout = idLayoutDict[containsName];
					file.Layout = layouts.FirstOrDefault(l => l.id == file.IdLayout);

					file.Quantity = file.Layout.method switch
					{
						1 => MethodZPL(file.Content, file.Layout.multiplier),
						2 => MethodTxt(file.Content, file.Layout.quantityPosition),
						_ => 0,
					};
				}
				else if (isCeA)
				{
					file.Quantity = MethodCeA(file.Content);
				}
				else
				{
					file.Layout = new LayoutDTO { id = 1000, viewName = "Não Cadastrado", fileName = "-/-", multiplier = 0, method = 0, quantityPosition = 0 };
					file.IdLayout = 0;
				}
			}
			return files;
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
				if (await _layoutService.Delete(id ?? 0) <= 0)
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

		// Method 1
		[NonAction]
		public int MethodZPL(string file, int multiplier)
		{
			string[] blocks = file.Split(new[] { "^XZ" }, StringSplitOptions.RemoveEmptyEntries);
			int totalQuantity = 0;

			foreach (string block in blocks)
			{
				int positionPQ = block.IndexOf("^PQ");
				if (positionPQ != -1)
				{
					Regex rgx = new(@"\D"); //busca por um char que não seja um dígito
					Match match = rgx.Match(block, positionPQ + 3);
					if (match.Success)
					{
						string quantityString = block.Substring(positionPQ + 3, match.Index - positionPQ - 3);

						if (int.TryParse(quantityString, out int quantity))
						{
							totalQuantity += quantity;
						}
					}
				}
			}
			return totalQuantity * multiplier;
		}

		// Method 2
		[NonAction]
		public int MethodTxt(string file, int quantityPosition)
		{
			int totalQuantity = 0;
			string[] lines = Regex.Split(file, "\r\n|\r|\n");
			lines = lines.Where(l => !string.IsNullOrEmpty(l)).ToArray();
			lines = lines.Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
			string[] separators = new string[] { "|", ",", ";" };

			foreach (string line in lines)
			{
				string[] lineContent = line.Split(separators, StringSplitOptions.None);
				if (int.TryParse(lineContent[quantityPosition], out int quantity))
				{
					totalQuantity += quantity;
				}
			}
			return totalQuantity;
		}

		// Method 3
		[NonAction]
		public int MethodCeA(string file)
		{
			int quantity = 0;
			string labelType = file.Substring(34, 4);
			string[] lines = Regex.Split(file, "\r\n|\r|\n");
			lines = lines.Where(l => !string.IsNullOrEmpty(l)).ToArray();

			// chegou até no loop com sucesso, mas preciso melhorar essa lógica pois
			// ele encerrou antes mesmo de começar, por causa da condição de parada dele
			// talvez sempre excluir a primeira linha e então fazer um foreach
			for (int i = 1; i < lines.Length; i++)
			{
				if (labelType == "A101" && lines[i].Length > 250)
				{
					// é tag
					quantity += int.Parse(lines[i].Substring(238, 4));
				}
				else if (labelType == "A301")
				{
					// é pack
					quantity += int.Parse(lines[i].Substring(149, 4));
				}
			}

			return quantity;
		}
	}
}