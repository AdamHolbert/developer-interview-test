using Smartwyre.DeveloperTest.Types.Calculators;
using Smartwyre.DeveloperTest.Types.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Factories
{
    public class IncentiveFactory : IIncentiveFactory
    {
        private readonly Dictionary<IncentiveType, IIncentive> _incentives;

        public IncentiveFactory(IEnumerable<IIncentive> incentives)
        {
            _incentives = incentives
                .GroupBy(i => i.Type)
                .ToDictionary(
                    g => g.Key,
                    g => g.Count() == 1
                        ? g.First()
                        : throw new InvalidOperationException($"Duplicate incentives found for: {g.Key}")
                );
        }

        public IIncentive Get(IncentiveType incentiveType)
        {
            if (!_incentives.TryGetValue(incentiveType, out var incentive))
            {
                throw new InvalidOperationException($"Unsupported incentive type: {incentiveType}");
            }

            return incentive;
        }
    }
}
