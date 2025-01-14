using Smartwyre.DeveloperTest.Types.Calculators;
using Smartwyre.DeveloperTest.Types.DTOs;
using Smartwyre.DeveloperTest.Types.Enums;
using Smartwyre.DeveloperTest.Types.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Incentives
{
    public class FixedRateRebateIncentiveTests
    {
        private readonly FixedRateRebateIncentive _incentive = new FixedRateRebateIncentive();

        [Theory]
        [InlineData(-1,   -1,   SupportedIncentiveType.FixedRateRebate, 0.1, 10, false, 0)]   // Rebate and Product are null
        [InlineData(10,   -1,   SupportedIncentiveType.FixedRateRebate, 0.1, 10, false, 0)]   // Product is null
        [InlineData(-1,   100,  SupportedIncentiveType.FixedRateRebate, 0.1, 10, false, 0)]   // Rebate is null
        [InlineData(10,   100,  SupportedIncentiveType.FixedCashAmount, 0.1, 10, false, 0)]   // Unsupported incentive type
        [InlineData(10,   100,  SupportedIncentiveType.FixedRateRebate, 0.1, 0,  false, 0)]   // Volume is 0
        [InlineData(10,   0,    SupportedIncentiveType.FixedRateRebate, 0.1, 10, false, 0)]   // Product price is 0
        [InlineData(10,   100,  SupportedIncentiveType.FixedRateRebate, 0,   10, false, 0)]   // Rebate percentage is 0
        [InlineData(10,   100,  SupportedIncentiveType.FixedRateRebate, 0.1, 10, true,  100)] // Valid case
        public void Process_ShouldReturnExpectedResult(
            decimal rebateAmount,
            decimal productPrice,
            SupportedIncentiveType supportedIncentive,
            decimal rebatePercentage,
            decimal volume,
            bool expectedSuccess,
            decimal expectedRebateAmount)
        {
            // Arrange
            var rebate = rebateAmount == -1 ? null :
                new Rebate { Percentage = rebatePercentage, Amount = rebateAmount };

            var product = productPrice == -1 ? null :
                new Product
                {
                    Price = productPrice,
                    SupportedIncentives = supportedIncentive
                };

            var context = new IncentiveContext
            {
                Request = new CalculateRebateRequest { Volume = volume },
                Rebate = rebate,
                Product = product
            };

            // Act
            var result = _incentive.Process(context);

            // Assert
            Assert.Equal(expectedSuccess, result.Success);
            Assert.Equal(expectedRebateAmount, result.RebateAmount);
        }
    }
}
