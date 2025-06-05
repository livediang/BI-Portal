using Microsoft.EntityFrameworkCore;
using Intranet.Web.Models;
using System.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Rol> Roles => Set<Rol>();
}
