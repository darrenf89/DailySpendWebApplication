using DailySpendBudgetWebApp.Models;
using DailySpendBudgetWebApp.Data;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Security.Claims;
using System.Text;
using DailySpendBudgetWebApp.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace DailySpendWebApp.Services
{
    public class ProductTools: IProductTools
    {

        private readonly ApplicationDBContext _db;

        public ProductTools( ApplicationDBContext db)
        {
            _db = db;
        }        

        public string UpdateBudgetCreateSavings(int BudgetID)
        {
            if (BudgetID == 0)
            {
                return "No Budget Detected";
            }
            else 
            { 

                Budgets? Budget = _db.Budgets?
                    .Include(x => x.Savings.Where(s => s.isSavingsClosed == false))
                    .Where(x => x.BudgetID == BudgetID)
                    .FirstOrDefault();

                int? DaysBetweenPay = Budget.AproxDaysBetweenPay;

                foreach (Savings Saving in Budget.Savings)
                {
                    if(Saving.isRegularSaving)
                    {
                        if (!Saving.isDailySaving)
                        {
                            if(Saving.SavingsType == "TargetAmount")
                            {
                                //Recalculate Date and daily amount
                                Saving.RegularSavingValue = Saving.PeriodSavingValue / DaysBetweenPay;

                                decimal? BalanceLeft = Saving.SavingsGoal - (Saving.CurrentBalance ?? 0);
                                int NumberOfDays = (int)Math.Ceiling(BalanceLeft / Saving.RegularSavingValue ?? 0);

                                DateTime Today = DateTime.UtcNow;
                                Saving.GoalDate = Today.AddDays(NumberOfDays);

                            }   
                            else if (Saving.SavingsType == "SavingsBuilder")
                            {
                                //Recalculate daily amount.
                                Saving.RegularSavingValue = Saving.PeriodSavingValue / DaysBetweenPay;
                            }
                        }
                    }
                    _db.SaveChanges();
                }
            }
            return "OK"; 
        }

        public string UpdateBudgetCreateIncome(int BudgetID)
        {
            if (BudgetID == 0)
            {
                return "No Budget Detected";
            }
            else
            {
                Budgets? Budget = _db.Budgets?
                    .Include(i => i.IncomeEvents.Where(i => i.isClosed == false))
                    .Where(x => x.BudgetID == BudgetID)
                    .FirstOrDefault();

                DateTime Today = DateTime.Now;

                foreach (IncomeEvents Income in Budget.IncomeEvents)
                {
                    //Check if income active date has passed and process accordingly
                    if (Income.IncomeActiveDate.Date <= Today.Date)
                    {
                        //Check if its an instantactive Income
                        if (Income.isInstantActive ?? false)
                        {
                            // if  instantactive Income Update MoneyAvilable and Left to spend then check if its recurring
                            Budget.MoneyAvailableBalance = Budget.MoneyAvailableBalance + Income.IncomeAmount;
                            Budget.LeftToSpendBalance = Budget.LeftToSpendBalance + Income.IncomeAmount;
                            //TODO: Add an Instant Active Income Transaction into transactions
                            //Calculate the Next Active Income Date if its recurring. Should go active on the date of the payday directly before the next DateOfIncomeEvent.
                            if (Income.isRecurringIncome)
                            {
                                DateTime NextIncomeDate = CalculateNextDate(Income.DateOfIncomeEvent, Income.RecurringIncomeType, Income.RecurringIncomeValue ?? 1, Income.RecurringIncomeDuration);
                                DateTime NextPayDay = Budget.NextIncomePayday ?? default;
                                DateTime PayDayAfterNext = CalculateNextDate(NextPayDay, Budget.PaydayType, Budget.PaydayValue ?? 1, Budget.PaydayDuration);
                                if (NextIncomeDate.Date < NextPayDay.Date) 
                                { 
                                    while (NextIncomeDate.Date < NextPayDay.Date)
                                    {
                                        Budget.MoneyAvailableBalance = Budget.MoneyAvailableBalance + Income.IncomeAmount;
                                        Budget.LeftToSpendBalance = Budget.LeftToSpendBalance + Income.IncomeAmount;
                                        //TODO: Add a Transaction into transactions
                                        NextIncomeDate = CalculateNextDate(NextIncomeDate, Income.RecurringIncomeType, Income.RecurringIncomeValue ?? 1, Income.RecurringIncomeDuration);
                                    }
                                }
                                else
                                {
                                    while (NextIncomeDate.Date > PayDayAfterNext) 
                                    {
                                        NextPayDay = PayDayAfterNext;
                                        PayDayAfterNext = CalculateNextDate(NextPayDay, Budget.PaydayType, Budget.PaydayValue ?? 1, Budget.PaydayDuration);
                                    }
                                    Income.IncomeActiveDate = NextPayDay.Date;
                                }
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            Budget.BankBalance = Budget.BankBalance + Income.IncomeAmount;
                            Budget.MoneyAvailableBalance = Budget.MoneyAvailableBalance + Income.IncomeAmount;
                            Budget.LeftToSpendBalance = Budget.LeftToSpendBalance + Income.IncomeAmount;
                            //TODO: Add a Transaction into transactions
                            if (Income.isRecurringIncome)
                            {
                                //Calculate the next DateOfIncomeEvent and set IncomeActiveDate To this as well!
                                DateTime NextDate = CalculateNextDate(Income.DateOfIncomeEvent, Income.RecurringIncomeType, Income.RecurringIncomeValue ?? 1, Income.RecurringIncomeDuration);
                                Income.DateOfIncomeEvent = NextDate;
                                Income.IncomeActiveDate = NextDate;
                            }
                            else
                            {
                                Income.isClosed = true;
                                Income.isIncomeAddedToBalance = true;
                            }
                        }
                    }

                    //If its an InstantActive Income Check if the Date of Income Event is in the past.
                    if (Income.DateOfIncomeEvent <= Today & Income.isInstantActive ?? false)
                    {
                        Budget.BankBalance = Budget.BankBalance + Income.IncomeAmount;
                        //TODO: Update Instant Active Income Transaction in transactions
                        if (Income.isRecurringIncome)
                        {
                            DateTime NextDate = CalculateNextDate(Income.DateOfIncomeEvent, Income.RecurringIncomeType, Income.RecurringIncomeValue ?? 1, Income.RecurringIncomeDuration);
                            Income.DateOfIncomeEvent = NextDate;
                        }
                        else
                        {
                            Income.isClosed = true;
                            Income.isIncomeAddedToBalance = true;
                        }
                    }
                    _db.SaveChanges();
                }
            }

            return "OK";
        }

        public string UpdateBudgetCreateSavingsSpend(int BudgetID)
        {
            decimal DailySavingOutgoing = new();
            decimal PeriodTotalSavingOutgoing = new();

            if (BudgetID == 0)
            {
                return "No Budget Detected";
            }
            else
            {
                Budgets? Budget = _db.Budgets?
                    .Include(x => x.Savings.Where(s => s.isSavingsClosed == false))
                    .Where(x => x.BudgetID == BudgetID)
                    .FirstOrDefault();

                int DaysToPayDay = (Budget.NextIncomePayday.GetValueOrDefault().Date  - DateTime.Today.Date).Days;

                foreach (Savings Saving in Budget.Savings)
                {
                    if(Saving.isRegularSaving & Saving.SavingsType == "SavingsBuilder")
                    {
                        DailySavingOutgoing += Saving.RegularSavingValue ?? 0;
                        PeriodTotalSavingOutgoing += ((Saving.RegularSavingValue ?? 0) * DaysToPayDay);
                    }
                    else if (Saving.isRegularSaving)
                    {
                        DailySavingOutgoing += Saving.RegularSavingValue ?? 0;
                        //check if goal date is before pay day
                        int DaysToSaving = (Saving.GoalDate.GetValueOrDefault().Date - DateTime.Today.Date).Days;
                        if (DaysToSaving < DaysToPayDay)
                        {
                            PeriodTotalSavingOutgoing += ((Saving.RegularSavingValue ?? 0) * DaysToSaving);
                        }
                        else 
                        {
                            PeriodTotalSavingOutgoing += ((Saving.RegularSavingValue ?? 0) * DaysToPayDay);
                        }

                    }
                }

                Budget.DailySavingOutgoing = DailySavingOutgoing;
                Budget.LeftToSpendBalance = Budget.LeftToSpendBalance - PeriodTotalSavingOutgoing;

                _db.SaveChanges();

                return "OK";
            }
        }

        public string UpdateBudgetCreateBillsSpend(int BudgetID)
        {
            decimal DailyBillOutgoing = new();
            decimal PeriodTotalBillOutgoing = new();

            if (BudgetID == 0)
            {
                return "No Budget Detected";
            }
            else
            {
                Budgets? Budget = _db.Budgets?
                    .Include(x => x.Bills.Where(b => b.isClosed == false))
                    .Where(x => x.BudgetID == BudgetID)
                    .FirstOrDefault();

                int DaysToPayDay = (Budget.NextIncomePayday.GetValueOrDefault().Date - DateTime.Today.Date).Days;

                foreach (Bills Bill in Budget.Bills)
                {
                    DailyBillOutgoing += Bill.RegularBillValue ?? 0;
                    //Check if Due Date is before Pay Dat
                    int DaysToBill = (Bill.BillDueDate.GetValueOrDefault().Date - DateTime.Today.Date).Days;
                    if (DaysToBill < DaysToPayDay)
                    {
                        PeriodTotalBillOutgoing += (Bill.RegularBillValue ?? 0) * DaysToBill;
                    }
                    else
                    {
                        PeriodTotalBillOutgoing += (Bill.RegularBillValue ?? 0) * DaysToPayDay;
                    }                  
                    
                }

                Budget.DailyBillOutgoing = DailyBillOutgoing;
                Budget.LeftToSpendBalance = Budget.LeftToSpendBalance - PeriodTotalBillOutgoing;
                Budget.MoneyAvailableBalance = Budget.MoneyAvailableBalance - PeriodTotalBillOutgoing;

                _db.SaveChanges();
            }            

            return "OK";
        }

        public DateTime CalculateNextDate(DateTime LastDate, string Type, int Value, string? Duration)
        {
            DateTime NextDate = new DateTime();
            string status = "";

            if (Type == "Everynth")
            {
                status = CalculateNextDateEverynth(ref NextDate,  LastDate, Value,  Duration);
            }
            else if (Type == "WorkingDays")
            {
                status = CalculateNextDateWorkingDays(ref NextDate,  LastDate,  Value);
            }
            else if (Type == "OfEveryMonth")
            {
                status = CalculateNextDateOfEveryMonth(ref NextDate,  LastDate,  Value);
            }
            else if (Type == "LastOfTheMonth")
            {
                status = CalculateNextDateLastOfTheMonth(ref NextDate,  LastDate, Duration);
            } 

            if (status == "OK")
            {
                return NextDate;
            }
            else
            {
                throw new Exception(status);
            }
            
        }

        public string CalculateNextDateEverynth(ref DateTime NextDate, DateTime LastDate, int Value, string? Duration)
        {
            try
            {
                int IntDuration;
                if (Duration == "days")
                {
                    IntDuration = 1;
                }
                else if (Duration == "weeks")
                {
                    IntDuration = 7;
                }
                else if (Duration == "years")
                {
                    IntDuration = 365;
                }
                else
                {
                    return "Duration not valid or null";
                }

                int DaysBetween = IntDuration * Value;

                NextDate = LastDate.AddDays(DaysBetween);
            }
            catch (Exception ex) 
            {
                return ex.Message;
            }

            return "OK";
        }

        public string CalculateNextDateWorkingDays(ref DateTime NextDate, DateTime LastDate, int Value)
        {
            try
            {
                int year = LastDate.Year;
                int month = LastDate.Month;

                int NextYear = new int();
                int NextMonth = new int();

                if (month != 12)
                {
                    NextYear = LastDate.Year;
                    NextMonth = month + 1;
                }
                else
                {
                    NextYear = year + 1;
                    NextMonth = 1;
                }

                DateTime NextCurrentDate = new DateTime();
                var i = DateTime.DaysInMonth(NextYear, NextMonth);
                var j = 1;
                while (i > 0)
                {
                    var dtCurrent = new DateTime(NextYear, NextMonth, i);
                    if (dtCurrent.DayOfWeek < DayOfWeek.Saturday && dtCurrent.DayOfWeek > DayOfWeek.Sunday)
                    {
                        NextCurrentDate = dtCurrent;
                        if (j == Value)
                        {
                            i = 0;
                        }
                        else
                        {
                            i = i - 1;
                            j = j + 1;
                        }
                    }
                    else
                    {
                        i = i - 1;
                    }
                }

                NextDate = NextCurrentDate.Date;
            }
            catch (Exception ex) 
            {
                return ex.Message;            
            }

            return "OK";
        }

        public string CalculateNextDateOfEveryMonth(ref DateTime NextDate, DateTime LastDate, int Value)
        {
            try
            {
                NextDate = LastDate.AddMonths(1);
            }
            catch(Exception ex) 
            { 
                return ex.Message; 
            }

            return "OK";
        }

        public string CalculateNextDateLastOfTheMonth(ref DateTime NextDate, DateTime LastDate, string? Duration)
        {

            try
            {
                int dayNumber = ((int)Enum.Parse(typeof(DayOfWeek), Duration));

                int year = LastDate.Year;
                int month = LastDate.Month;

                int NextYear = new int();
                int NextMonth = new int();

                if (month != 12)
                {
                    NextYear = LastDate.Year;
                    NextMonth = month + 1;
                }
                else
                {
                    NextYear = year + 1;
                    NextMonth = 1;
                }

                DateTime NewDate = new DateTime();

                var i = DateTime.DaysInMonth(NextYear, NextMonth);
                while (i > 0)
                {
                    var dtCurrent = new DateTime(NextYear, NextMonth, i);
                    if ((int)dtCurrent.DayOfWeek == dayNumber)
                    {
                        NewDate = dtCurrent;
                        i = 0;
                    }
                    else
                    {
                        i = i - 1;
                    }
                }

                NextDate = NewDate.Date;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "OK";
        }

    }

}
