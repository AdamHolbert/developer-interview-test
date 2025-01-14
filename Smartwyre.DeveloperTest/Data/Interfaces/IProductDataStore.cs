using Smartwyre.DeveloperTest.Types.Models;

namespace Smartwyre.DeveloperTest.Data.Interfaces
{
    public interface IProductDataStore
    {
        Product GetProduct(string productIdentifier);
    }
}