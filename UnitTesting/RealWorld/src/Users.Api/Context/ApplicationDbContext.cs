using Microsoft.EntityFrameworkCore;
using Users.Api.Models;

namespace Users.Api.Context;

public sealed class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}