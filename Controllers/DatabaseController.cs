using Microsoft.AspNetCore.Mvc;
using Pima.Api.Models;
using Pima.Api.Repositories;

namespace Pima.Api.Controllers;

/// <summary>
/// Provides database health checks and employee data queries.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DatabaseController : ControllerBase
{
    private readonly IDatabaseHealthRepository _healthRepo;
    private readonly IEmployeeRepository _employeeRepo;

    public DatabaseController(IDatabaseHealthRepository healthRepo, IEmployeeRepository employeeRepo)
    {
        _healthRepo = healthRepo;
        _employeeRepo = employeeRepo;
    }

    /// <summary>
    /// Checks whether the database is reachable.
    /// </summary>
    /// <returns>A health-check result with status, database name, and timestamp.</returns>
    [HttpGet("health")]
    [ProducesResponseType(typeof(DatabaseHealthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DatabaseHealthResponse), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> HealthCheck()
    {
        var (isHealthy, error) = await _healthRepo.CanConnectAsync();

        var response = new DatabaseHealthResponse
        {
            Status = isHealthy ? "Healthy" : "Unhealthy",
            Database = _healthRepo.GetDatabaseName(),
            CheckedAt = DateTime.UtcNow,
            Error = error
        };

        return isHealthy
            ? Ok(response)
            : StatusCode(StatusCodes.Status503ServiceUnavailable, response);
    }

    /// <summary>
    /// Returns the top 2 employees from the Employees table.
    /// </summary>
    [HttpGet("employees")]
    [ProducesResponseType(typeof(List<Employee>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmployees()
    {
        var employees = await _employeeRepo.GetTopAsync(2);
        return Ok(employees);
    }

    /// <summary>
    /// Returns all employees.
    /// </summary>
    [HttpGet("employees/all")]
    [ProducesResponseType(typeof(List<Employee>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllEmployees()
    {
        var employees = await _employeeRepo.GetAllAsync();
        return Ok(employees);
    }

    /// <summary>
    /// Returns employees in a given department via stored procedure.
    /// </summary>
    [HttpGet("employees/department/{department}")]
    [ProducesResponseType(typeof(List<Employee>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmployeesByDepartment(string department)
    {
        var employees = await _employeeRepo.GetByDepartmentAsync(department);
        return Ok(employees);
    }

    /// <summary>
    /// Creates a new employee record.
    /// </summary>
    [HttpPost("employees")]
    [ProducesResponseType(typeof(Employee), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
    {
        employee.CreatedAt = DateTime.UtcNow;
        var created = await _employeeRepo.AddAsync(employee);
        return CreatedAtAction(nameof(GetEmployees), created);
    }

    /// <summary>
    /// Updates the last name of an employee.
    /// </summary>
    [HttpPatch("employees/{id}/lastname")]
    [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateEmployeeName(int id, [FromBody] string lastName)
    {
        var employee = await _employeeRepo.UpdateLastNameAsync(id, lastName);
        if (employee == null)
            return NotFound();

        return Ok(employee);
    }

    /// <summary>
    /// Deletes an employee by ID.
    /// </summary>
    [HttpDelete("employees/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var deleted = await _employeeRepo.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}

/// <summary>
/// Response model for the database health-check endpoint.
/// </summary>
public class DatabaseHealthResponse
{
    public string Status { get; set; } = default!;
    public string Database { get; set; } = default!;
    public DateTime CheckedAt { get; set; }
    public string? Error { get; set; }
}
