using Smartwyre.DeveloperTest.Types.DTOs;

namespace Smartwyre.DeveloperTest.Services.Interfaces;

public interface IRebateService
{
    CalculateRebateResult Calculate(CalculateRebateRequest request);
}
