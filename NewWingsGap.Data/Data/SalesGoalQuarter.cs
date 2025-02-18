using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("SalesGoalQuarters")]
[PrimaryKey(nameof(Id), nameof(Quarter))]
public class SalesGoalQuarter : Entity
{
    [Key]
    [Required]
    public int Quarter { get; set; }

    public int SalesGoalId { get; set; } // Required foreign key property

    [JsonIgnore]
    public required SalesGoal SalesGoal { get; set; }

    public int GrossSalesNeeded { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PercentQuarterSales => GrossSalesNeeded % SalesGoal.GrossSalesNeeded;

    public int Referral { get; set; }
    public int SelfOriginating { get; set; }
    public int Internet { get; set; }
    public int Realtor { get; set; }
    public int WalkIn { get; set; }
    public int FollowUp { get; set; }

    [Required]
    public DateTime LastModified { get; set; }
}
