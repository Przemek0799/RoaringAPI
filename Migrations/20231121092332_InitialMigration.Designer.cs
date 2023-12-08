﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RoaringAPI.Model;

#nullable disable

namespace RoaringAPI.Migrations
{
    [DbContext(typeof(RoaringDbcontext))]
    [Migration("20231121092332_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.13");

            modelBuilder.Entity("RoaringAPI.Model.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AddressLine")
                        .HasColumnType("TEXT");

                    b.Property<string>("CoAddress")
                        .HasColumnType("TEXT");

                    b.Property<string>("Commune")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CommuneCode")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CompanyId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("County")
                        .HasColumnType("TEXT");

                    b.Property<string>("Town")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ZipCode")
                        .HasColumnType("INTEGER");

                    b.HasKey("AddressId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("RoaringAPI.Model.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CompanyName")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CompanyRatingId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("CompanyRegistrationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Currency")
                        .HasColumnType("TEXT");

                    b.Property<bool?>("EmployerContributionReg")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IndustryCode")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IndustryText")
                        .HasColumnType("TEXT");

                    b.Property<string>("LegalGroupCode")
                        .HasColumnType("TEXT");

                    b.Property<string>("LegalGroupText")
                        .HasColumnType("TEXT");

                    b.Property<int?>("NumberCompanyUnits")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NumberEmployeesInterval")
                        .HasColumnType("TEXT");

                    b.Property<int?>("NumberOfEmployees")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("PreliminaryTaxReg")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RoaringCompanyId")
                        .HasColumnType("TEXT");

                    b.Property<bool?>("SeveralCompanyName")
                        .HasColumnType("INTEGER");

                    b.HasKey("CompanyId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("RoaringAPI.Model.CompanyEmployee", b =>
                {
                    b.Property<int>("EmployeeInCompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CompanyId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TopDirectorFunction")
                        .HasColumnType("TEXT");

                    b.Property<string>("TopDirectorName")
                        .HasColumnType("TEXT");

                    b.HasKey("EmployeeInCompanyId");

                    b.ToTable("CompanyEmployees");
                });

            modelBuilder.Entity("RoaringAPI.Model.CompanyRating", b =>
                {
                    b.Property<int>("CompanyRatingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CauseOfReject")
                        .HasColumnType("TEXT");

                    b.Property<string>("Commentary")
                        .HasColumnType("TEXT");

                    b.Property<int?>("CreditLimit")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Currency")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Rating")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RatingText")
                        .HasColumnType("TEXT");

                    b.Property<string>("RejectComment")
                        .HasColumnType("TEXT");

                    b.Property<string>("RejectText")
                        .HasColumnType("TEXT");

                    b.Property<string>("RiskPrognosis")
                        .HasColumnType("TEXT");

                    b.HasKey("CompanyRatingId");

                    b.ToTable("CompanyRatings");
                });

            modelBuilder.Entity("RoaringAPI.Model.CompanyStructure", b =>
                {
                    b.Property<int>("CompanyStructureId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CompanyId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CompanyLevel")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MotherCompanyId")
                        .IsRequired()
                        .HasColumnType("INTEGER");

                    b.Property<double>("OwnedPercentage")
                        .HasColumnType("REAL");

                    b.HasKey("CompanyStructureId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("MotherCompanyId");

                    b.ToTable("CompanyStructures");
                });

            modelBuilder.Entity("RoaringAPI.Model.FinancialRecord", b =>
                {
                    b.Property<int>("FinancialRecordID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("BsCashAndBankBalances")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("BsShareCapital")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("BsTotalCurrentLiabilities")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("BsTotalEquity")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("BsTotalEquityAndLiabilities")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("BsTotalInventories")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("BsTotalLongTermDebts")
                        .HasColumnType("TEXT");

                    b.Property<int>("CompanyId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("FromDate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("KpiEbitda")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("KpiEbitdaMarginPercent")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("KpiReturnOnEquityPercent")
                        .HasColumnType("TEXT");

                    b.Property<int>("NumberOfShares")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("PlEbit")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("PlNetOperatingIncome")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("PlNetProfitLoss")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("PlProfitLossAfterFinItems")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("PlSales")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ToDate")
                        .HasColumnType("TEXT");

                    b.HasKey("FinancialRecordID");

                    b.ToTable("FinancialRecords");
                });

            modelBuilder.Entity("RoaringAPI.Model.CompanyStructure", b =>
                {
                    b.HasOne("RoaringAPI.Model.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RoaringAPI.Model.Company", "MotherCompany")
                        .WithMany()
                        .HasForeignKey("MotherCompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("MotherCompany");
                });
#pragma warning restore 612, 618
        }
    }
}
