using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Budgets")]
[PrimaryKey(nameof(Id), nameof(Year))]
public class Budget : YearEntity
{ 
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal HealthCareContribution { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal FourO1KContribution { get; set; }

     [JsonIgnore]
    public required User User { get; set; }
    public List<BudgetItem>? BudgetItems { get; set; } = new();
    public List<BudgetGoal>? BudgetGoals { get; set; } = new();

    [NotMapped]
    public decimal TaxableIncome => (User == null ? 0m : User.GrossAnnualIncome) - FourO1KContribution - HealthCareContribution;

    [NotMapped]
    public decimal NetAnnualIncome =>
        TaxableIncome - FourO1KContribution - HealthCareContribution - FederalTax - StateTax - MedicadeTax - FICATax;

    [NotMapped]
    public decimal FederalTax => TaxableIncome * 0.0145m;

    [NotMapped]
    public decimal StateTax => 0m;

    [NotMapped]
    public decimal MedicadeTax => User.GrossAnnualIncome * 0.0145m;

    [NotMapped]
    public decimal FICATax => User.GrossAnnualIncome * 0.0620m;

    [NotMapped]
    public decimal MonthlySurvivalBudgetTotal => BudgetItems?.Sum(x => x.Amount) ?? 0.0m;

    [NotMapped]
    public decimal SurvivalFullYear => MonthlySurvivalBudgetTotal * 12;

    [NotMapped]
    public decimal AllBudgetGoalCost => BudgetGoals?.Sum(x => x.Amount) + SurvivalFullYear ?? 0.0m;

    [NotMapped]
    public decimal Remainder => NetAnnualIncome - AllBudgetGoalCost;

    [Required]
    public DateTime LastModified { get; set; }
}
