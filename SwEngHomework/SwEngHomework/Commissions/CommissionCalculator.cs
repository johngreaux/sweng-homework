using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SwEngHomework.Commissions.Models;

namespace SwEngHomework.Commissions
{
    public class CommissionCalculator : ICommissionCalculator
    {
        public IDictionary<string, double> CalculateCommissionsByAdvisor(string jsonInput)
        {
            // Format json input
            CommissionCalculatorModel input = JsonConvert.DeserializeObject<CommissionCalculatorModel>(jsonInput);
            IDictionary<string, string> advisors = input.advisors.ToDictionary(a => a.name, a => a.level);
            IDictionary<string, double> commisionsReport = input.advisors.ToDictionary(a => a.name, a => 0.00);

            // Loop through all accounts and generate commisions report
            foreach (var a in input.accounts)
            {
                if (advisors.ContainsKey(a.advisor))
                {
                    // Add commision to advisor
                    commisionsReport[a.advisor] += Math.Round(CalculateCommision(a.presentValue, GetBpsRate(a.presentValue), GetCommissionRate(advisors[a.advisor])), 2);
                }
            }

            return commisionsReport;
        }

        // Calculates the commision rate for an advisor based on vale, bps rate, and commission rate
        private double CalculateCommision(double value, double bpsRate, double commissionRate)
        {
            return (value * bpsRate) * commissionRate;
        }

        // Returns the bps rate based on the given 
        private double GetBpsRate(double presentValue)
        {
            if (presentValue > 0 && presentValue < 50000)
                return 0.0005;
            else if (presentValue >= 50000 && presentValue < 100000)
                return 0.0006;
            else if (presentValue >= 100000)
                return 0.0007;
            else
                throw new Exception("BPS rate cannot be calculated for the given value! Please review your input.");
        }

        // Returns the commision rate based on the given level of the advisor
        private double GetCommissionRate(string level)
        {
            if (level.Equals("Senior"))
                return 1.0;
            else if (level.Equals("Experienced"))
                return 0.5;
            else if (level.Equals("Junior"))
                return 0.25;
            else
                throw new Exception ("The give advisor level is not valid! Please review your input.");
        }
    }
}
