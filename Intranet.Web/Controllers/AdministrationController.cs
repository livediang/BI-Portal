using Intranet.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult AdminUsers(string searchTerm, int page = 1, int pageSize = 5)
        {
            var query = _context.admUsers.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(u => u.nameUser.Contains(searchTerm) || u.mailUser.Contains(searchTerm));
            }

            int totalRecords = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            var users = query
                .Include(u => u.Rol)
                .OrderBy(u => u.idUser)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new AdministrationViewModel
            {
                Users = users,
                SearchTerm = searchTerm,
                CurrentPage = page,
                TotalPages = totalPages
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_UserTablePartial", viewModel);
            }

            return View(viewModel);
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
            return RedirectToAction("AdminUsers", "Administration");
        }

        [HttpGet]
        public IActionResult AdminUserEdit(int idUser)
        {
            var idUserEdit = _context.admUsers.FirstOrDefault(u => u.idUser == idUser);
            if (idUserEdit == null)
            {
                return NotFound();
            }

            ViewBag.RolesList = new SelectList(_context.admRoles, "idRol", "nameRol", idUserEdit.idRol);
            return View(idUserEdit);
        }

        [HttpPost]
        public IActionResult AdminUserEdit(admUser model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.RolesList = new SelectList(_context.admRoles, "idRol", "nameRol", model.idRol);
                return View(model);
            }

            var idUserDB = _context.admUsers.FirstOrDefault(u => u.idUser == model.idUser);
            if (idUserDB == null)
            {
                return NotFound();
            }

            bool existsUser = _context.admUsers.Any(u => (u.nameUser == model.nameUser || u.mailUser == model.mailUser) && u.idUser != model.idUser);
            if (existsUser)
            {
                ModelState.AddModelError("", "The username or email is already registered by another user.");
                ViewBag.Roles = _context.admRoles.ToList();
                return View(model);
            }

            idUserDB.nameUser = model.nameUser;
            idUserDB.mailUser = model.mailUser;
            idUserDB.idRol = model.idRol;

            if (!string.IsNullOrEmpty(model.passwordUser))
            {
                idUserDB.passwordUser = BCrypt.Net.BCrypt.HashPassword(model.passwordUser);
            }

            _context.SaveChanges();

            TempData["Success"] = "User successfully updated.";
            return RedirectToAction("AdminUsers", "Administration");
        }

        [HttpPost]
        public IActionResult AdminUserDelete(int idUser)
        {
            var idUserDelete = _context.admUsers.FirstOrDefault(u => u.idUser == idUser);
            if (idUserDelete == null)
            {
                return NotFound();
            }

            _context.admUsers.Remove(idUserDelete);
            _context.SaveChanges();

            TempData["Success"] = "User successfully deleted.";
            return RedirectToAction("AdminUsers", "Administration");
        }

        public IActionResult AdminProfile()
        {
            var dbUser = GetCurrentUser();

            if (dbUser == null) return Unauthorized();

            return View(dbUser);
        }
    }
}
