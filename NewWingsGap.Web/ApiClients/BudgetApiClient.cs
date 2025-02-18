using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NewWingsGap.Web.ApiClients;

public class BudgetApiClient(HttpClient httpClient) : UsersApiClient(httpClient)
{
    private readonly HttpClient _httpClient = httpClient;
    
    public async Task<Budget[]> GetBudgetsByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetStringAsync($"/api/budgets/users/{userId}", cancellationToken);
        var budgets = JsonConvert.DeserializeObject<List<Budget>>(response);
        return budgets?.ToArray() ?? Array.Empty<Budget>();
    }

    public async Task DeleteBudgetAsync(int budgetId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync($"/api/budgets/{budgetId}", cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}

public class Budget
{
    public int Id { get; set; }
    public int Year { get; set; }
    public decimal HealthCareContribution { get; set; }
    public decimal FourO1KContribution { get; set; }
    public List<BudgetItem>? BudgetItems { get; set; }
    public List<BudgetGoal>? BudgetGoals { get; set; }
    public decimal TaxableIncome;
    public decimal NetAnnualIncome;
    public string FederalTax;
    public decimal StateTax;
    public decimal MedicadeTax;
    public decimal FICATax;
    public decimal MonthlySurvivalBudgetTotal;
    public decimal SurvivalFullYear;
    public decimal Remainder;
}

public class BudgetItem
{
    public int Id { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
}

public class BudgetGoal
{
    public int Id { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
}


