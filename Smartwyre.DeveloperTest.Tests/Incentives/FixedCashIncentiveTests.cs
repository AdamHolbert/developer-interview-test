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
    public class FixedCashIncentiveTests
    {
        private readonly FixedCashIncentive _incentive = new FixedCashIncentive();

        [Theory]
        [InlineData(-1,   -1,  SupportedIncentiveType.FixedCashAmount, false, 0)]  // Rebate and Product are null
        [InlineData(-1,   100, SupportedIncentiveType.FixedCashAmount, false, 0)]  // Rebate is null
        [InlineData(10,   -1,  SupportedIncentiveType.FixedCashAmount, false, 0)]  // Product is null
        [InlineData(10,   100, SupportedIncentiveType.FixedRateRebate, false, 0)]  // Unsupported incentive type
        [InlineData(0,    100, SupportedIncentiveType.FixedCashAmount, false, 0)]  // Rebate amount is 0
        [InlineData(10,   0,   SupportedIncentiveType.FixedCashAmount, true,  10)] // Valid case (Product price is irrelevant here)
        [InlineData(10,   100, SupportedIncentiveType.FixedCashAmount, true,  10)] // Valid case
        public void Process_ShouldReturnExpectedResult(
            decimal rebateAmount,
            decimal productPrice,
            SupportedIncentiveType supportedIncentive,
            bool expectedSuccess,
            decimal expectedRebateAmount)
        {
            // Arrange
            var rebate = rebateAmount == -1 ? null : 
                new Rebate { Amount = rebateAmount };

            var product = productPrice == -1 ? null :
                new Product
                {
                    Price = productPrice,
                    SupportedIncentives = supportedIncentive
                };

            var context = new IncentiveContext
            {
                Request = new CalculateRebateRequest(),
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
