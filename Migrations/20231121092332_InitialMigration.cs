using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoaringAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    AddressLine = table.Column<string>(type: "TEXT", nullable: true),
                    CoAddress = table.Column<string>(type: "TEXT", nullable: true),
                    Commune = table.Column<string>(type: "TEXT", nullable: true),
                    CommuneCode = table.Column<int>(type: "INTEGER", nullable: true),
                    County = table.Column<string>(type: "TEXT", nullable: true),
                    ZipCode = table.Column<int>(type: "INTEGER", nullable: true),
                    Town = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressId);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyRatingId = table.Column<int>(type: "INTEGER", nullable: true),
                    RoaringCompanyId = table.Column<string>(type: "TEXT", nullable: true),
                    CompanyName = table.Column<string>(type: "TEXT", nullable: true),
                    CompanyRegistrationDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IndustryCode = table.Column<int>(type: "INTEGER", nullable: true),
                    IndustryText = table.Column<string>(type: "TEXT", nullable: true),
                    LegalGroupCode = table.Column<string>(type: "TEXT", nullable: true),
                    LegalGroupText = table.Column<string>(type: "TEXT", nullable: true),
                    EmployerContributionReg = table.Column<bool>(type: "INTEGER", nullable: true),
                    NumberCompanyUnits = table.Column<int>(type: "INTEGER", nullable: true),
                    NumberEmployeesInterval = table.Column<string>(type: "TEXT", nullable: true),
                    PreliminaryTaxReg = table.Column<bool>(type: "INTEGER", nullable: true),
                    SeveralCompanyName = table.Column<bool>(type: "INTEGER", nullable: true),
                    Currency = table.Column<string>(type: "TEXT", nullable: true),
                    NumberOfEmployees = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "CompanyEmployees",
                columns: table => new
                {
                    EmployeeInCompanyId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    TopDirectorFunction = table.Column<string>(type: "TEXT", nullable: true),
                    TopDirectorName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyEmployees", x => x.EmployeeInCompanyId);
                });

            migrationBuilder.CreateTable(
                name: "CompanyRatings",
                columns: table => new
                {
                    CompanyRatingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CauseOfReject = table.Column<string>(type: "TEXT", nullable: true),
                    RejectComment = table.Column<string>(type: "TEXT", nullable: true),
                    RejectText = table.Column<string>(type: "TEXT", nullable: true),
                    Commentary = table.Column<string>(type: "TEXT", nullable: true),
                    CreditLimit = table.Column<int>(type: "INTEGER", nullable: true),
                    Currency = table.Column<string>(type: "TEXT", nullable: true),
                    Rating = table.Column<int>(type: "INTEGER", nullable: true),
                    RatingText = table.Column<string>(type: "TEXT", nullable: true),
                    RiskPrognosis = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyRatings", x => x.CompanyRatingId);
                });

            migrationBuilder.CreateTable(
                name: "FinancialRecords",
                columns: table => new
                {
                    FinancialRecordID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    FromDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ToDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BsShareCapital = table.Column<decimal>(type: "TEXT", nullable: false),
                    NumberOfShares = table.Column<int>(type: "INTEGER", nullable: false),
                    KpiEbitda = table.Column<decimal>(type: "TEXT", nullable: false),
                    KpiEbitdaMarginPercent = table.Column<decimal>(type: "TEXT", nullable: false),
                    KpiReturnOnEquityPercent = table.Column<decimal>(type: "TEXT", nullable: false),
                    PlNetOperatingIncome = table.Column<decimal>(type: "TEXT", nullable: false),
                    PlSales = table.Column<decimal>(type: "TEXT", nullable: false),
                    PlEbit = table.Column<decimal>(type: "TEXT", nullable: false),
                    PlProfitLossAfterFinItems = table.Column<decimal>(type: "TEXT", nullable: false),
                    PlNetProfitLoss = table.Column<decimal>(type: "TEXT", nullable: false),
                    BsTotalInventories = table.Column<decimal>(type: "TEXT", nullable: false),
                    BsCashAndBankBalances = table.Column<decimal>(type: "TEXT", nullable: false),
                    BsTotalEquity = table.Column<decimal>(type: "TEXT", nullable: false),
                    BsTotalLongTermDebts = table.Column<decimal>(type: "TEXT", nullable: false),
                    BsTotalCurrentLiabilities = table.Column<decimal>(type: "TEXT", nullable: false),
                    BsTotalEquityAndLiabilities = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialRecords", x => x.FinancialRecordID);
                });

            migrationBuilder.CreateTable(
                name: "CompanyStructures",
                columns: table => new
                {
                    CompanyStructureId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    MotherCompanyId = table.Column<int>(type: "INTEGER", nullable: false),
                    CompanyLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    OwnedPercentage = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyStructures", x => x.CompanyStructureId);
                    table.ForeignKey(
                        name: "FK_CompanyStructures_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyStructures_Companies_MotherCompanyId",
                        column: x => x.MotherCompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyStructures_CompanyId",
                table: "CompanyStructures",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyStructures_MotherCompanyId",
                table: "CompanyStructures",
                column: "MotherCompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "CompanyEmployees");

            migrationBuilder.DropTable(
                name: "CompanyRatings");

            migrationBuilder.DropTable(
                name: "CompanyStructures");

            migrationBuilder.DropTable(
                name: "FinancialRecords");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
