using Microsoft.EntityFrameworkCore;
using Pima.Api.Data;

namespace Pima.Api.Repositories;

/// <summary>
/// Checks database connectivity via EF Core.
/// </summary>
public class DatabaseHealthRepository : IDatabaseHealthRepository
{
    private readonly AppDbContext _context;

    public DatabaseHealthRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<(bool Success, string? Error)> CanConnectAsync()
    {
        try
        {
            var result = await _context.Database.CanConnectAsync();
            return (result, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    /// <inheritdoc />
    public string GetDatabaseName()
    {
        return _context.Database.GetDbConnection().Database;
    }
}
