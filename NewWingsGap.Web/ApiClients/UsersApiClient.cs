using Newtonsoft.Json;

namespace NewWingsGap.Web;

public class UsersApiClient(HttpClient httpClient)
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


