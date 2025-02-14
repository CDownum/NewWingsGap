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
            GrossAnnualIncome = 100000
        };

        Budget budget = new()
        {
            User = user,
            Year = DateTime.Now.Year,
            HealthCareContribution = 1000,
            FourO1KContribution = 1000
        };

        BudgetItem budgetItem = new()
        {
            Amount = 1000,
            Description = "Emergency Fund",
            LastModified = DateTime.Now,
            Budget = budget
        };

        BudgetGoal budgetGoal = new()
        {
            Amount = 1000,
            Description = "Emergency Fund",
            LastModified = DateTime.Now,
            Budget = budget
        };

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            // Seed the database
            await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            await dbContext.Users.AddAsync(user, cancellationToken);
            await dbContext.Budgets!.AddAsync(budget, cancellationToken);
            await dbContext.BudgetItems!.AddAsync(budgetItem, cancellationToken);
            await dbContext.BudgetGoals!.AddAsync(budgetGoal, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }
}