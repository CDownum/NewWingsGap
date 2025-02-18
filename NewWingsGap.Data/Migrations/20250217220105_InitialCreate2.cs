using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewWingsGap.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GrossAnnualIncome = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ReportingManager = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salaried = table.Column<bool>(type: "bit", nullable: false),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salutation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cell = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Budgets",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    HealthCareContribution = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FourO1KContribution = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => new { x.Id, x.Year });
                    table.ForeignKey(
                        name: "FK_Budgets_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesGoals",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    AverageSalesPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CommissionRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AverageCommision = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AverageLossRatio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetSalesClosed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetSalesNeeded = table.Column<decimal>(type: "decimal(18,3)", nullable: false),
                    GrossSalesNeeded = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesGoals", x => new { x.Id, x.Year });
                    table.ForeignKey(
                        name: "FK_SalesGoals_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetGoals",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BudgetId = table.Column<int>(type: "int", nullable: false),
                    BudgetYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetGoals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetGoals_Budgets_BudgetId_BudgetYear",
                        columns: x => new { x.BudgetId, x.BudgetYear },
                        principalSchema: "dbo",
                        principalTable: "Budgets",
                        principalColumns: new[] { "Id", "Year" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BudgetItems",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BudgetId = table.Column<int>(type: "int", nullable: false),
                    BudgetYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetItems_Budgets_BudgetId_BudgetYear",
                        columns: x => new { x.BudgetId, x.BudgetYear },
                        principalSchema: "dbo",
                        principalTable: "Budgets",
                        principalColumns: new[] { "Id", "Year" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesGoalQuarters",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Quarter = table.Column<int>(type: "int", nullable: false),
                    SalesGoalId = table.Column<int>(type: "int", nullable: false),
                    SalesGoalYear = table.Column<int>(type: "int", nullable: true),
                    GrossSalesNeeded = table.Column<int>(type: "int", nullable: false),
                    Referral = table.Column<int>(type: "int", nullable: false),
                    SelfOriginating = table.Column<int>(type: "int", nullable: false),
                    Internet = table.Column<int>(type: "int", nullable: false),
                    Realtor = table.Column<int>(type: "int", nullable: false),
                    WalkIn = table.Column<int>(type: "int", nullable: false),
                    FollowUp = table.Column<int>(type: "int", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesGoalQuarters", x => new { x.Id, x.Quarter });
                    table.ForeignKey(
                        name: "FK_SalesGoalQuarters_SalesGoals_SalesGoalId_SalesGoalYear",
                        columns: x => new { x.SalesGoalId, x.SalesGoalYear },
                        principalSchema: "dbo",
                        principalTable: "SalesGoals",
                        principalColumns: new[] { "Id", "Year" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetGoals_BudgetId_BudgetYear",
                schema: "dbo",
                table: "BudgetGoals",
                columns: new[] { "BudgetId", "BudgetYear" });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetItems_BudgetId_BudgetYear",
                schema: "dbo",
                table: "BudgetItems",
                columns: new[] { "BudgetId", "BudgetYear" });

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_UserId",
                schema: "dbo",
                table: "Budgets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesGoalQuarters_SalesGoalId_SalesGoalYear",
                schema: "dbo",
                table: "SalesGoalQuarters",
                columns: new[] { "SalesGoalId", "SalesGoalYear" });

            migrationBuilder.CreateIndex(
                name: "IX_SalesGoals_UserId",
                schema: "dbo",
                table: "SalesGoals",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetGoals",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BudgetItems",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SalesGoalQuarters",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Budgets",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SalesGoals",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "dbo");
        }
    }
}
