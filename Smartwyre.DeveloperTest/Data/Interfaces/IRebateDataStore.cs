using Smartwyre.DeveloperTest.Types.Models;

namespace Smartwyre.DeveloperTest.Data.Interfaces
{
    public interface IRebateDataStore
    {
        Rebate GetRebate(string rebateIdentifier);
        void StoreCalculationResult(Rebate account, decimal rebateAmount);
    }
}