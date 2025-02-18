using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("SalesGoals")]
[PrimaryKey(nameof(Id), nameof(Year))]
public class SalesGoal : YearEntity
{
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal AverageSalesPrice { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal CommissionRate { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal AverageCommision { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal AverageLossRatio { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal NetSalesClosed { get; set; }

    [Column(TypeName = "decimal(18,3)")]
    public decimal NetSalesNeeded { get; set; }

    [Column(TypeName = "decimal(18,0)")]
    public decimal GrossSalesNeeded { get; set; }
    
    public List<SalesGoalQuarter> SalesGoalQuarters { get; set; } = new();

    [Required]
    public DateTime LastModified { get; set; }

    [JsonIgnore]
    public required User User { get; set; }
}
