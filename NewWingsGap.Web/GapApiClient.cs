using Newtonsoft.Json;

namespace NewWingsGap.Web;

public class GapApiClient(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<User[]> GetUsersAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<User>? users = null;

        var response = await _httpClient.GetStringAsync("/api/users", cancellationToken);
        var userList = JsonConvert.DeserializeObject<List<User>>(response);

        if (userList != null)
        {
            users = userList.Take(maxItems).ToList();
        }

        return users?.ToArray() ?? Array.Empty<User>();
    }

    public async Task<User?> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetStringAsync($"/api/users/{userId}", cancellationToken);
        var user = JsonConvert.DeserializeObject<User>(response);
        return user;
    }

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
    public decimal FederalTax;
    public decimal StateTax;
    public decimal MedicadeTax;
    public decimal FICATax;
    public decimal MonthlySurvivalBudgetTotal;
    public decimal SurvivalFullYear;
    public decimal AllBudgetGoalCost;
    public decimal Remainder;
}

public record User(int id, Role role, DateTime startDate, DateTime endDate,
    string firstName, string middleName, string lastName, decimal grossAnnualIncome)
{
    public int Id { get; set; } = id;
    public Role Role { get; set; } = role;
    public DateTime StartDate { get; set; } = startDate;
    public DateTime EndDate { get; set; } = endDate;
    public string FirstName { get; set; } = firstName;
    public string MiddleName { get; set; } = middleName;
    public string LastName { get; set; } = lastName;
    public decimal GrossAnnualIncome { get; set; } = grossAnnualIncome;
}

public enum Role
{
    Admin,
    Sales,
    Ceo,
    Developer,
    VicePresident
}

public class BudgetItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Amount { get; set; }
}

public class BudgetGoal
{
    public int Id { get; set; }
    public string Description { get; set; }
    public decimal TargetAmount { get; set; }
}
