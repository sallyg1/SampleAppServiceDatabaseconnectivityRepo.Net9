using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Pima.Api.Data;
using Pima.Api.Models;

namespace Pima.Api.Repositories;

/// <summary>
/// EF Core implementation of <see cref="IEmployeeRepository"/>.
/// </summary>
public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;

    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<List<Employee>> GetTopAsync(int count)
    {
        return await _context.Employees
            .Take(count)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<List<Employee>> GetByDepartmentAsync(string department)
    {
        var departmentParam = new SqlParameter("@Department", department);

        var managerParam = new SqlParameter("@DepartmentManager", SqlDbType.NVarChar, 200)
        {
            Direction = ParameterDirection.Output
        };

        var salaryParam = new SqlParameter("@AverageSalary", SqlDbType.Float)
        {
            Direction = ParameterDirection.Output
        };

        var employees = await _context.Employees
            .FromSqlRaw(
                "EXEC GetEmployeesByDepartment @Department, @DepartmentManager OUTPUT, @AverageSalary OUTPUT",
                departmentParam, managerParam, salaryParam)
            .ToListAsync();

        return employees;
    }

    /// <inheritdoc />
    public async Task<Employee> AddAsync(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return employee;
    }
}
