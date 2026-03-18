# Pima API

A .NET 9 Web API that connects to Azure SQL via Azure Key Vault for secret management.

## Prerequisites

- .NET 9 SDK
- Azure SQL Database
- Azure Key Vault with a secret named `SqlDatabase` containing the SQL connection string

## Stored Procedures

### GetEmployeesByDepartment

Returns all employees in a given department. Used by the `GET /api/database/employees/department/{department}` endpoint.

```sql
CREATE OR ALTER PROCEDURE GetEmployeesByDepartment
    @Department NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT EmployeeId, FirstName, LastName, Email, Phone,
           Department, JobTitle, HireDate, Salary, IsActive,
           CreatedAt, UpdatedAt
    FROM Employees
    WHERE Department = @Department;
END
```

## API Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/api/database/health` | Database health check |
| GET | `/api/database/employees` | Returns top 2 employees |
| GET | `/api/database/employees/department/{department}` | Returns employees by department (stored procedure) |
| GET | `/api/secrets/{secretName}` | Retrieves a secret from Azure Key Vault |


