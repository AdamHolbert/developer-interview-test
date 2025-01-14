using Smartwyre.DeveloperTest.Types.DTOs;
using Smartwyre.DeveloperTest.Types.Enums;
using Smartwyre.DeveloperTest.Types.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Types.Calculators
{
    public class FixedCashIncentive : IIncentive
    {
        public IncentiveType Type => IncentiveType.FixedCashAmount;

        public IncentiveResult Process(IncentiveContext context)
        {
            var failedProcess = new IncentiveResult()
            {
                Success = false,
                RebateAmount = 0
            };

            if (context.Rebate == null)
                return failedProcess;

            if (context.Product == null)
                return failedProcess;

            if (!context.Product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount))
                return failedProcess;

            if (context.Rebate.Amount == 0)
                return failedProcess;

            return new IncentiveResult()
            {
                Success = true,
                RebateAmount = context.Rebate.Amount
            };
        }
    }
}
