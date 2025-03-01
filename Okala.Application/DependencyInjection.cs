using System.Reflection;
using Okala.Application.Common.Config;
using Okala.Application.Common.Interfaces.Services;
using Okala.Application.Providers;
using Okala.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Okala.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICryptoQuoteService, CryptoQuoteService>();
        return services;
    }

    public static IServiceCollection AddProviders(this IServiceCollection services, IConfiguration configuration)
    {
        EnabledProviderSetting enabledProviderSetting = new();
        configuration.Bind(nameof(EnabledProviderSetting), enabledProviderSetting);
        services.AddSingleton(enabledProviderSetting);

        ExchangeRatesApiConfiguration exchangeRatesApiConfig = new();
        configuration.Bind(nameof(ExchangeRatesApiConfiguration), exchangeRatesApiConfig);

        CoinMarketCapApiConfiguration coinMarketCapApiConfiguration = new();
        configuration.Bind(nameof(CoinMarketCapApiConfiguration), coinMarketCapApiConfiguration);

        //TODO: Providers could be instanciated dynamically at runtime (Per enabled provider)

        // var providers = Assembly.GetExecutingAssembly().GetTypes()
        //     .Where(p => p.IsAssignableTo(typeof(ICryptoExchangeProvider)) && p.IsClass)
        //     .ToArray()
        //     .OfType<ICryptoExchangeProvider>()
        //     .Where(p => enabledProviderSetting.Contains(p.Name));


        //I register providers and their clients statically

        services.AddHttpClient(nameof(CoinMarketCapProvider), (client) =>
        {
            client.BaseAddress = new Uri(coinMarketCapApiConfiguration.Address);
            client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", coinMarketCapApiConfiguration.Key);
        });

        services.AddHttpClient(nameof(ExchangeRatesApiProvider), (client) =>
        {
            client.BaseAddress = new Uri(exchangeRatesApiConfig.Address);
            client.DefaultRequestHeaders.Add("apiKey", exchangeRatesApiConfig.Key);
            client.Timeout = TimeSpan.FromSeconds(exchangeRatesApiConfig.Timeout);
        });

        services.AddTransient<ICryptoExchangeProvider, CoinMarketCapProvider>();
        //services.AddTransient<ICryptoExchangeProvider, ExchangeRatesApiProvider>();

        return services;
    }
}