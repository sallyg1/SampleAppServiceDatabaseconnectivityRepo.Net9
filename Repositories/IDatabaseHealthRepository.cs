namespace Pima.Api.Repositories;

/// <summary>
/// Abstracts database health-check logic for testability.
/// </summary>
public interface IDatabaseHealthRepository
{
    /// <summary>
    /// Returns true if the database is reachable, false otherwise.
    /// </summary>
    Task<(bool Success, string? Error)> CanConnectAsync();

    /// <summary>
    /// Returns the database name from the connection string.
    /// </summary>
    string GetDatabaseName();
}
