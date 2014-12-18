using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AWStruck.Savings
{
    public class SavingsHelper
    {
        public static SavingsResponse CalculateSavings()
        {
            return new SavingsResponse
            {
                FromDate = DateTime.Today.AddDays(-2),
                TotalSaved = 2323.21M,
                CurrentSavingsPerHour = 12.87M,
                SavedInLastDay = 211.08M,
                SavedInLastHour = 12.87M
            };
        }
    }
}