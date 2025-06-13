using Intranet.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Intranet.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministrationController : Controller
    {
        private readonly AppDbContext _context;

        public AdministrationController(AppDbContext context)
        {
            _context = context;
        }

        private admUser? GetCurrentUser()
        {
            var idUser = User.FindFirst(ClaimTypes.NameIdentifier);

            if (idUser == null) return null;

            int idUserPortal = int.Parse(idUser.Value);

            return _context.admUsers.Include(u => u.Rol).FirstOrDefault(u => u.idUser == idUserPortal);
        }

        [HttpGet]
        public IActionResult AdminUserRegister()
        {
            ViewBag.Roles = _context.admRoles.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult AdminUserRegister(admUser model, int selectidRol)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _context.admRoles.ToList();
                return View(model);
            }

            bool userExists = _context.admUsers.Any(u => u.nameUser == model.nameUser || u.mailUser == model.mailUser);

            if (userExists)
            {
                ModelState.AddModelError("", "The username or email is already registered.");
                ViewBag.Roles = _context.admRoles.ToList();
                return View(model);
            }

            model.passwordUser = BCrypt.Net.BCrypt.HashPassword(model.passwordUser);

            model.idRol = selectidRol;

            _context.admUsers.Add(model);
            _context.SaveChanges();

            TempData["Success"] = "User successfully registered.";
            return RedirectToAction("AdminUsers", "Home");
        }

        [HttpGet]
        public IActionResult AdminUsers()
        {
            var todos = _context.admUsers
                .Include(u => u.Rol)
                .ToList();

            return View(todos);
        }

        public IActionResult AdminProfile()
        {
            var dbUser = GetCurrentUser();

            if (dbUser == null) return Unauthorized();

            return View(dbUser);
        }
    }
}
