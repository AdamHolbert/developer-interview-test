using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Data.Interfaces;
using Smartwyre.DeveloperTest.DependencyInjection;
using Smartwyre.DeveloperTest.Runner.Data;
using Smartwyre.DeveloperTest.Services.Interfaces;
using Smartwyre.DeveloperTest.Types.DTOs;
using System;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    {
        // Set up dependency injection
        var serviceProvider = ConfigureServices();

        // Resolve the RebateService
        var rebateService = serviceProvider.GetRequiredService<IRebateService>();

        // Display feature set
        Console.WriteLine("Welcome to the Rebate Service Demo!");
        Console.WriteLine("Available Products: Coke (Fixed Rate), Sandwich (Fixed Cash/Rate)");
        Console.WriteLine("Available Rebates: 5$OFF (Fixed Cash), 10%OFF (Fixed Rate), 25%OFF (Fixed Rate)");

        Console.Write("Enter Product Identifier: ");
        var productIdentifier = Console.ReadLine().Trim();

        Console.Write("Enter Rebate Identifier: ");
        var rebateIdentifier = Console.ReadLine().Trim();

        Console.Write("Enter Volume: ");
        if (!int.TryParse(Console.ReadLine().Trim(), out var volume) || volume <= 0)
        {
            Console.WriteLine("Invalid volume. Please enter a positive integer.");
            return;
        }

        var request = new CalculateRebateRequest
        {
            ProductIdentifier = productIdentifier,
            RebateIdentifier = rebateIdentifier,
            Volume = volume
        };

        try
        {
            var result = rebateService.Calculate(request);

            if (result.Success)
            {
                Console.WriteLine("Rebate calculation succeeded!");
            }
            else
            {
                Console.WriteLine("Rebate calculation failed. Check your inputs.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static ServiceProvider ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddSingleton<IProductDataStore, InMemoryProductDataStore>();
        services.AddSingleton<IRebateDataStore, InMemoryRebateDataStore>();

        services.AddServices();
        services.AddDomainLogic();

        
        return services.BuildServiceProvider();
    }
}
