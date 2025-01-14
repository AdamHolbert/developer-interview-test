using Moq;
using Smartwyre.DeveloperTest.Data.Interfaces;
using Smartwyre.DeveloperTest.Factories;
using Smartwyre.DeveloperTest.Services.Implimentations;
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

namespace Smartwyre.DeveloperTest.Tests.Services
{
    public class RebateServiceTests
    {
        private readonly Mock<IRebateDataStore> _mockRebateDataStore = new Mock<IRebateDataStore>();
        private readonly Mock<IProductDataStore> _mockProductDataStore = new Mock<IProductDataStore>();
        private readonly Mock<IIncentiveFactory> _mockIncentiveFactory = new Mock<IIncentiveFactory>();
        private readonly RebateService _rebateService;

        public RebateServiceTests()
        {
            _rebateService = new RebateService(
                _mockRebateDataStore.Object,
                _mockProductDataStore.Object,
                _mockIncentiveFactory.Object);
        }

        [Fact]
        public void Calculate_ShouldReturnSuccess_WhenAllInputsAreValid()
        {
            // Arrange
            var request = new CalculateRebateRequest
            {
                ProductIdentifier = "Product1",
                RebateIdentifier = "Rebate1"
            };

            var product = new Product
            {
                Identifier = "Product1",
                Price = 100,
                SupportedIncentives = SupportedIncentiveType.FixedCashAmount
            };

            var rebate = new Rebate
            {
                Incentive = IncentiveType.FixedCashAmount,
                Amount = 10
            };

            var incentive = new Mock<IIncentive>();
            incentive.Setup(i => i.Process(It.IsAny<IncentiveContext>()))
                .Returns(new IncentiveResult { Success = true, RebateAmount = 10 });

            _mockProductDataStore.Setup(store => store.GetProduct(request.ProductIdentifier))
                .Returns(product);

            _mockRebateDataStore.Setup(store => store.GetRebate(request.RebateIdentifier))
                .Returns(rebate);

            _mockIncentiveFactory.Setup(factory => factory.Get(rebate.Incentive))
                .Returns(incentive.Object);

            // Act
            var result = _rebateService.Calculate(request);

            // Assert
            Assert.True(result.Success);
            _mockRebateDataStore.Verify(store => store.StoreCalculationResult(rebate, 10), Times.Once);
        }

        [Fact]
        public void Calculate_ShouldReturnFailure_WhenRebateNotFound()
        {
            // Arrange
            var request = new CalculateRebateRequest
            {
                ProductIdentifier = "Product1",
                RebateIdentifier = "Rebate1"
            };

            _mockRebateDataStore.Setup(store => store.GetRebate(request.RebateIdentifier))
                .Returns((Rebate)null);

            // Act
            var result = _rebateService.Calculate(request);

            // Assert
            Assert.False(result.Success);
            _mockRebateDataStore.Verify(store => store.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never);
        }

        [Fact]
        public void Calculate_ShouldReturnFailure_WhenProductNotFound()
        {
            // Arrange
            var request = new CalculateRebateRequest
            {
                ProductIdentifier = "Product1",
                RebateIdentifier = "Rebate1"
            };

            _mockProductDataStore.Setup(store => store.GetProduct(request.ProductIdentifier))
                .Returns((Product)null);

            // Act
            var result = _rebateService.Calculate(request);

            // Assert
            Assert.False(result.Success);
            _mockRebateDataStore.Verify(store => store.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never);
        }

        [Fact]
        public void Calculate_ShouldReturnFailure_WhenIncentiveNotApplicable()
        {
            // Arrange
            var request = new CalculateRebateRequest
            {
                ProductIdentifier = "Product1",
                RebateIdentifier = "Rebate1"
            };

            var product = new Product
            {
                Identifier = "Product1",
                Price = 100,
                SupportedIncentives = SupportedIncentiveType.FixedCashAmount
            };

            var rebate = new Rebate
            {
                Incentive = IncentiveType.FixedCashAmount,
                Amount = 10
            };

            var incentive = new Mock<IIncentive>();
            incentive.Setup(i => i.Process(It.IsAny<IncentiveContext>()))
                .Returns(new IncentiveResult { Success = false });

            _mockProductDataStore.Setup(store => store.GetProduct(request.ProductIdentifier))
                .Returns(product);

            _mockRebateDataStore.Setup(store => store.GetRebate(request.RebateIdentifier))
                .Returns(rebate);

            _mockIncentiveFactory.Setup(factory => factory.Get(rebate.Incentive))
                .Returns(incentive.Object);

            // Act
            var result = _rebateService.Calculate(request);

            // Assert
            Assert.False(result.Success);
            _mockRebateDataStore.Verify(store => store.StoreCalculationResult(It.IsAny<Rebate>(), It.IsAny<decimal>()), Times.Never);
        }

        [Fact]
        public void Calculate_ShouldThrowException_WhenIncentiveTypeIsUnsupported()
        {
            // Arrange
            var request = new CalculateRebateRequest
            {
                ProductIdentifier = "Product1",
                RebateIdentifier = "Rebate1"
            };

            var product = new Product
            {
                Identifier = "Product1",
                Price = 100,
                SupportedIncentives = SupportedIncentiveType.FixedCashAmount
            };

            var rebate = new Rebate
            {
                Incentive = IncentiveType.FixedCashAmount,
                Amount = 10
            };

            _mockProductDataStore.Setup(store => store.GetProduct(request.ProductIdentifier))
                .Returns(product);

            _mockRebateDataStore.Setup(store => store.GetRebate(request.RebateIdentifier))
                .Returns(rebate);

            _mockIncentiveFactory.Setup(factory => factory.Get(rebate.Incentive))
                .Throws(new InvalidOperationException("Unsupported incentive type."));

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _rebateService.Calculate(request));
            Assert.Contains("Unsupported incentive type", exception.Message);
        }
    }

}
