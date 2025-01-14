using Smartwyre.DeveloperTest.Data.Interfaces;
using Smartwyre.DeveloperTest.Factories;
using Smartwyre.DeveloperTest.Services.Interfaces;
using Smartwyre.DeveloperTest.Types.DTOs;
using Smartwyre.DeveloperTest.Types.Models;

namespace Smartwyre.DeveloperTest.Services.Implimentations;

public class RebateService : IRebateService
{
    private readonly IRebateDataStore _rebateDataStore;
    private readonly IProductDataStore _productDataStore;
    private readonly IIncentiveFactory _incentiveFactory;

    public RebateService(IRebateDataStore rebateDataStore, IProductDataStore productDataStore, IIncentiveFactory incentiveFactory)
    {
        _rebateDataStore = rebateDataStore;
        _productDataStore = productDataStore;
        _incentiveFactory = incentiveFactory;
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        Product product = _productDataStore.GetProduct(request.ProductIdentifier);
        Rebate rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
        if (product == null || rebate == null)
            return new CalculateRebateResult() { Success = false};
        
        var context = new IncentiveContext
        {
            Request = request,
            Rebate = rebate,
            Product = product
        };

        var incentive = _incentiveFactory.Get(rebate.Incentive);
        var result = incentive.Process(context);
        if (result.Success)
        {
            _rebateDataStore.StoreCalculationResult(rebate, result.RebateAmount);
        }

        return new CalculateRebateResult()
        {
            Success = result.Success,
        };
    }
}
