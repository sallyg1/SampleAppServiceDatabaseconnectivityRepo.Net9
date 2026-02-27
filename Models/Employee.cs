using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pima.Api.Models;

/// <summary>
/// Entity mapping to the existing Employees table in Azure SQL.
/// </summary>
[Table("Employees")]
public class Employee
{
    [Key]
    public int EmployeeId { get; set; }

    [Required, MaxLength(50)]
    public string FirstName { get; set; } = default!;

    [Required, MaxLength(50)]
    public string LastName { get; set; } = default!;

    [Required, MaxLength(100)]
    public string Email { get; set; } = default!;

    [MaxLength(20)]
    public string? Phone { get; set; }

    [Required, MaxLength(50)]
    public string Department { get; set; } = default!;

    [Required, MaxLength(100)]
    public string JobTitle { get; set; } = default!;

    public DateTime HireDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Salary { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
