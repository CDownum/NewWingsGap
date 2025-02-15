using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("BudgetItems")]
public class BudgetItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string Description { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public DateTime LastModified { get; set; }

    [Required]
    public required Budget Budget { get; set; }
}
