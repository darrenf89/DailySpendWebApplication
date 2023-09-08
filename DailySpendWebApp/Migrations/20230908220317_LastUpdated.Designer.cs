﻿// <auto-generated />
using System;
using DailySpendBudgetWebApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DailySpendWebApp.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20230908220317_LastUpdated")]
    partial class LastUpdated
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.Bills", b =>
                {
                    b.Property<int>("BillID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BillID"), 1L, 1);

                    b.Property<decimal?>("BillAmount")
                        .IsRequired()
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("BillCurrentBalance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("BillDueDate")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("BillDuration")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BillName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BillType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("BillValue")
                        .HasColumnType("int");

                    b.Property<int?>("BudgetsBudgetID")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("RegularBillValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("isClosed")
                        .HasColumnType("bit");

                    b.Property<bool>("isRecuring")
                        .HasColumnType("bit");

                    b.HasKey("BillID");

                    b.HasIndex("BudgetsBudgetID");

                    b.ToTable("Bills");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.BudgetHstoryLastPeriod", b =>
                {
                    b.Property<int>("BudgetHistoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BudgetHistoryID"), 1L, 1);

                    b.Property<decimal?>("BankBalance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("BudgetsBudgetID")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("LeftToSPendBalance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("MoneyAvailableBalance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("TotalBillsLastPeriod")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("TotalEarnedLastPeriod")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("TotalSavedlastPeriod")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("TotalSpentLastPeriod")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("BudgetHistoryID");

                    b.HasIndex("BudgetsBudgetID");

                    b.ToTable("BudgetHstoryLastPeriod");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.Budgets", b =>
                {
                    b.Property<int>("BudgetID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BudgetID"), 1L, 1);

                    b.Property<int?>("AproxDaysBetweenPay")
                        .HasColumnType("int");

                    b.Property<decimal?>("BankBalance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("BudgetCreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("BudgetName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("BudgetValuesLastUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("CurrencyType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("DailyBillOutgoing")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("DailySavingOutgoing")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("IsCreated")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("LeftToSpendBalance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("LeftToSpendDailyAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("MoneyAvailableBalance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("NextIncomePayday")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("NextIncomePaydayCalculated")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("PaydayAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("PaydayDuration")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaydayType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PaydayValue")
                        .HasColumnType("int");

                    b.Property<decimal?>("StartDayDailyAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("UserAccountsUserID")
                        .HasColumnType("int");

                    b.HasKey("BudgetID");

                    b.HasIndex("UserAccountsUserID");

                    b.ToTable("Budgets");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.BudgetSettings", b =>
                {
                    b.Property<int>("SettingsID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SettingsID"), 1L, 1);

                    b.Property<int?>("BudgetID")
                        .HasColumnType("int");

                    b.Property<int?>("CurrencyDecimalDigits")
                        .HasColumnType("int");

                    b.Property<int?>("CurrencyDecimalSeparator")
                        .HasMaxLength(5)
                        .HasColumnType("int");

                    b.Property<int?>("CurrencyGroupSeparator")
                        .HasMaxLength(5)
                        .HasColumnType("int");

                    b.Property<int?>("CurrencyPattern")
                        .HasMaxLength(5)
                        .HasColumnType("int");

                    b.Property<int?>("CurrencySymbol")
                        .HasMaxLength(5)
                        .HasColumnType("int");

                    b.Property<int?>("DateSeperator")
                        .HasMaxLength(5)
                        .HasColumnType("int");

                    b.Property<int?>("ShortDatePattern")
                        .HasMaxLength(10)
                        .HasColumnType("int");

                    b.HasKey("SettingsID");

                    b.HasIndex("BudgetID");

                    b.ToTable("BudgetSettings");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.Categories", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryID"), 1L, 1);

                    b.Property<int?>("BudgetsBudgetID")
                        .HasColumnType("int");

                    b.Property<int?>("CategoryGroupID")
                        .HasColumnType("int");

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isSubCategory")
                        .HasColumnType("bit");

                    b.HasKey("CategoryID");

                    b.HasIndex("BudgetsBudgetID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.ErrorLog", b =>
                {
                    b.Property<int>("ErrorLogID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ErrorLogID"), 1L, 1);

                    b.Property<int>("BudgetID")
                        .HasColumnType("int");

                    b.Property<string>("DeviceIdiom")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeviceModel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeviceName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DeviceOSVersion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DevicePlatform")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ErrorMethod")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ErrorPage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("WhenAdded")
                        .HasColumnType("datetime2");

                    b.HasKey("ErrorLogID");

                    b.ToTable("ErrorLog");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.IncomeEvents", b =>
                {
                    b.Property<int>("IncomeEventID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IncomeEventID"), 1L, 1);

                    b.Property<int?>("BudgetsBudgetID")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfIncomeEvent")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("IncomeActiveDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("IncomeAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("IncomeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecurringIncomeDuration")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecurringIncomeType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RecurringIncomeValue")
                        .HasColumnType("int");

                    b.Property<bool>("isClosed")
                        .HasColumnType("bit");

                    b.Property<bool?>("isIncomeAddedToBalance")
                        .HasColumnType("bit");

                    b.Property<bool?>("isInstantActive")
                        .HasColumnType("bit");

                    b.Property<bool>("isRecurringIncome")
                        .HasColumnType("bit");

                    b.HasKey("IncomeEventID");

                    b.HasIndex("BudgetsBudgetID");

                    b.ToTable("IncomeEvents");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.lut_CurrencyDecimalDigits", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<string>("CurrencyDecimalDigits")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("lut_CurrencyDecimalDigits");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.lut_CurrencyDecimalSeparator", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<string>("CurrencyDecimalSeparator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("lut_CurrencyDecimalSeparators");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.lut_CurrencyGroupSeparator", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<string>("CurrencyGroupSeparator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("lut_CurrencyGroupSeparators");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.lut_CurrencyPlacement", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<string>("CurrencyPlacement")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CurrencyPositivePatternRef")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("lut_CurrencyPlacements");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.lut_CurrencySymbol", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CurrencySymbol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("lut_CurrencySymbols");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.lut_DateFormat", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<string>("DateFormat")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DateSeperatorID")
                        .HasColumnType("int");

                    b.Property<int>("ShortDatePatternID")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("lut_DateFormats");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.lut_DateSeperator", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<string>("DateSeperator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("lut_DateSeperators");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.lut_NumberFormat", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<int>("CurrencyDecimalDigitsID")
                        .HasColumnType("int");

                    b.Property<int>("CurrencyDecimalSeparatorID")
                        .HasColumnType("int");

                    b.Property<int>("CurrencyGroupSeparatorID")
                        .HasColumnType("int");

                    b.Property<string>("NumberFormat")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("lut_NumberFormats");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.lut_ShortDatePattern", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<string>("ShortDatePattern")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("lut_ShortDatePatterns");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.Savings", b =>
                {
                    b.Property<int>("SavingID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SavingID"), 1L, 1);

                    b.Property<int?>("BudgetsBudgetID")
                        .HasColumnType("int");

                    b.Property<decimal?>("CurrentBalance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("GoalDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastUpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("LastUpdatedValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("PeriodSavingValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("RegularSavingValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("SavingsGoal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("SavingsName")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("SavingsType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("canExceedGoal")
                        .HasColumnType("bit");

                    b.Property<string>("ddlSavingsPeriod")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isAutoComplete")
                        .HasColumnType("bit");

                    b.Property<bool>("isDailySaving")
                        .HasColumnType("bit");

                    b.Property<bool>("isRegularSaving")
                        .HasColumnType("bit");

                    b.Property<bool>("isSavingsClosed")
                        .HasColumnType("bit");

                    b.HasKey("SavingID");

                    b.HasIndex("BudgetsBudgetID");

                    b.ToTable("Savings");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.Transactions", b =>
                {
                    b.Property<int>("TransactionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionID"), 1L, 1);

                    b.Property<int?>("BudgetsBudgetID")
                        .HasColumnType("int");

                    b.Property<string>("Category")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CategoryID")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Payee")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SavingID")
                        .HasColumnType("int");

                    b.Property<string>("SavingName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SavingsSpendType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("TransactionAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("WhenAdded")
                        .HasColumnType("datetime2");

                    b.Property<bool>("isIncome")
                        .HasColumnType("bit");

                    b.Property<bool>("isSpendFromSavings")
                        .HasColumnType("bit");

                    b.Property<bool>("isTransacted")
                        .HasColumnType("bit");

                    b.Property<string>("stage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TransactionID");

                    b.HasIndex("BudgetsBudgetID");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.UserAccounts", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"), 1L, 1);

                    b.Property<DateTime>("AccountCreated")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DefaultBudgetID")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<DateTime?>("LastLoggedOn")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("NickName")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isAgreedToTerms")
                        .HasColumnType("bit");

                    b.Property<bool>("isDPAPermissions")
                        .HasColumnType("bit");

                    b.Property<bool>("isEmailVerified")
                        .HasColumnType("bit");

                    b.HasKey("UserID");

                    b.HasIndex("DefaultBudgetID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("UserAccounts");
                });

            modelBuilder.Entity("DailySpendWebApp.Models.PayPeriodStats", b =>
                {
                    b.Property<int>("PayPeriodID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PayPeriodID"), 1L, 1);

                    b.Property<decimal>("BillsToDate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("BudgetsBudgetID")
                        .HasColumnType("int");

                    b.Property<int>("DurationOfPeriod")
                        .HasColumnType("int");

                    b.Property<decimal?>("EndBBPeiordAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("EndLtSDailyAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("EndLtSPeiordAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("EndMaBPeiordAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("IncomeToDate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("SavingsToDate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("SpendToDate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("StartBBPeiordAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("StartLtSDailyAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("StartLtSPeiordAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("StartMaBPeiordAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("isCurrentPeriod")
                        .HasColumnType("bit");

                    b.HasKey("PayPeriodID");

                    b.HasIndex("BudgetsBudgetID");

                    b.ToTable("PayPeriodStats");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.Bills", b =>
                {
                    b.HasOne("DailySpendBudgetWebApp.Models.Budgets", null)
                        .WithMany("Bills")
                        .HasForeignKey("BudgetsBudgetID");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.BudgetHstoryLastPeriod", b =>
                {
                    b.HasOne("DailySpendBudgetWebApp.Models.Budgets", null)
                        .WithMany("BudgetHistory")
                        .HasForeignKey("BudgetsBudgetID");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.Budgets", b =>
                {
                    b.HasOne("DailySpendBudgetWebApp.Models.UserAccounts", null)
                        .WithMany("Budgets")
                        .HasForeignKey("UserAccountsUserID");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.BudgetSettings", b =>
                {
                    b.HasOne("DailySpendBudgetWebApp.Models.Budgets", "Budget")
                        .WithMany()
                        .HasForeignKey("BudgetID");

                    b.Navigation("Budget");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.Categories", b =>
                {
                    b.HasOne("DailySpendBudgetWebApp.Models.Budgets", null)
                        .WithMany("Categories")
                        .HasForeignKey("BudgetsBudgetID");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.IncomeEvents", b =>
                {
                    b.HasOne("DailySpendBudgetWebApp.Models.Budgets", null)
                        .WithMany("IncomeEvents")
                        .HasForeignKey("BudgetsBudgetID");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.Savings", b =>
                {
                    b.HasOne("DailySpendBudgetWebApp.Models.Budgets", null)
                        .WithMany("Savings")
                        .HasForeignKey("BudgetsBudgetID");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.Transactions", b =>
                {
                    b.HasOne("DailySpendBudgetWebApp.Models.Budgets", null)
                        .WithMany("Transactions")
                        .HasForeignKey("BudgetsBudgetID");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.UserAccounts", b =>
                {
                    b.HasOne("DailySpendBudgetWebApp.Models.Budgets", "Budget")
                        .WithMany()
                        .HasForeignKey("DefaultBudgetID");

                    b.Navigation("Budget");
                });

            modelBuilder.Entity("DailySpendWebApp.Models.PayPeriodStats", b =>
                {
                    b.HasOne("DailySpendBudgetWebApp.Models.Budgets", null)
                        .WithMany("PayPeriodStats")
                        .HasForeignKey("BudgetsBudgetID");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.Budgets", b =>
                {
                    b.Navigation("Bills");

                    b.Navigation("BudgetHistory");

                    b.Navigation("Categories");

                    b.Navigation("IncomeEvents");

                    b.Navigation("PayPeriodStats");

                    b.Navigation("Savings");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.UserAccounts", b =>
                {
                    b.Navigation("Budgets");
                });
#pragma warning restore 612, 618
        }
    }
}
