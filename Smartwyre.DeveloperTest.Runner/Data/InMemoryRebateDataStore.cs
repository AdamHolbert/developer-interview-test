using Smartwyre.DeveloperTest.Data.Interfaces;
using Smartwyre.DeveloperTest.Types.Enums;
using Smartwyre.DeveloperTest.Types.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Runner.Data
{
    public class InMemoryRebateDataStore : IRebateDataStore
    {
        public readonly Dictionary<string, Rebate> _rebates = new()
        {
            {
                "5$OFF",
                new Rebate {
                    Identifier = "5$OFF",
                    Amount = 5,
                    Incentive = IncentiveType.FixedCashAmount
                }
            },
            {
                "10%OFF",
                new Rebate {
                    Identifier = "10%OFF",
                    Amount = 0.25m,
                    Incentive = IncentiveType.FixedRateRebate
                }
            },
            {
                "25%OFF",
                new Rebate {
                    Identifier = "25%OFF",
                    Amount = 0.25m,
                    Incentive = IncentiveType.FixedRateRebate
                }
            },
        };

        public Rebate GetRebate(string rebateIdentifier)
        {
            return _rebates.GetValueOrDefault(rebateIdentifier);
        }

        public void StoreCalculationResult(Rebate account, decimal rebateAmount)
        {
            Console.WriteLine($"Stored Rebate Calculation: {account.Incentive}, Amount: {rebateAmount}");
        }
    }
}
