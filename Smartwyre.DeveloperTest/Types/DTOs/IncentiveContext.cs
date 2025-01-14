using Smartwyre.DeveloperTest.Types.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Types.DTOs
{
    public class IncentiveContext
    {
        public CalculateRebateRequest Request { get; set; }
        public Rebate Rebate { get; set; }
        public Product Product { get; set; }
    }
}
