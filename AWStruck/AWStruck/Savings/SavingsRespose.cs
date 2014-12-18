using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AWStruck.Savings
{
    public class SavingsResponse
    {
        public DateTime FromDate { get; set; }
        public decimal TotalSaved { get; set; }
        public decimal CurrentSavingsPerHour { get; set; }
        public decimal SavedInLastDay { get; set; }
        public decimal SavedInLastHour { get; set; }
    }
}