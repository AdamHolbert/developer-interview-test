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
    public class AmountPerUomIncentiveTests
    {
        private readonly AmountPerUomIncentive _incentive = new AmountPerUomIncentive();

        [Theory]
        [InlineData(-1,   -1,   SupportedIncentiveType.AmountPerUom,    10, false, 0)]   // Rebate and Product are null
        [InlineData(-1,   100,  SupportedIncentiveType.AmountPerUom,    10, false, 0)]   // Rebate is null
        [InlineData(10,   -1,   SupportedIncentiveType.AmountPerUom,    10, false, 0)]   // Product is null
        [InlineData(10,   100,  SupportedIncentiveType.FixedCashAmount, 10, false, 0)]   // Unsupported incentive type
        [InlineData(0,    100,  SupportedIncentiveType.AmountPerUom,    10, false, 0)]   // Rebate amount is 0
        [InlineData(10,   100,  SupportedIncentiveType.AmountPerUom,    0,  false, 0)]   // Request volume is 0
        [InlineData(10,   0,    SupportedIncentiveType.AmountPerUom,    10, true,  100)] // Valid case
        public void Process_ShouldReturnExpectedResult(
            decimal rebateAmount,
            decimal productPrice,
            SupportedIncentiveType supportedIncentive,
            decimal requestVolume,
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

            var request = new CalculateRebateRequest
            {
                Volume = requestVolume
            };

            var context = new IncentiveContext
            {
                Request = request,
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
