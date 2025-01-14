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
    public class FixedRateRebateIncentive : IIncentive
    {
        public IncentiveType Type => IncentiveType.FixedRateRebate;

        public IncentiveResult Process(IncentiveContext context)
        {
            if(!IsApplicable(context))
                return new IncentiveResult() { Success = false, RebateAmount = 0 };

            return new IncentiveResult() { Success = true, RebateAmount = CalculateRebate(context) };
        }
        
        public bool IsApplicable(IncentiveContext context)
        {
            if (context.Rebate == null)
                return false;

            if (context.Product == null)
                return false;

            if (!context.Product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate))
                return false;

            if (context.Rebate.Percentage == 0 || context.Product.Price == 0 || context.Request.Volume == 0)
                return false;

            return true;
        }

        public decimal CalculateRebate(IncentiveContext context)
        {
            return context.Product.Price * context.Rebate.Percentage * context.Request.Volume;
        }
    }
}
