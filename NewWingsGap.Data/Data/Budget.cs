using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Budgets")]
public class Budget
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required User User { get; set; }

    [Required]
    public int Year { get; set; }

    [Required]
    public decimal HealthCareContribution { get; set; }

    [Required]
    public decimal FourO1KContribution { get; set; }

    public List<BudgetItem>? BudgetItems { get; set; }
    public List<BudgetGoal>? BudgetGoals { get; set; }

    [NotMapped]
    public decimal TaxableIncome => User.GrossAnnualIncome - FourO1KContribution - HealthCareContribution;

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
}
