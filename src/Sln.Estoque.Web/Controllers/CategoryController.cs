using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sln.Estoque.Domain.DTO;
using Sln.Estoque.Domain.IServices;
using Sln.Estoque.Domain.Util;
using Sln.Estoque.Web.Auth;

namespace Sln.Estoque.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _service;
		
        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Alta")]
        public ActionResult Index()
        {
            return View(_service.FindAll());
        }

		[Authorize(Roles = "Alta")]
		public ActionResult Create()
        {
			ViewData["CategoryId"] = new SelectList(_service.FindAll(), "id", "name", "Select...");
            return View();
        }

        [HttpPost]
		[Authorize(Roles = "Alta")]
		public async Task<IActionResult> Create([Bind("name")] CategoryDTO category)
        {
			if (ModelState.IsValid)
            {
                if (await _service.Save(category) > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(category);
        }

		[Authorize(Roles = "Alta")]
		public async Task<IActionResult> Edit(int id)
        {
			if (id == null)
            {
                return NotFound();
            }

            var category = await _service.FindById(id);
            return View(category);
        }

        [HttpPost]
		[Authorize(Roles = "Alta")]
		public async Task<IActionResult> Edit(int? id, [Bind("id, name")] CategoryDTO category)
        {
			if (!(id == category.id))
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (await _service.Save(category) > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(category);
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

			if (await _service.Delete(id ?? 0) <= 0)
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