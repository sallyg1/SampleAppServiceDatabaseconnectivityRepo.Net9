using Pima.Api.Models;

namespace Pima.Api.Repositories;

/// <summary>
/// Repository interface for querying employees.
/// </summary>
public interface IEmployeeRepository
{
    /// <summary>
    /// Returns the first <paramref name="count"/> employees from the table.
    /// </summary>
    Task<List<Employee>> GetTopAsync(int count);
}
