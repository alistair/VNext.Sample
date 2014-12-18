using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AWStruck.AWS;

namespace AWStruck.Savings
{
    public class SavingsHelper
    {
        private const double T2MicroPerHour = 0.018;
        private const double T2SmallPerHour = 0.036;
        private const double T2MediumPerHour = 0.072;
        private const double DefaultPerHour = 0.05;

        //private DateTime _fromDate
        //{
        //    get
        //    {
        //        if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["SavingsStartDate"])) return DateTime.Now;
        //        try
        //        {
        //            return DateTime.Parse(ConfigurationManager.AppSettings["SavingsStartDate"]);
        //        }
        //        catch
        //        {
        //            return DateTime.Now;
        //        }
        //    }
        //}

        public SavingsResponse CalculateSavings()
        {
            return new SavingsResponse
            {
                CurrentSavingsPerHour = CalculateSavingsPerHour()
                //FromDate = _fromDate,
                //TotalSaved = 2323.21,
                //SavedInLastDay = 211.08,
                //SavedInLastHour = 12.87
            };
        }

        private double CalculateSavingsPerHour()
        {
            var envs = GetTurnedOffEnvironments();
            return envs.Sum(environment => environment.InstanceCount*GetPricePerHourForInstanceType(environment.Type));
        }

        private IEnumerable<Environment> GetTurnedOffEnvironments()
        {
            var envs = Environments.GetEnvironments(Global.CreateAmazonClient());
            return envs.Where(e => e.State != "running").ToList();
        }

        private double GetPricePerHourForInstanceType(string instanceType)
        {
            switch (instanceType)
            {
                case "t2.micro":
                    return T2MicroPerHour;
                case "t2.small":
                    return T2SmallPerHour;
                case "t2.medium":
                    return T2MediumPerHour;
                default:
                    return DefaultPerHour;
            }
        }
    }
}