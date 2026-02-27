using Microsoft.EntityFrameworkCore;
using Pima.Api.Models;

namespace Pima.Api.Data;

/// <summary>
/// EF Core DbContext for Azure SQL database access.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees => Set<Employee>();
}
