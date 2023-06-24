using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sln.Estoque.Domain.DTO;
using Sln.Estoque.Domain.IServices;
using Sln.Estoque.Domain.Util;
using Sln.Estoque.Web.Auth;

namespace Sln.Estoque.Web.Controllers
{
	public class UnitController : Controller
    {
        private readonly IUnitService _unitService;

        public UnitController(IUnitService unitService)
        {
            _unitService = unitService;
        }

		[Authorize(Roles = "Alta")]
        public ActionResult Index()
        {
            return View(_unitService.FindAll());
        }

		[Authorize(Roles = "Alta")]
		public IActionResult Create()
        {
			ViewBag.UnitId = new SelectList(_unitService.FindAll(), "id", "name", "Select...");
            return View();
        }

		[HttpPost]
		[Authorize(Roles = "Alta")]
		public async Task<IActionResult> Create([Bind("name")] UnitDTO unit)
		{
			if (ModelState.IsValid)
			{
				if (await _unitService.Save(unit) > 0)
				{
					return RedirectToAction(nameof(Index));
				}
			}

			return View(unit);
		}

		[Authorize(Roles = "Alta")]
		public async Task<IActionResult> Edit(int id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var unit = await _unitService.FindById(id);
			return View(unit);
		}

		[HttpPost]
		[Authorize(Roles = "Alta")]
		public async Task<IActionResult> Edit(int? id, [Bind("id, name")] UnitDTO unit)
		{
			if (!(id == unit.id))
			{
				return NotFound();
			}
			if (ModelState.IsValid)
			{
				if (await _unitService.Save(unit) > 0)
				{
					return RedirectToAction(nameof(Index));
				}
			}

			return View(unit);
		}

		[HttpPost]
		[Authorize(Roles = "Alta")]
		public async Task<IActionResult> Delete(int? id)
		{
			var returnDel = new ReturnJsonGeneric
			{
				status = "Success",
				code = "200"
			};
			if (await _unitService.Delete(id ?? 0) <= 0)
			{
				returnDel = new ReturnJsonGeneric
				{
					status = "Error",
					code = "200"
				};
			}
			return Json(returnDel);
		}
	}
}