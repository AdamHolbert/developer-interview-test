using Smartwyre.DeveloperTest.Data.Interfaces;
using Smartwyre.DeveloperTest.Types.Enums;
using Smartwyre.DeveloperTest.Types.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartwyre.DeveloperTest.Runner.Data
{
    public class InMemoryProductDataStore : IProductDataStore
    {
        public readonly Dictionary<string, Product> _products = new()
        {
            {
                "Coke",
                new Product {
                    Id = 1,
                    Identifier = "Coke",
                    Uom = "Each",
                    Price = 3,
                    SupportedIncentives = SupportedIncentiveType.FixedRateRebate,
                }
            },
            {
                "Sandwich",
                new Product {
                    Id = 2,
                    Identifier = "Sandwich",
                    Uom = "Each",
                    Price = 10,
                    SupportedIncentives = SupportedIncentiveType.FixedCashAmount | SupportedIncentiveType.FixedRateRebate
                }
            }
        };

        public Product GetProduct(string productIdentifier)
        {
            return _products.GetValueOrDefault(productIdentifier);
        }
    }
}
