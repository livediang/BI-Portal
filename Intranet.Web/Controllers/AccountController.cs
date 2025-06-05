using Microsoft.AspNetCore.Mvc;
using Intranet.Web.Models;

public class AccountController : Controller
{
    private readonly AuthService _authService;
    public AccountController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public IActionResult Login(string nameUser, string passwordUser)
    {
        var user = _authService.ValidateUser(nameUser, passwordUser);

        if (user != null)
        {
            HttpContext.Session.SetInt32("idUser", user.idUser);
            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Usuario o contraseña inválidos";

        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
