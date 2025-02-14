using NewWingsGap.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Users")]
public class User
{
    public int Id { get; set; }

    [Required]
    public Role Role { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    [Required]
    public required string FirstName { get; set; }

    public string? MiddleName { get; set; }

    [Required]
    public required string LastName { get; set; }

    [NotMapped]
    public string DisplayName => $"{FirstName} {LastName}";
        
    public decimal GrossAnnualIncome { get; set; }

   public List<Budget>? Budgets { get; set; }
}
