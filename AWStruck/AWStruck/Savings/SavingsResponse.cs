using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AWStruck.Savings
{
    public class SavingsResponse
    {
        public DateTime FromDate { get; set; }
        public double TotalSaved { get; set; }
        public double CurrentSavingsPerHour { get; set; }
        public double SavedInLastDay { get; set; }
        public double SavedInLastHour { get; set; }
    }
}