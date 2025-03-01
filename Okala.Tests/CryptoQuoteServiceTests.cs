using Okala.Application.Common;
using Okala.Application.Common.Interfaces.Services;
using Okala.Application.Providers;
using Okala.Application.Services;
using Okala.Domain.Common;
using Microsoft.Extensions.Logging;

namespace Okala.Tests;

public class CryptoQuoteServiceTests
{
    private readonly Microsoft.Extensions.Logging.ILoggerFactory loggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create((options) => { });
    private readonly ICryptoQuoteCache mockOfCache = new MockOfCache();

    [Fact]
    public void GivenEmptyProviderList_ShouldThrowInvalidArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            CryptoQuoteService cryptoQuoteService = new CryptoQuoteService(new List<ICryptoExchangeProvider>(), mockOfCache, null, null);
        });
    }

    [Fact]
    public async void GivenInvalidCryptoSign_ShouldReturnInvalidCryptoSign()
    {
        //Setup
        CryptoQuoteService cryptoQuoteService = new CryptoQuoteService(new List<ICryptoExchangeProvider>() { new CoinMarketCapProvider(null), new ExchangeRatesApiProvider(null) }, mockOfCache, null, null);

        //Action
        var result = await cryptoQuoteService.Get(Domain.Common.CryptoEnum.None, CancellationToken.None);

        //Assertion
        Assert.True(result.IsError);
        Assert.True(result.FirstError == Domain.Common.Errors.Exchange.InvalidCurrency);
    }

    [Fact]
    public void GivenSpecificSignThatIsSupportedByOnlyOneProvider_SelectValidProvider()
    {
        //Setup
        CryptoQuoteService cryptoQuoteService = new CryptoQuoteService(new List<ICryptoExchangeProvider>() {
            new CoinMarketCapProvider(null), new ExchangeRatesApiProvider(null) },
            mockOfCache,
            new Application.Common.Config.EnabledProviderSetting()
            {
                nameof(CoinMarketCapProvider),
                nameof(ExchangeRatesApiProvider)
            },
            CreateMockLogger<CryptoQuoteService>());

        //Action
        var result = cryptoQuoteService.SelectProvider(CryptoEnum.ETH);

        //Assertion
        Assert.Equal(typeof(CoinMarketCapProvider), result.GetType());
    }


    [Fact]
    public void EnableOnlyOneProvider_SelectionShouldAlwaysResultOneByBitocin()
    {
        //Setup
        CryptoQuoteService cryptoQuoteService = new CryptoQuoteService(new List<ICryptoExchangeProvider>() {
            new CoinMarketCapProvider(null), new ExchangeRatesApiProvider(null) },
            mockOfCache,
            new Application.Common.Config.EnabledProviderSetting()
            {
                nameof(ExchangeRatesApiProvider)
            },
            CreateMockLogger<CryptoQuoteService>());

        //Action
        var result = cryptoQuoteService.SelectProvider(CryptoEnum.BTC);

        //Assertion
        Assert.Equal(typeof(ExchangeRatesApiProvider), result.GetType());
    }

    [Fact]
    public async Task EnableOnlyOneProviderThatDoesNotSupportEth_ShouldReturnNoProviderFound()
    {
        //Setup
        CryptoQuoteService cryptoQuoteService = new CryptoQuoteService(new List<ICryptoExchangeProvider>() {
            new CoinMarketCapProvider(null), new ExchangeRatesApiProvider(null) },
            mockOfCache,
            new Application.Common.Config.EnabledProviderSetting()
            {
                nameof(ExchangeRatesApiProvider)
            },
            CreateMockLogger<CryptoQuoteService>());

        //Action
        var result = await cryptoQuoteService.Get(CryptoEnum.ETH, CancellationToken.None);
        //Assertion
        Assert.True(result.IsError);
        Assert.Equal(Errors.Exchange.NoValidProvidersFound, result.FirstError);
    }

    #region Mocks
    private ILogger<T> CreateMockLogger<T>()
    {
        return loggerFactory.CreateLogger<T>();
    }

    //I could implement mocks using Moq. But I dare not because of time limitations.
    private class MockOfCache : ICryptoQuoteCache
    {
        public async Task<QuoteResult> Get(CryptoEnum cryptoSign)
        {
            return await Task.FromResult<QuoteResult>(null);
        }

        public async Task Set(CryptoEnum cryptoSign, QuoteResult quoteResult, TimeSpan expiration)
        {
            await Task.CompletedTask;
        }
    }
    #endregion
}