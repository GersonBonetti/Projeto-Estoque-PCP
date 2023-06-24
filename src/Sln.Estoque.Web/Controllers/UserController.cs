using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sln.Estoque.Application.Service.SQLServerServices;
using Sln.Estoque.Domain.DTO;
using Sln.Estoque.Domain.Entities;
using Sln.Estoque.Domain.IServices;
using Sln.Estoque.Domain.Util;
using Sln.Estoque.Web.Auth;

namespace Sln.Estoque.Web.Controllers
{
	public class UserController : Controller
	{
		private readonly IUserService _userService;
		private readonly IRoleService _roleService;
		private readonly TokenGenerator _tokenGenerator = new();

		public UserController(IUserService service, IRoleService roleService)
		{
			_userService = service;
			_roleService = roleService;
		}

		[AllowAnonymous]
		public IActionResult Index()
		{
			return View();
		}

		[Authorize(Roles = "Alta")]
		public IActionResult Create()
		{
			ViewBag.RoleId = new SelectList(_roleService.FindAll(), "id", "level", 3);
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "Alta")]
		public async Task<IActionResult> Create([Bind("id, name, username, password, roleId")] UserDTO user)
		{
			if (ModelState.IsValid)
			{
				if (await _userService.Save(user) > 0)
				{
					return Redirect("/User/Create#crsuccess");
				}
			}

			return Redirect("/Home/Index#error");
		}

		[HttpPost]
		[Authorize(Roles = "Alta")]
		public async Task<JsonResult> Delete(int id)
		{
			var returnDel = new ReturnJsonGeneric
			{
				status = "error"
			};

			if (await _userService.Delete(id) >= 0)
			{
				returnDel.status = "success";
			}

			return Json(returnDel);
		}

		[HttpPost]
		[AllowAnonymous]
		public JsonResult SignIn(string userName, string passWord)
		{
			var retSignIn = new ReturnJsonUser
			{
				status = "error",
				username = "",
				roleId = 0,
				name = "",
			};

			var users = _userService.FindAll();
			foreach (var item in users)
			{
				if (userName == item.username && passWord == item.password)
				{
					retSignIn = new ReturnJsonUser
					{
						status = "success",
						username = item.username,
						roleId = item.roleId,
						name = item.name
					};

					var options = new CookieOptions
					{
						Path = "/", 
						HttpOnly = true
					};

					var tokenJwt = _tokenGenerator.GenerateToken(item.name, item.role.Level);
					Response.Cookies.Append("userJwt", tokenJwt, options);

					return Json(retSignIn);
				}
			}
			return Json(retSignIn);
		}

		[Authorize(Roles = "Alta")]
		public async Task<IActionResult> Edit(int id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var user = await _userService.FindById(id);
			ViewBag.Role = new SelectList(_roleService.FindAll(), "id", "level", user.roleId);

			return View(user);
		}

		[HttpPost]
		[Authorize(Roles = "Alta")]
		public async Task<IActionResult> Edit(int id, [Bind("id, name, username, roleId")] UserDTO user)
		{
			if (user.password == "" || user.password == null)
			{
				UserDTO userDataBase = await _userService.FindById(user.id);
				user.password = userDataBase.password;
			}

			if (await _userService.Save(user) > 0)
			{
				return Redirect("/User/List#edsuccess");
			}
			return View(user);
		}

		[HttpPost]
		[Authorize(Roles = "Alta")]
		public async Task<JsonResult> EditPassword(string name, string password)
		{
            var updtPassword = new ReturnJsonGeneric
            {
                status = "Error",
                code = "400"
            };

            UserDTO? user = _userService.FindAll().Find(x => x.name == name);

			if (user != null)
			{
				user.password = password;
				if (await _userService.Save(user) > 0)
				{
					updtPassword.status = "Success";
					updtPassword.code = "200";
                    return Json(updtPassword);
				}
			}

            return Json(updtPassword);
		}

		[Authorize(Roles = "Alta")]
		public ActionResult List()
		{
			return View(_userService.FindAll());
		}

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
				if (await _userService.Delete(id ?? 0) <= 0)
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
	}
}