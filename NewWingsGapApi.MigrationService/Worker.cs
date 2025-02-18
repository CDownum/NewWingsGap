using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

using OpenTelemetry.Trace;

namespace MigrationService;

public class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    public const string ActivitySourceName = "Migrations";
    private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<NewWingsGapContext>();

            await EnsureDatabaseAsync(dbContext, cancellationToken);
            await RunMigrationAsync(dbContext, cancellationToken);
            //await SeedDataAsync(dbContext, cancellationToken);
        }
        catch (Exception ex)
        {
            activity?.RecordException(ex);
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }

    private static async Task EnsureDatabaseAsync(NewWingsGapContext dbContext, CancellationToken cancellationToken)
    {
        var dbCreator = dbContext.GetService<IRelationalDatabaseCreator>();

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Create the database if it does not exist.
            // Do this first so there is then a database to start a transaction against.
            if (!await dbCreator.ExistsAsync(cancellationToken))
            {
                await dbCreator.CreateAsync(cancellationToken);
            }
        });
    }

    private static async Task RunMigrationAsync(NewWingsGapContext dbContext, CancellationToken cancellationToken)
    {
        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Run migration in a transaction to avoid partial migration if it fails.
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            await dbContext.Database.MigrateAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }

    private static async Task SeedDataAsync(NewWingsGapContext dbContext, CancellationToken cancellationToken)
    {
        User user = new()
        {
            Role = NewWingsGap.Data.Enums.Role.Admin,
            StartDate = DateTime.Now,
            FirstName = "Charles",
            MiddleName = "Michael",
            LastName = "Downum",
            GrossAnnualIncome = 100000,
            ReportingManager = "Steve Rigby",
            Salaried = true, 
            Email = "charles.downum@example.com"
        };
        Budget budget = new()
        {
            User = user,
            Year = DateTime.Now.Year,
            HealthCareContribution = 1000,
            FourO1KContribution = 1000
        };        
        var budgetItems = new List<BudgetItem>
        {
            new() {
                Amount = 2000,
                Description = "Mortgate Payment",
                LastModified = DateTime.Now,
                Budget = budget
            },
            new() {
                Amount = 800,
                Description = "Utilities",
                LastModified = DateTime.Now,
                Budget = budget
            },
            new() {
                Amount = 150,
                Description = "Home Repair / Maintainence",
                LastModified = DateTime.Now,
                Budget = budget
            },
            new() {
                Amount = 750,
                Description = "Auto Loan(s)",
                LastModified = DateTime.Now,
                Budget = budget
            },
            new() {
                Amount = 100,
                Description = "Auto Repair / Maintainence",
                LastModified = DateTime.Now,
                Budget = budget
            },
            new() {
                Amount = 100,
                Description = "Auto Insurance",
                LastModified = DateTime.Now,
                Budget = budget
            },
            new() {
                Amount = 300,
                Description = "Fuel / Transporation",
                LastModified = DateTime.Now,
                Budget = budget
            },
            new() {
                Amount = 200,
                Description = "Communication",
                LastModified = DateTime.Now,
                Budget = budget
            }            ,
            new() {
                Amount = 1000,
                Description = "Grocery",
                LastModified = DateTime.Now,
                Budget = budget
            },
            new() {
                Amount = 900,
                Description = "Dining / Entertainment",
                LastModified = DateTime.Now,
                Budget = budget
            },
            new() {
                Amount = 200,
                Description = "School",
                LastModified = DateTime.Now,
                Budget = budget
            },
            new() {
                Amount = 400,
                Description = "Clothing",
                LastModified = DateTime.Now,
                Budget = budget
            },
            new() {
                Amount = 50,
                Description = "Perscriptions / Medical Co-pay",
                LastModified = DateTime.Now,
                Budget = budget
            },
            new() {
                Amount = 200,
                Description = "Credit Cards",
                LastModified = DateTime.Now,
                Budget = budget
            },
            new() {
                Amount = 100,
                Description = "Memberships",
                LastModified = DateTime.Now,
                Budget = budget
            },
            new() {
                Amount = 50,
                Description = "Gifts",
                LastModified = DateTime.Now,
                Budget = budget
            },
            new() {
                Amount = 200,
                Description = "Charity",
                LastModified = DateTime.Now,
                Budget = budget
            }
        };
        var budgetGoals = new List<BudgetGoal>()
        {
            new() {
                Amount = 12000,
                Description = "College For Kids",
                LastModified = DateTime.Now,
                Budget = budget
            },
            new() {
                Amount = 7500,
                Description = "Vacation",
                LastModified = DateTime.Now,
                Budget = budget
            },
            new() {
                Amount = 17500,
                Description = "Remodeling",
                LastModified = DateTime.Now,
                Budget = budget
            },
            new() {
                Amount = 6000,
                Description = "Additional Savings",
                LastModified = DateTime.Now,
                Budget = budget
            },
        };
        var salesGoals = new List<SalesGoal>() {
            new() {
                Year = DateTime.Now.Year,
                AverageSalesPrice = 250000,
                CommissionRate = 1.75m,
                AverageCommision = 43750m,
                AverageLossRatio = 11.54m,
                NetSalesClosed = 88.46m,
                NetSalesNeeded = 45.714m,
                GrossSalesNeeded = 52,
                LastModified = DateTime.Now,
                User = user
            }
        };
        var salesGoalQuarters = new List<SalesGoalQuarter>()
        {
            new() {
                Quarter = 1,
                GrossSalesNeeded = 16,
                Referral = 4,
                SelfOriginating = 3,
                Internet = 1,
                Realtor = 4,
                WalkIn = 2,
                FollowUp = 2,
                LastModified = DateTime.Now,
                SalesGoalId = salesGoals.First().Id,
                SalesGoal = salesGoals.First()
            },
            new() {
                Quarter = 2,
                GrossSalesNeeded = 14,
                Referral = 4,
                SelfOriginating = 3,
                Internet = 1,
                Realtor = 3,
                WalkIn = 1,
                FollowUp = 2,
                LastModified = DateTime.Now,
                SalesGoalId = salesGoals.First().Id,
                SalesGoal = salesGoals.First()
            },
            new() {
                Quarter = 3,
                GrossSalesNeeded = 12,
                Referral = 4,
                SelfOriginating = 3,
                Internet = 1,
                Realtor = 2,
                WalkIn = 1,
                FollowUp = 1,
                LastModified = DateTime.Now,
                SalesGoalId = salesGoals.First().Id,
                SalesGoal = salesGoals.First()
            },
            new() {
                Quarter = 4,
                GrossSalesNeeded = 10,
                Referral = 4,
                SelfOriginating = 3,
                Internet = 1,
                Realtor = 1,
                WalkIn = 0,
                FollowUp = 1,
                LastModified = DateTime.Now, 
                SalesGoalId = salesGoals.First().Id,
                SalesGoal = salesGoals.First()
            }
        };

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Seed the database
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            await dbContext.Users.AddAsync(user, cancellationToken);
            await dbContext.Budgets!.AddAsync(budget, cancellationToken);

            foreach (var budgetItem in budgetItems)
            {
                await dbContext.BudgetItems!.AddAsync(budgetItem, cancellationToken);
            }

            foreach (var budgetGoal in budgetGoals)
            {
                await dbContext.BudgetGoals!.AddAsync(budgetGoal, cancellationToken);
            }

            foreach (var salesGoal in salesGoals)
            {
                await dbContext.SalesGoals!.AddAsync(salesGoal, cancellationToken);
            }

            foreach (var salesGoalQuarter in salesGoalQuarters)
            {
                await dbContext.SalesGoalQuarters!.AddAsync(salesGoalQuarter, cancellationToken);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }
}