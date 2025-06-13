using Intranet.Web.Models;
using Microsoft.EntityFrameworkCore;

public class AuthService
{
    private readonly AppDbContext _context;

    public AuthService(AppDbContext context)
    {
        _context = context;
    }

    public admUser? ValidateUser(string mailUser, string passwordUser)
    {
        var user = _context.admUsers
            .Include(u => u.Rol)
            .FirstOrDefault(u => u.mailUser == mailUser);

        if (user != null && BCrypt.Net.BCrypt.Verify(passwordUser, user.passwordUser))
        {
            return user;
        }

        return null;
    }
}
