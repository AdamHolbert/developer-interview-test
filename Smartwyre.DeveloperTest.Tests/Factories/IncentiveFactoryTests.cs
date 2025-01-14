using Smartwyre.DeveloperTest.Factories;
using Smartwyre.DeveloperTest.Types.Calculators;
using Smartwyre.DeveloperTest.Types.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.Factories
{
    public class IncentiveFactoryTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithValidIncentives()
        {
            // Arrange
            var incentives = new List<IIncentive>
            {
                new FixedCashIncentive(),
                new FixedRateRebateIncentive()
            };

            // Act
            var factory = new IncentiveFactory(incentives);

            // Assert
            Assert.NotNull(factory);
        }

        [Fact]
        public void Constructor_ShouldThrowExceptionForDuplicateIncentives()
        {
            // Arrange
            var incentives = new List<IIncentive>
            {
                new FixedCashIncentive(),
                new FixedCashIncentive()
            };

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => new IncentiveFactory(incentives));
            Assert.Contains("Duplicate incentives found for", exception.Message);
        }

        [Fact]
        public void Get_ShouldReturnCorrectIncentive()
        {
            // Arrange
            var fixedCashIncentive = new FixedCashIncentive();
            var fixedRateRebateIncentive = new FixedRateRebateIncentive();

            var incentives = new List<IIncentive> { fixedCashIncentive, fixedRateRebateIncentive };
            var factory = new IncentiveFactory(incentives);

            // Act
            var result = factory.Get(IncentiveType.FixedCashAmount);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<FixedCashIncentive>(result);
        }

        [Fact]
        public void Get_ShouldThrowExceptionForUnsupportedIncentiveType()
        {
            // Arrange
            var incentives = new List<IIncentive>
            {
                new FixedCashIncentive()
            };

            var factory = new IncentiveFactory(incentives);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => factory.Get(IncentiveType.AmountPerUom));
            Assert.Contains("Unsupported incentive type", exception.Message);
        }
    }
}
