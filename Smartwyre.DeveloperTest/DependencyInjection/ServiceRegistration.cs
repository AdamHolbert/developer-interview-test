using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Data.Implementations;
using Smartwyre.DeveloperTest.Data.Interfaces;
using Smartwyre.DeveloperTest.Factories;
using Smartwyre.DeveloperTest.Services.Implimentations;
using Smartwyre.DeveloperTest.Services.Interfaces;
using Smartwyre.DeveloperTest.Types.Calculators;
using System.Linq;

namespace Smartwyre.DeveloperTest.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddDataStores(this IServiceCollection services)
        {
            services.AddScoped<IProductDataStore, ProductDataStore>();
            services.AddScoped<IRebateDataStore, RebateDataStore>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IRebateService, RebateService>();
            return services;
        }

        public static IServiceCollection AddDomainLogic(this IServiceCollection services)
        {
            var incentiveTypes = typeof(IIncentive).Assembly.GetTypes()
                .Where(t => typeof(IIncentive).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

            foreach (var type in incentiveTypes)
            {
                services.AddTransient(typeof(IIncentive), type);
            }

            services.AddTransient<IIncentiveFactory, IncentiveFactory>();

            return services;
        }
    }
}
