﻿// <auto-generated />
using System;
using DailySpendBudgetWebApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DailySpendBudgetWebApp.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20230314181008_230314dbupdates")]
    partial class _230314dbupdates
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
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("BillCurrentBalance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("BillDailyValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("BillDueDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("BillName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BillType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("BudgetsBudgetID")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("LastUpdatedValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("RecuringPeriod")
                        .HasColumnType("int");

                    b.Property<string>("RecuringType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isRecuring")
                        .HasColumnType("bit");

                    b.HasKey("BillID");

                    b.HasIndex("BudgetsBudgetID");

                    b.ToTable("Bills");
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

                    b.Property<decimal?>("LeftToSPendBalance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("LeftToSpendDailyAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("MoneyAvailableBalance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("NextIncomePayday")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("PaydayAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("PaydayDuration")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaydayType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PaydayValue")
                        .HasColumnType("int");

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

                    b.Property<string>("CurrencyDecimalSeparator")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("CurrencyGroupSeparator")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("CurrencyPattern")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("CurrencySymbol")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("DateSeperator")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("ShortDatePattern")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

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

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("LastTransactionID")
                        .HasColumnType("int");

                    b.Property<int?>("NumberOfTransactions")
                        .HasColumnType("int");

                    b.Property<decimal?>("TotalCategorySpend")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("CategoryID");

                    b.HasIndex("BudgetsBudgetID");

                    b.ToTable("Categories");
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

                    b.Property<decimal>("IncomeAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("IncomeCategory")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("IncomeDailyValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("IncomeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RecurringIncomePeriod")
                        .HasColumnType("int");

                    b.Property<string>("RecurringIncomeType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isActive")
                        .HasColumnType("bit");

                    b.Property<bool>("isMainIncome")
                        .HasColumnType("bit");

                    b.Property<bool>("isRecurringIncome")
                        .HasColumnType("bit");

                    b.HasKey("IncomeEventID");

                    b.HasIndex("BudgetsBudgetID");

                    b.ToTable("IncomeEvents");
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

                    b.Property<int>("CategoryID")
                        .HasColumnType("int");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TransactionAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("isSpendFromSavings")
                        .HasColumnType("bit");

                    b.HasKey("TransactionID");

                    b.HasIndex("BudgetsBudgetID");

                    b.HasIndex("CategoryID");

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

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.Bills", b =>
                {
                    b.HasOne("DailySpendBudgetWebApp.Models.Budgets", null)
                        .WithMany("Bills")
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

                    b.HasOne("DailySpendBudgetWebApp.Models.Categories", "Category")
                        .WithMany("Transactions")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.UserAccounts", b =>
                {
                    b.HasOne("DailySpendBudgetWebApp.Models.Budgets", "Budget")
                        .WithMany()
                        .HasForeignKey("DefaultBudgetID");

                    b.Navigation("Budget");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.Budgets", b =>
                {
                    b.Navigation("Bills");

                    b.Navigation("Categories");

                    b.Navigation("IncomeEvents");

                    b.Navigation("Savings");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("DailySpendBudgetWebApp.Models.Categories", b =>
                {
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
