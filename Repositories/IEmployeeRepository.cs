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
    /// Returns all employees.
    /// </summary>
    Task<List<Employee>> GetAllAsync();

    /// <summary>
    /// Returns employees in the given department via stored procedure.
    /// </summary>
    Task<List<Employee>> GetByDepartmentAsync(string department);

    /// <summary>
    /// Adds a new employee and returns the created entity.
    /// </summary>
    Task<Employee> AddAsync(Employee employee);

    /// <summary>
    /// Updates the last name of an employee by ID.
    /// </summary>
    Task<Employee?> UpdateLastNameAsync(int id, string lastName);

    /// <summary>
    /// Deletes an employee by ID. Returns true if found and deleted.
    /// </summary>
    Task<bool> DeleteAsync(int id);
}
