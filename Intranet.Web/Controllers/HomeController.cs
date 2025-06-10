using Intranet.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Intranet.Web.Controllers;

[Authorize(Roles = "Administrador")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // Ejemplo de cómo obtener datos del usuario autenticado desde Claims
        var userName = User.Identity?.Name; // Este es el nameUser
        var email = User.FindFirst("Email")?.Value;
        var userId = User.FindFirst("UserId")?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        // Puedes pasar estos datos a la vista si lo deseas
        ViewBag.UserName = userName;
        ViewBag.Email = email;
        ViewBag.UserId = userId;
        ViewBag.Role = role;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
