using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("BudgetItems")]
public class BudgetItem : Entity
{
    [Required]
    public required string Description { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public DateTime LastModified { get; set; }

    [Required]
    [JsonIgnore]
    public required Budget Budget { get; set; }

    //public BudgetItem(string description, decimal amount, DateTime lastModified, Budget budget)
    //{
    //    Description = description;
    //    Amount = amount;
    //    LastModified = lastModified;
    //    Budget = budget;
    //}
}
