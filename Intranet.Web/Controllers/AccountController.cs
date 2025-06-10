using Microsoft.AspNetCore.Mvc;
using Intranet.Web.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

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
    public async Task<IActionResult> Login(string nameUser, string passwordUser)
    {
        var user = _authService.ValidateUser(nameUser, passwordUser);

        if (user != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.nameUser),
                new Claim(ClaimTypes.Email, user.mailUser),
                new Claim("UserId", user.idUser.ToString()),
                new Claim(ClaimTypes.Role, user.Rol?.nameRol ?? "SinRol")
            };

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyCookieAuth", principal);
            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Usuario o contraseña inválidos";
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("MyCookieAuth");
        return RedirectToAction("Login");
    }

    public IActionResult AccessDenied()
    {
        return View("AccessDenied");
    }
}
