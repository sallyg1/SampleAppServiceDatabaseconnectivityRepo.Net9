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
