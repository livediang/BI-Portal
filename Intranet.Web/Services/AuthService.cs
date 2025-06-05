using Intranet.Web.Models;
using Microsoft.EntityFrameworkCore;

public class AuthService
{
    private readonly AppDbContext _context;

    public AuthService(AppDbContext context)
    {
        _context = context;
    }

    public User? ValidateUser(string nameUser, string passwordUser)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(passwordUser);
        var user = _context.Users
            .Include(u => u.nameRol)
            .FirstOrDefault(u => u.nameUser == nameUser);

        if (user != null && BCrypt.Net.BCrypt.Verify(passwordUser, user.passwordUser))
        {
            return user;
        }

        return null;
    }
}
