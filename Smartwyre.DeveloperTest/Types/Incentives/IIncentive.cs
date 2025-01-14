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
    public interface IIncentive
    {
        IncentiveType Type { get; }
        IncentiveResult Process(IncentiveContext context);
    }
}
