using Microsoft.EntityFrameworkCore;
using Intranet.Web.Models;
using System.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<admUser> admUsers { get; set; }
    public DbSet<admRol> admRoles { get; set; }
}
