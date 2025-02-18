using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace NewWingsGap.Web;

public class QuarterlySalesApiClient(HttpClient httpClient) : UsersApiClient(httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<SalesGoal[]> GetQuarterlySalesByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetStringAsync($"/api/salesGoals/users/{userId}", cancellationToken);
        var budgets = JsonConvert.DeserializeObject<List<SalesGoal>>(response);
        return budgets?.ToArray() ?? Array.Empty<SalesGoal>();
    }
}

public class SalesGoal
{
    public int Id { get; set; }
    public int Year { get; set; }   
    public decimal AverageSalesPrice { get; set; }
    public decimal CommissionRate { get; set; }
    public decimal AverageCommision { get; set; }
    public decimal AverageLossRatio { get; set; }
    public decimal NetSalesClosed { get; set; }
    public decimal NetSalesNeeded { get; set; }
    public decimal GrossSalesNeeded { get; set; }

    public List<SalesGoalQuarter> SalesGoalQuarters { get; set; } = new();

    public DateTime LastModified { get; set; }
}

public class SalesGoalQuarter
{
    public int Quarter { get; set; }
    public string QuarterDescription
    {
        get
        {
            return Quarter switch
            {
                1 => "1st",
                2 => "2nd",
                3 => "3rd",
                4 => "4th",
                _ => throw new NotImplementedException()
            };
        }
    }

    public int GrossSalesNeeded { get; set; }

    public decimal TotalPercent
    {
        get
        {
            return RealtorPercentQuarterSales + FollowUpPercentQuarterSales + InternetPercentQuarterSales 
                + ReferralPercentQuarterSales + SelfOriginatingPercentQuarterSales + WalkInPercentQuarterSales;
        }
    }
    public decimal Total
    {
        get
        {
            return Realtor + FollowUp + Internet
                + Referral + SelfOriginating + WalkIn;
        }
    }
    public decimal ReferralPercentQuarterSales
    {
        get
        {
            if (GrossSalesNeeded == 0)
                return 0;
            return (decimal)Referral / GrossSalesNeeded;
        }
    }

    public decimal SelfOriginatingPercentQuarterSales
    {
        get
        {
            if (GrossSalesNeeded == 0)
                return 0;
            return (decimal)SelfOriginating / GrossSalesNeeded;
        }
    }

    public decimal InternetPercentQuarterSales
    {
        get
        {
            if (GrossSalesNeeded == 0)
                return 0;
            return (decimal)Internet / GrossSalesNeeded;
        }
    }

    public decimal RealtorPercentQuarterSales
    {
        get
        {
            if (GrossSalesNeeded == 0)
                return 0;
            return (decimal)Realtor / GrossSalesNeeded;
        }
    }

    public decimal WalkInPercentQuarterSales
    {
        get
        {
            if (GrossSalesNeeded == 0)
                return 0;
            return (decimal)WalkIn / GrossSalesNeeded;
        }
    }

    public decimal FollowUpPercentQuarterSales
    {
        get
        {
            if (GrossSalesNeeded == 0)
                return 0;
            return (decimal)FollowUp / GrossSalesNeeded;
        }
    }


    public int Referral { get; set; }
    public int SelfOriginating { get; set; }
    public int Internet { get; set; }
    public int Realtor { get; set; }
    public int WalkIn { get; set; }
    public int FollowUp { get; set; }
    public DateTime LastModified { get; set; }
}


