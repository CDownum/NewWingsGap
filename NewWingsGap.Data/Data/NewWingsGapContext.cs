using Microsoft.EntityFrameworkCore;

public class NewWingsGapContext : DbContext
{
    public NewWingsGapContext(DbContextOptions<NewWingsGapContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Budget>? Budgets { get; set; }
    public DbSet<BudgetItem>? BudgetItems { get; set; }
    public DbSet<BudgetGoal>? BudgetGoals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BudgetGoal>().Property(x => x.Amount).HasPrecision(18, 2);
        modelBuilder.Entity<BudgetItem>().Property(x => x.Amount).HasPrecision(18, 2);

        modelBuilder.Entity<Budget>().Property(x => x.FourO1KContribution).HasPrecision(18, 2);
        modelBuilder.Entity<Budget>().Property(x => x.HealthCareContribution).HasPrecision(18, 2);
        modelBuilder.Entity<User>().Property(x => x.GrossAnnualIncome).HasPrecision(18, 2);

        base.OnModelCreating(modelBuilder);
    }
}
