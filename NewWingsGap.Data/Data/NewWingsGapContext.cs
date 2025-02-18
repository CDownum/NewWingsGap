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
    public DbSet<SalesGoal>? SalesGoals { get; set; }
    public DbSet<SalesGoalQuarter>? SalesGoalQuarters { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("dbo");
        modelBuilder.Entity<BudgetGoal>().Property(x => x.Amount).HasPrecision(18, 2);
        modelBuilder.Entity<BudgetItem>().Property(x => x.Amount).HasPrecision(18, 2);

        modelBuilder.Entity<User>().Property(x => x.GrossAnnualIncome).HasPrecision(18, 2);

        modelBuilder.Entity<User>().ToTable("Users", "dbo");
        modelBuilder.Entity<Budget>().ToTable("Budgets", "dbo");
        modelBuilder.Entity<BudgetItem>().ToTable("BudgetItems", "dbo");
        modelBuilder.Entity<BudgetGoal>().ToTable("BudgetGoals", "dbo");
        modelBuilder.Entity<SalesGoal>().ToTable("SalesGoals", "dbo");
        modelBuilder.Entity<SalesGoalQuarter>().ToTable("SalesGoalQuarters", "dbo");

        base.OnModelCreating(modelBuilder);
    }
}
