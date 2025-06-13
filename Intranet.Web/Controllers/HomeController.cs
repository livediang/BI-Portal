using Intranet.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace Intranet.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    private admUser? GetCurrentUser()
    {
        var idUser = User.FindFirst(ClaimTypes.NameIdentifier);

        if (idUser == null) return null;

        int idUserPortal = int.Parse(idUser.Value);

        return _context.admUsers.Include(u => u.Rol).FirstOrDefault(u => u.idUser == idUserPortal);
    }

    public IActionResult Index()
    {
        var dbUser = GetCurrentUser();

        if (dbUser == null) return Unauthorized();

        return View(dbUser);
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
