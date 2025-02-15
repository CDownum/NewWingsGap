using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewWingsGap.Data.Migrations
{
    /// <inheritdoc />
    public partial class SchemaName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Users",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Budgets",
                newName: "Budgets",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "BudgetItems",
                newName: "BudgetItems",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "BudgetGoals",
                newName: "BudgetGoals",
                newSchema: "dbo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Users",
                schema: "dbo",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Budgets",
                schema: "dbo",
                newName: "Budgets");

            migrationBuilder.RenameTable(
                name: "BudgetItems",
                schema: "dbo",
                newName: "BudgetItems");

            migrationBuilder.RenameTable(
                name: "BudgetGoals",
                schema: "dbo",
                newName: "BudgetGoals");
        }
    }
}
