using System.Diagnostics;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Sln.Estoque.Web.Auth;
using Sln.Estoque.Web.Models;

namespace Sln.Estoque.Web.Controllers;

public class HomeController : Controller
{
    [Authorize]
    public IActionResult Index()
    {
		return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
