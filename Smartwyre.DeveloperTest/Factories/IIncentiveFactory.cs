using Smartwyre.DeveloperTest.Types.Calculators;
using Smartwyre.DeveloperTest.Types.Enums;

namespace Smartwyre.DeveloperTest.Factories
{
    public interface IIncentiveFactory
    {
        IIncentive Get(IncentiveType incentiveType);
    }
}