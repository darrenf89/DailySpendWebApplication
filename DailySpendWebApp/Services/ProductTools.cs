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
using System.Text.Json;
using System.Globalization;
using DailySpendWebApp.Models;

namespace DailySpendWebApp.Services
{
    public class ProductTools : IProductTools
    {
        public class DefaultCategories
        {
            public string CatName { get; set; }
            public List<SubCategories> SubCategories { get; set; }
        }

        public class SubCategories
        {
            public string SubCatName { get; set; }
        }

        private readonly ApplicationDBContext _db;

        public ProductTools(ApplicationDBContext db)
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
                    if (Saving.isRegularSaving)
                    {
                        if (!Saving.isDailySaving)
                        {
                            if (Saving.SavingsType == "TargetAmount")
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
                    DateTime NextPayDay = Budget.NextIncomePayday ?? default;
                    if (Income.isInstantActive ?? false)
                    {
                        DateTime PayDayAfterNext = CalculateNextDate(NextPayDay, Budget.PaydayType, Budget.PaydayValue ?? 1, Budget.PaydayDuration);
                        DateTime NextIncomeDate = CalculateNextDate(Income.DateOfIncomeEvent, Income.RecurringIncomeType, Income.RecurringIncomeValue ?? 1, Income.RecurringIncomeDuration);
                        //Next Income Date happens in this Pay window so process
                        if(Income.DateOfIncomeEvent.Date < NextPayDay.Date)
                        {
                            Income.IncomeActiveDate = DateTime.UtcNow;
                            Budget.MoneyAvailableBalance = Budget.MoneyAvailableBalance + Income.IncomeAmount;
                            Budget.LeftToSpendBalance = Budget.LeftToSpendBalance + Income.IncomeAmount;
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
                            DateTime CalPayDate = NextPayDay.Date;
                            while (Income.DateOfIncomeEvent.Date >= NextPayDay.Date)
                            {
                                CalPayDate = NextPayDay;
                                NextPayDay = CalculateNextDate(NextPayDay, Budget.PaydayType, Budget.PaydayValue ?? 1, Budget.PaydayDuration);
                            }
                            Income.IncomeActiveDate = CalPayDate.Date;
                        }

                        if(Income.DateOfIncomeEvent <= Today.Date)
                        {
                            Budget.BankBalance = Budget.BankBalance + Income.IncomeAmount;
                            //TODO: Update Instant Active Income Transaction in transactions
                            Income.DateOfIncomeEvent = NextIncomeDate.Date;
                            if (Income.isRecurringIncome)
                            {
                                DateTime CalPayDate = new DateTime();
                                while (NextIncomeDate.Date > NextPayDay.Date)
                                {
                                    CalPayDate = NextPayDay;
                                    NextPayDay = CalculateNextDate(NextPayDay, Budget.PaydayType, Budget.PaydayValue ?? 1, Budget.PaydayDuration);
                                }
                                Income.IncomeActiveDate = CalPayDate.Date;
                            }
                            else
                            {
                                Income.isClosed = true;
                                Income.isIncomeAddedToBalance = true;
                            }
                        }
                    }                    
                    else
                    {
                        if(Income.DateOfIncomeEvent.Date <= Today.Date)
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
                }

            _db.SaveChanges();
            return "OK";

            }
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

                int DaysToPayDay = (Budget.NextIncomePayday.GetValueOrDefault().Date - DateTime.Today.Date).Days;

                foreach (Savings Saving in Budget.Savings)
                {
                    if (Saving.isRegularSaving & Saving.SavingsType == "SavingsBuilder")
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

                    PeriodTotalSavingOutgoing += Saving.CurrentBalance ?? 0;
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

                    PeriodTotalBillOutgoing += Bill.BillCurrentBalance;

                }

                Budget.DailyBillOutgoing = DailyBillOutgoing;
                Budget.LeftToSpendBalance = Budget.LeftToSpendBalance - PeriodTotalBillOutgoing;
                Budget.MoneyAvailableBalance = Budget.MoneyAvailableBalance - PeriodTotalBillOutgoing;

            }
            _db.SaveChanges();
            return "OK";
        }

        public DateTime CalculateNextDate(DateTime LastDate, string Type, int Value, string? Duration)
        {
            DateTime NextDate = new DateTime();
            string status = "";

            if (Type == "Everynth")
            {
                status = CalculateNextDateEverynth(ref NextDate, LastDate, Value, Duration);
            }
            else if (Type == "WorkingDays")
            {
                status = CalculateNextDateWorkingDays(ref NextDate, LastDate, Value);
            }
            else if (Type == "OfEveryMonth")
            {
                status = CalculateNextDateOfEveryMonth(ref NextDate, LastDate, Value);
            }
            else if (Type == "LastOfTheMonth")
            {
                status = CalculateNextDateLastOfTheMonth(ref NextDate, LastDate, Duration);
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
            catch (Exception ex)
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

        public string CreateDefaultCategories(int BudgetID)
        {

            string fileName = "wwwroot/JSON/DefaultCategories.json";
            string jsonString = System.IO.File.ReadAllText(fileName, Encoding.UTF8);
            List<DefaultCategories> DefaultCategories = JsonSerializer.Deserialize<List<DefaultCategories>>(jsonString)!;

            Budgets? Budget = _db.Budgets?
                .Include(x => x.Categories)
                .Where(x => x.BudgetID == BudgetID)
                .FirstOrDefault();

            _db.Attach(Budget);

            foreach (var category in DefaultCategories)
            {
                Categories HeaderCat = new Categories();
                HeaderCat.CategoryName = category.CatName;
                HeaderCat.isSubCategory = false;
                Budget.Categories.Add(HeaderCat);

                _db.SaveChanges();
                HeaderCat.CategoryGroupID = HeaderCat.CategoryID;

                foreach (var item in category.SubCategories)
                {
                    Categories SubCat = new Categories();
                    SubCat.CategoryName = item.SubCatName;
                    SubCat.isSubCategory = true;
                    SubCat.CategoryGroupID = HeaderCat.CategoryID;
                    Budget.Categories.Add(SubCat);

                }
                _db.SaveChanges();
            }

            _db.SaveChanges();

            return "OK";
        }

        public CultureInfo LoadCurrencySetting(int BudgetID)
        {
            BudgetSettings? BS = _db.BudgetSettings?.Where(b => b.BudgetID == BudgetID).FirstOrDefault();
            lut_CurrencySymbol? Symbol = _db.lut_CurrencySymbols?.Where(s => s.id == BS.CurrencySymbol).FirstOrDefault();
            lut_CurrencyDecimalSeparator? DecimalSep = _db.lut_CurrencyDecimalSeparators?.Where(d => d.id == BS.CurrencyDecimalSeparator).FirstOrDefault();
            lut_CurrencyGroupSeparator? GroupSeparator = _db.lut_CurrencyGroupSeparators?.Where(g => g.id == BS.CurrencyGroupSeparator).FirstOrDefault();
            lut_CurrencyDecimalDigits? DecimalDigits = _db.lut_CurrencyDecimalDigits?.Where(d => d.id == BS.CurrencyDecimalDigits).FirstOrDefault();
            lut_CurrencyPlacement? CurrencyPositivePat = _db.lut_CurrencyPlacements?.Where(c => c.id == BS.CurrencyPattern).FirstOrDefault();
            lut_DateSeperator? DateSeperator = _db.lut_DateSeperators?.Where(c => c.id == BS.DateSeperator).FirstOrDefault();
            lut_DateFormat? DateFormat = _db.lut_DateFormats?.Where(c => c.DateSeperatorID == BS.DateSeperator & c.ShortDatePatternID == BS.ShortDatePattern).FirstOrDefault();

            CultureInfo nfi = new CultureInfo("en-GB");

            nfi.NumberFormat.CurrencySymbol = Symbol.CurrencySymbol;
            nfi.NumberFormat.CurrencyDecimalSeparator = DecimalSep.CurrencyDecimalSeparator;
            nfi.NumberFormat.CurrencyGroupSeparator = GroupSeparator.CurrencyGroupSeparator;
            nfi.NumberFormat.CurrencyDecimalDigits = Convert.ToInt32(DecimalDigits.CurrencyDecimalDigits);
            nfi.NumberFormat.CurrencyPositivePattern = CurrencyPositivePat.CurrencyPositivePatternRef;
            nfi.DateTimeFormat.ShortDatePattern = DateFormat.DateFormat;
            nfi.DateTimeFormat.DateSeparator = DateSeperator.DateSeperator;

            return nfi;
        }

        public string TransactTransaction(ref Transactions T, int? BudgetID)
        {
            Budgets? Budget = _db.Budgets?
                .Include(x => x.PayPeriodStats.Where(p => p.isCurrentPeriod))
                .Where(x => x.BudgetID == BudgetID)
                .FirstOrDefault();

            _db.Attach(Budget);

            if (BudgetID == 0)
            {
                return "Couldnt find budget";
            }
            else
            {

                if (T.isIncome)
                {
                    Budget.BankBalance += T.TransactionAmount;
                    Budget.MoneyAvailableBalance += T.TransactionAmount;
                    Budget.LeftToSpendBalance += T.TransactionAmount;
                    //Recalculate how much you have left to spen
                    int DaysToPayDay = (Budget.NextIncomePayday.GetValueOrDefault().Date - DateTime.Today.Date).Days;
                    Budget.LeftToSpendDailyAmount = (Budget.LeftToSpendBalance ?? 0) / DaysToPayDay;
                    Budget.PayPeriodStats[0].IncomeToDate += T.TransactionAmount ?? 0;                    
                }
                else
                {
                    Budget.BankBalance -= T.TransactionAmount;
                    Budget.MoneyAvailableBalance -= T.TransactionAmount;
                    Budget.LeftToSpendBalance -= T.TransactionAmount;
                    Budget.LeftToSpendDailyAmount -= T.TransactionAmount ?? 0;
                    Budget.PayPeriodStats[0].SpendToDate += T.TransactionAmount ?? 0;
                }

                T.isTransacted = true;
                _db.SaveChanges();

                return "OK";                
            }

        }

        public string TransactSavingsTransaction(ref Transactions T, int? BudgetID)
        {
            Budgets? Budget = _db.Budgets?
               .Include(x => x.PayPeriodStats.Where(p => p.isCurrentPeriod))
               .Include(x => x.Savings)
               .Where(x => x.BudgetID == BudgetID)
               .FirstOrDefault();

            int TransactionsSavingsID = T.SavingID ?? 0;

            Savings S = Budget.Savings.Where(s => s.SavingID == TransactionsSavingsID).First();

            _db.Attach(Budget);

            if (BudgetID == 0)
            {
                return "Couldnt find budget";
            }
            else
            {
                if (T.SavingsSpendType == "UpdateValues")
                {
                    if (T.isIncome)
                    {
                        Budget.BankBalance += T.TransactionAmount;
                        Budget.MoneyAvailableBalance += T.TransactionAmount;
                        Budget.PayPeriodStats[0].IncomeToDate += T.TransactionAmount ?? 0;
                        Budget.PayPeriodStats[0].SavingsToDate += T.TransactionAmount ?? 0;
                        S.CurrentBalance += T.TransactionAmount;
                        S.LastUpdatedValue = T.TransactionAmount;
                        S.LastUpdatedDate = DateTime.UtcNow;
                    }
                    else
                    {
                        Budget.BankBalance -= T.TransactionAmount;
                        Budget.MoneyAvailableBalance -= T.TransactionAmount;
                        S.CurrentBalance -= T.TransactionAmount;
                        S.LastUpdatedValue = T.TransactionAmount;
                        S.LastUpdatedDate = DateTime.UtcNow;
                        Budget.PayPeriodStats[0].SpendToDate += T.TransactionAmount ?? 0;
                    }

                    _db.SaveChanges();

                    //Update Regular Saving Value
                    RecalculateRegularSavingFromTransaction(ref S);
                    
                }
                else if (T.SavingsSpendType == "MaintainValues")
                {
                    if (T.isIncome)
                    {
                        Budget.BankBalance += T.TransactionAmount;
                        Budget.MoneyAvailableBalance += T.TransactionAmount;
                        S.CurrentBalance += T.TransactionAmount;
                        S.SavingsGoal += T.TransactionAmount;
                        S.LastUpdatedValue = T.TransactionAmount;
                        S.LastUpdatedDate = DateTime.UtcNow;
                        Budget.PayPeriodStats[0].IncomeToDate += T.TransactionAmount ?? 0;
                        Budget.PayPeriodStats[0].SavingsToDate += T.TransactionAmount ?? 0;
                    }
                    else
                    {
                        Budget.BankBalance -= T.TransactionAmount;
                        Budget.MoneyAvailableBalance -= T.TransactionAmount;
                        S.CurrentBalance -= T.TransactionAmount;
                        S.SavingsGoal -= T.TransactionAmount;
                        S.LastUpdatedValue = T.TransactionAmount;
                        S.LastUpdatedDate = DateTime.UtcNow;
                        Budget.PayPeriodStats[0].SpendToDate += T.TransactionAmount ?? 0;
                    }
                }
                else if (T.SavingsSpendType == "BuildingSaving" | T.SavingsSpendType == "EnvelopeSaving")
                {
                    if (T.isIncome)
                    {
                        Budget.BankBalance += T.TransactionAmount;
                        Budget.MoneyAvailableBalance += T.TransactionAmount;
                        S.CurrentBalance += T.TransactionAmount;
                        S.LastUpdatedValue = T.TransactionAmount;
                        S.LastUpdatedDate = DateTime.UtcNow;
                        Budget.PayPeriodStats[0].IncomeToDate += T.TransactionAmount ?? 0;
                        Budget.PayPeriodStats[0].SavingsToDate += T.TransactionAmount ?? 0;
                    }
                    else
                    {
                        Budget.BankBalance -= T.TransactionAmount;
                        Budget.MoneyAvailableBalance -= T.TransactionAmount;
                        S.CurrentBalance -= T.TransactionAmount;
                        S.LastUpdatedValue = T.TransactionAmount;
                        S.LastUpdatedDate = DateTime.UtcNow;
                        Budget.PayPeriodStats[0].SpendToDate += T.TransactionAmount ?? 0;
                    }
                }

                T.isTransacted = true;
                _db.SaveChanges();

                return "OK";
            }

        }

        public string RecalculateRegularSavingFromTransaction(ref Savings S)
        {
            if(S.isRegularSaving)
            {
                if (S.SavingsType == "TargetAmount")
                {
                    CalculateSavingsTargetAmount(ref S);
                }
                else if (S.SavingsType == "TargetDate")
                {
                    CalculateSavingsTargetDate(ref S);
                }
            }

            return "OK";
        }

        public string CalculateSavingsTargetAmount(ref Savings S)
        {
            decimal? BalanceLeft = S.SavingsGoal - (S.CurrentBalance ?? 0);
            int NumberOfDays = (int)Math.Ceiling(BalanceLeft / S.RegularSavingValue ?? 0);

            DateTime Today = DateTime.UtcNow;
            S.GoalDate = Today.AddDays(NumberOfDays);

            return "OK";
        }

        public string CalculateSavingsTargetDate(ref Savings S)
        {
            int DaysToSavingDate = (S.GoalDate.GetValueOrDefault().Date - DateTime.Today.Date).Days;
            decimal? AmountOutstanding = S.SavingsGoal - S.CurrentBalance;

            S.RegularSavingValue = AmountOutstanding / DaysToSavingDate;

            return "OK";
        }

        public string CreateNewPayPeriodStats(int? BudgetID)
        {
            Budgets? Budget = _db.Budgets?
               .Include(x => x.PayPeriodStats.Where(p => p.isCurrentPeriod))
               .Where(x => x.BudgetID == BudgetID)
               .FirstOrDefault();

            _db.Attach(Budget);

            Budget.PayPeriodStats[0].isCurrentPeriod = false;

            PayPeriodStats stats = new PayPeriodStats();

            stats.isCurrentPeriod = true;

            stats.StartDate = DateTime.UtcNow;
            stats.EndDate = Budget.NextIncomePayday ?? DateTime.UtcNow;
            stats.DurationOfPeriod = Budget.AproxDaysBetweenPay ?? 0;

            stats.SavingsToDate = 0;
            stats.BillsToDate = 0;
            stats.IncomeToDate = 0;
            stats.SpendToDate = 0;

            stats.StartLtSDailyAmount = Budget.LeftToSpendDailyAmount;
            stats.StartLtSPeiordAmount = Budget.LeftToSpendBalance;
            stats.StartBBPeiordAmount = Budget.BankBalance;
            stats.StartMaBPeiordAmount = Budget.MoneyAvailableBalance;

            Budget.PayPeriodStats.Add(stats);
            _db.SaveChanges();

            return "OK";
        }

        public string GetBudgetDatePattern(int BudgetID)
        {
            if(BudgetID == 0)
            {

                return "dd MM yyyy";
            }
            else
            {
                BudgetSettings? BS = _db.BudgetSettings.Where(x => x.BudgetID == BudgetID).FirstOrDefault();

                lut_ShortDatePattern? DatePattern = _db.lut_ShortDatePatterns.Where(x => x.id == BS.ShortDatePattern).FirstOrDefault();
                lut_DateSeperator? dateSeperator = _db.lut_DateSeperators.Where(x => x.id == BS.DateSeperator).FirstOrDefault();

                lut_DateFormat? DateFormat = _db.lut_DateFormats.Where(d => d.DateSeperatorID == dateSeperator.id & d.ShortDatePatternID == DatePattern.id).FirstOrDefault(); 

                return DateFormat.DateFormat ?? "dd MM yyyy";
            }

        }

        public string GetBudgetShortDatePattern(int BudgetID)
        {
            if (BudgetID == 0)
            {

                return "ddMMyyyy";
            }
            else
            {
                BudgetSettings? BS = _db.BudgetSettings.Where(x => x.BudgetID == BudgetID).FirstOrDefault();

                lut_ShortDatePattern? DatePattern = _db.lut_ShortDatePatterns.Where(x => x.id == BS.ShortDatePattern).FirstOrDefault();

                return DatePattern.ShortDatePattern ?? "ddMMyyyy";
            }

        }
        public string UpdatePayPeriodStats(int? BudgetID)
        {
            Budgets? Budget = _db.Budgets?
               .Include(x => x.PayPeriodStats.Where(p => p.isCurrentPeriod))
               .Where(x => x.BudgetID == BudgetID)
               .FirstOrDefault();

            _db.Attach(Budget);

            PayPeriodStats stats = Budget.PayPeriodStats[0];
            _db.Attach(stats);

            stats.EndDate = Budget.NextIncomePayday ?? DateTime.UtcNow;
            stats.DurationOfPeriod = (stats.EndDate - stats.StartDate).Days;

            stats.StartLtSDailyAmount = Budget.LeftToSpendDailyAmount;
            stats.StartLtSPeiordAmount = Budget.LeftToSpendBalance;
            stats.StartBBPeiordAmount = Budget.BankBalance;
            stats.StartMaBPeiordAmount = Budget.MoneyAvailableBalance;

            _db.SaveChanges();

            return "OK";
        }
        public string RecalculateBudgetDetails(int? BudgetID)
        {
            string status = "OK";
            Budgets? Budget = _db.Budgets?
               .Where(x => x.BudgetID == BudgetID)
               .FirstOrDefault();

            _db.Attach(Budget);

            Budget.MoneyAvailableBalance = Budget.BankBalance;
            Budget.LeftToSpendBalance = Budget.BankBalance;

            status = UpdateBudgetCreateSavings(BudgetID ?? 0);
            status = UpdateBudgetCreateIncome(BudgetID ?? 0);
            status = UpdateBudgetCreateSavingsSpend(BudgetID ?? 0);
            status = UpdateBudgetCreateBillsSpend(BudgetID?? 0);
            
            return status;
        }

        public string RegularBudgetUpdateLoop(int? BudgetID)
        {
            string status = "OK";
            if (BudgetID == 0)
            {
                return "Couldnt find budget";
            }
            else
            {
                Budgets Budget = _db.Budgets
                    .Where(x => x.BudgetID == BudgetID)
                    .First();

                _db.Attach(Budget);

                DateTime Today = DateTime.Now.Date;
                DateTime LastBudgetUpdated = Budget.BudgetValuesLastUpdated.Date;

                while(LastBudgetUpdated < Today)
                {
                    status = BudgetUpdateDailyy(Budget.BudgetID, LastBudgetUpdated);
                    if(status == "OK")
                    {
                        LastBudgetUpdated = LastBudgetUpdated.AddDays(1);
                        Budget.BudgetValuesLastUpdated = LastBudgetUpdated;
                    }
                    else
                    {
                        break;
                    }
                }

            }

            return "OK";
        }

        public string BudgetUpdateDailyy(int BudgetID, DateTime LastUpdated)
        {
            try
            {
                Budgets Budget = _db.Budgets
                    .Include(x => x.PayPeriodStats.Where(p => p.isCurrentPeriod))
                    .Include(x => x.Transactions.Where(t => !t.isTransacted))
                    .Include(x => x.Savings.Where(s => !s.isSavingsClosed))
                    .Include(x => x.Bills.Where(b => !b.isClosed))
                    .Include(x => x.IncomeEvents.Where(i => !i.isClosed))
                    .Where(x => x.BudgetID == BudgetID)
                    .First();

                //Process All Types
                
                UpdateSavingsDaily(ref Budget);
                //ProcessBillsDaily(ref Budget);
                //ProcessIncomeDaily(ref Budget);
                UpdateTransactionDaily(ref Budget);

                //Check if PayDay

                //Recalculate Balances

                //Upate Stats if needed

                DateTime NextPayDay = Budget.NextIncomePayday.GetValueOrDefault().Date;

            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
            
            _db.SaveChanges();
            return "OK";
        }

        public void UpdateTransactionDaily(ref Budgets Budget)
        {
            foreach(Transactions Transaction in Budget.Transactions)
            {
                if(Transaction.TransactionDate.GetValueOrDefault().Date <= DateTime.Now.Date)
                {
                    if(Transaction.isSpendFromSavings)
                    {
                        TransactSavingsTransactionDaily(ref Budget, Transaction.TransactionID);
                    }
                    else
                    {
                        TransactTransactionDaily(ref Budget, Transaction.TransactionID);
                    }
                    Budget.PayPeriodStats[0].SpendToDate += Transaction.TransactionAmount ?? 0;
                    Transaction.isTransacted = true;
                }
            }
        }

        public void TransactSavingsTransactionDaily(ref Budgets Budget, int ID)
        {
            Transactions T = Budget.Transactions.Where(t => t.TransactionID == ID).First();
            int TransactionsSavingsID = T.SavingID ?? 0;

            Savings S = Budget.Savings.Where(s => s.SavingID == TransactionsSavingsID).First();

            if (T.SavingsSpendType == "UpdateValues")
            {
                if (T.isIncome)
                {
                    Budget.BankBalance += T.TransactionAmount;
                    Budget.MoneyAvailableBalance += T.TransactionAmount;
                    Budget.PayPeriodStats[0].IncomeToDate += T.TransactionAmount ?? 0;
                    Budget.PayPeriodStats[0].SavingsToDate += T.TransactionAmount ?? 0;
                    S.CurrentBalance += T.TransactionAmount;
                    S.LastUpdatedValue = T.TransactionAmount;
                    S.LastUpdatedDate = DateTime.UtcNow;
                }
                else
                {
                    Budget.BankBalance -= T.TransactionAmount;
                    Budget.MoneyAvailableBalance -= T.TransactionAmount;
                    S.CurrentBalance -= T.TransactionAmount;
                    S.LastUpdatedValue = T.TransactionAmount;
                    S.LastUpdatedDate = DateTime.UtcNow;
                    Budget.PayPeriodStats[0].SpendToDate += T.TransactionAmount ?? 0;
                }

                //Update Regular Saving Value
                RecalculateRegularSavingFromTransaction(ref S);
                
            }
            else if (T.SavingsSpendType == "MaintainValues")
            {
                if (T.isIncome)
                {
                    Budget.BankBalance += T.TransactionAmount;
                    Budget.MoneyAvailableBalance += T.TransactionAmount;
                    S.CurrentBalance += T.TransactionAmount;
                    S.SavingsGoal += T.TransactionAmount;
                    S.LastUpdatedValue = T.TransactionAmount;
                    S.LastUpdatedDate = DateTime.UtcNow;
                    Budget.PayPeriodStats[0].IncomeToDate += T.TransactionAmount ?? 0;
                    Budget.PayPeriodStats[0].SavingsToDate += T.TransactionAmount ?? 0;
                }
                else
                {
                    Budget.BankBalance -= T.TransactionAmount;
                    Budget.MoneyAvailableBalance -= T.TransactionAmount;
                    S.CurrentBalance -= T.TransactionAmount;
                    S.SavingsGoal -= T.TransactionAmount;
                    S.LastUpdatedValue = T.TransactionAmount;
                    S.LastUpdatedDate = DateTime.UtcNow;
                    Budget.PayPeriodStats[0].SpendToDate += T.TransactionAmount ?? 0;
                }
            }
            else if (T.SavingsSpendType == "BuildingSaving" | T.SavingsSpendType == "EnvelopeSaving")
            {
                if (T.isIncome)
                {
                    Budget.BankBalance += T.TransactionAmount;
                    Budget.MoneyAvailableBalance += T.TransactionAmount;
                    S.CurrentBalance += T.TransactionAmount;
                    S.LastUpdatedValue = T.TransactionAmount;
                    S.LastUpdatedDate = DateTime.UtcNow;
                    Budget.PayPeriodStats[0].IncomeToDate += T.TransactionAmount ?? 0;
                    Budget.PayPeriodStats[0].SavingsToDate += T.TransactionAmount ?? 0;
                }
                else
                {
                    Budget.BankBalance -= T.TransactionAmount;
                    Budget.MoneyAvailableBalance -= T.TransactionAmount;
                    S.CurrentBalance -= T.TransactionAmount;
                    S.LastUpdatedValue = T.TransactionAmount;
                    S.LastUpdatedDate = DateTime.UtcNow;
                    Budget.PayPeriodStats[0].SpendToDate += T.TransactionAmount ?? 0;
                }
            }
        }

        public void TransactTransactionDaily(ref Budgets Budget, int ID)
        {
            
            Transactions T = Budget.Transactions.Where(t => t.TransactionID == ID).First();

            if (T.isIncome)
            {
                Budget.BankBalance += T.TransactionAmount;
                Budget.MoneyAvailableBalance += T.TransactionAmount;
                Budget.LeftToSpendBalance += T.TransactionAmount;
                //Recalculate how much you have left to spen
                int DaysToPayDay = (Budget.NextIncomePayday.GetValueOrDefault().Date - DateTime.Today.Date).Days;
                Budget.LeftToSpendDailyAmount = (Budget.LeftToSpendBalance ?? 0) / DaysToPayDay;
                Budget.PayPeriodStats[0].IncomeToDate += T.TransactionAmount ?? 0;                    
            }
            else
            {
                Budget.BankBalance -= T.TransactionAmount;
                Budget.MoneyAvailableBalance -= T.TransactionAmount;
                Budget.LeftToSpendBalance -= T.TransactionAmount;
                Budget.LeftToSpendDailyAmount -= T.TransactionAmount ?? 0;
                Budget.PayPeriodStats[0].SpendToDate += T.TransactionAmount ?? 0;
            }

        }

        public void UpdateSavingsDaily(ref Budgets Budget)
        {
            foreach(Savings Saving in Budget.Savings)
            {
                if(Saving.isRegularSaving)
                {
                    if(Saving.SavingsType == "SavingsBuilder")
                    {
                        Saving.CurrentBalance = Saving.CurrentBalance + Saving.RegularSavingValue;
                        Saving.LastUpdatedValue = Saving.RegularSavingValue;
                        Saving.LastUpdatedDate = DateTime.Now.Date;
                        Budget.PayPeriodStats[0].SavingsToDate += Saving.RegularSavingValue ?? 0;
                    }
                    else if (Saving.SavingsType == "TargetAmount")
                    {
                        if(Saving.SavingsGoal > Saving.CurrentBalance)
                        {

                            decimal? Amount;
                            if(Saving.canExceedGoal)
                            {
                                Amount = Saving.RegularSavingValue;
                            }
                            else
                            {
                                Amount = (Saving.SavingsGoal - Saving.CurrentBalance) < Saving.RegularSavingValue ? (Saving.SavingsGoal - Saving.CurrentBalance) : Saving.RegularSavingValue;
                            }

                            Saving.CurrentBalance += Amount;
                            Saving.LastUpdatedValue = Amount;
                            Saving.LastUpdatedDate = DateTime.Now.Date;
                            Budget.PayPeriodStats[0].SavingsToDate += Saving.RegularSavingValue ?? 0;

                            if(Saving.CurrentBalance == Saving.SavingsGoal & Saving.isAutoComplete)
                            {
                                Saving.isSavingsClosed = true;
                            }
                        }
                    }
                    else if (Saving.SavingsType == "TargetDate")
                    {
                        decimal? Amount;
                        if (Saving.GoalDate.GetValueOrDefault().Date <= DateTime.Now.Date)
                        {
                            Amount = (Saving.SavingsGoal - Saving.CurrentBalance);
                            Saving.CurrentBalance += Amount;
                            Saving.LastUpdatedValue = Amount;
                            Saving.LastUpdatedDate = DateTime.Now.Date;
                            if (Saving.isAutoComplete)
                            {
                                Saving.isSavingsClosed = true;
                            }
                            Budget.PayPeriodStats[0].SavingsToDate += Saving.RegularSavingValue ?? 0;                            
                        }
                        else
                        {
                            Amount = Saving.RegularSavingValue;
                            Saving.CurrentBalance += Amount;
                            Saving.LastUpdatedValue = Amount;
                            Saving.LastUpdatedDate = DateTime.Now.Date;
                            Budget.PayPeriodStats[0].SavingsToDate += Saving.RegularSavingValue ?? 0;
                        }
                    }
                }
            }
        }
    }

}
