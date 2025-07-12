using Intranet.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;

public class AccountController : Controller
{
    private readonly AuthService _authService;
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    public AccountController(AuthService authService, AppDbContext context, IConfiguration configuration)
    {
        _authService = authService;
        _context = context;
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string mailUser, string passwordUser)
    {
        var user = _authService.ValidateUser(mailUser, passwordUser);

        if (user != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.nameUser),
                new Claim(ClaimTypes.Email, user.mailUser),
                new Claim(ClaimTypes.NameIdentifier, user.idUser.ToString()),
                new Claim(ClaimTypes.Role, user.Rol?.nameRol ?? "NoRol")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Invalid email or password";
        return View();
    }

    [HttpGet]
    public IActionResult LoginReset() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult LoginReset(string mailUser)
    {
        var userReset = _context.admUsers.FirstOrDefault(u => u.mailUser == mailUser);      
        if(userReset == null)
        {
            ViewBag.Message = "Email not register.";
            return View();
        }

        string token = Guid.NewGuid().ToString();
        userReset.PasswordResetToken = token;
        userReset.TokenExpiration = DateTime.Now.AddHours(1);
        _context.SaveChanges();

        string resetLink = Url.Action("LoginURL", "Account", new { token = token }, protocol: Request.Scheme);

        try
        {
            var smtpUser = _configuration["SMTP:Email"];
            var smtpPass = _configuration["SMTP:Password"];
            var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            var message = new MailMessage(smtpUser, userReset.mailUser)
            {
                Subject = "Password Reset",
                Body = $"URL Link to reset Password:\n{resetLink}",
                IsBodyHtml = false
            };

            smtp.Send(message);

            ViewBag.Message = "A link has been sent to your email to reset your password.";
        }
        catch (Exception ex)
        {
            ViewBag.Message = "Error to send message: " + ex.Message;
        }

        return View();
    }

    public IActionResult LoginURL(string token)
    {
        var emailUserToken = _context.admUsers.FirstOrDefault(u => u.PasswordResetToken == token && u.TokenExpiration > DateTime.Now);
        if (emailUserToken == null)
        {
            return NotFound("Token invalid or expired.");
        }

        ViewBag.Token = token;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult LoginURL(string token, string newPassword)
    {
        var userToken = _context.admUsers.FirstOrDefault(u => u.PasswordResetToken == token && u.TokenExpiration > DateTime.Now);
        if (userToken == null)
        {
            return NotFound("Token invalid or expired.");
        }

        userToken.passwordUser = BCrypt.Net.BCrypt.HashPassword(newPassword);
        userToken.PasswordResetToken = null;
        userToken.TokenExpiration = null;
        _context.SaveChanges();

        ViewBag.Message = "Your password has been restablished.";
        return View("ConfirmedReset");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    public IActionResult AccessDenied() => View("AccessDenied");
}
