using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using AWStruck.AWS;

namespace AWStruck.Savings
{
    public class SavingsHelper
    {
        private const double T2MicroPerHour = 0.018;
        private const double T2SmallPerHour = 0.036;
        private const double T2MediumPerHour = 0.072;
        private const double DefaultPerHour = 0.05;

        private DateTime _fromDate
        {
            get
            {
                if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["SavingsStartDate"])) return DateTime.Now;
                try
                {
                    return DateTime.Parse(ConfigurationManager.AppSettings["SavingsStartDate"]);
                }
                catch
                {
                    return DateTime.Now;
                }
                return DateTime.Now;
            }
        }

        public SavingsResponse CalculateSavings()
        {
            return new SavingsResponse
            {
                FromDate = _fromDate,
                TotalSaved = 2323.21,
                CurrentSavingsPerHour = CalculateSavingsPerHour(),
                SavedInLastDay = 211.08,
                SavedInLastHour = 12.87
            };
        }

        private double CalculateSavingsPerHour()
        {
            var savedPerHour = 0.0;
            var envs = GetTurnedOffEnvironments();
            foreach (var environment in envs)
            {
                var numberOfInstances = environment.InstanceCount;
                var price = GetPricePerHourForInstanceType(environment.Type);
                var saved = numberOfInstances*price;
                savedPerHour = savedPerHour + saved;
            }
            return envs.Sum(environment => environment.InstanceCount*GetPricePerHourForInstanceType(environment.Type));
        }

        private IEnumerable<Environment> GetTurnedOffEnvironments()
        {
            var env1 = new Environment
            {
                InstanceIds = {"instance1", "instance2", "instance3"},
                Name = "Env1",
                State = "running",
                Type = "t2.micro"
            };

            var env2 = new Environment
            {
                InstanceIds = { "instance21", "instance22", "instance23", "instance24" },
                Name = "Env2",
                State = "stopped",
                Type = "t2.micro"
            };

            var envs = new List<Environment> {env1, env2};

            //var envs = Environments.GetEnvironments(Global.CreateAmazonClient());
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