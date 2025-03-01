using Okala.Application.Common.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Okala.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        //It can be handled by redis or other distributed caches
        services.AddMemoryCache();
        services.AddSingleton<ICryptoQuoteCache, InMemoryCryptoQuoteCache>();
        
        return services;
    }
}