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

    /// <summary>
    /// Returns employees in the given department via stored procedure.
    /// </summary>
    Task<List<Employee>> GetByDepartmentAsync(string department);

    /// <summary>
    /// Adds a new employee and returns the created entity.
    /// </summary>
    Task<Employee> AddAsync(Employee employee);
}
