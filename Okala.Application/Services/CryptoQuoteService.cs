using Okala.Application.Common;
using Okala.Application.Common.Interfaces.Services;
using ErrorOr;
using Okala.Domain.Common;
using Microsoft.Extensions.Logging;
using Okala.Application.Common.Config;
using Microsoft.Toolkit.Diagnostics;
using Okala.Application.Extensions;

namespace Okala.Application.Services;

public class CryptoQuoteService : ICryptoQuoteService
{
    private const int CACHE_QUOTE_RESULT_SECONDS = 10;

    private readonly IEnumerable<ICryptoExchangeProvider> _exchangeProviders;
    private readonly ICryptoQuoteCache _cryptoQuoteCache;
    private readonly EnabledProviderSetting _enabledProviderSetting;
    private readonly ILogger<CryptoQuoteService> _logger;

    public CryptoQuoteService(IEnumerable<ICryptoExchangeProvider> exchangeProviders, ICryptoQuoteCache cryptoQuoteCache, EnabledProviderSetting enabledProviderSetting, ILogger<CryptoQuoteService> logger)
    {
        if (exchangeProviders?.Count() == 0)
        {
            ThrowHelper.ThrowArgumentException("no provider is registered in order to check quotes");
        }

        _exchangeProviders = exchangeProviders!;
        _cryptoQuoteCache = cryptoQuoteCache;
        _enabledProviderSetting = enabledProviderSetting;
        _logger = logger;
    }

    public async Task<ErrorOr<QuoteResult>> Get(CryptoEnum cryptoSign, CancellationToken cancellationToken)
    {
        if (cryptoSign == CryptoEnum.None) return Errors.Exchange.InvalidCurrency;

        QuoteResult quoteResult = null;

        try
        {
            quoteResult = await _cryptoQuoteCache.Get(cryptoSign);
        }
        catch (System.Exception ex)
        {
            _logger.LogWarning(ex, "could not find result inside cache");
        }

        if (quoteResult is not null) return quoteResult;

        var provider = SelectProvider(cryptoSign);

        if (provider is null)
        {
            _logger.LogWarning("could not find and provider to check quote");
            return Errors.Exchange.NoValidProvidersFound;
        }

        try
        {
            quoteResult = await provider.GetQuotes(cryptoSign, cancellationToken);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "could not fetch quoteResult from provider: @provider", provider.Name);
            return Errors.Exchange.CouldNotHandleRequestTechnicalError;
        }

        try
        {
            await _cryptoQuoteCache.Set(cryptoSign, quoteResult, TimeSpan.FromSeconds(CACHE_QUOTE_RESULT_SECONDS));
        }
        catch (System.Exception ex)
        {
            _logger.LogWarning(ex, "could not set quoteResult inside cache");
        }

        return quoteResult;
    }

    public ICryptoExchangeProvider SelectProvider(CryptoEnum currentSign)
    {
        return _exchangeProviders?
            .Where(x => _enabledProviderSetting.Contains(x.Name))?
            .Where(x => x.SupportedSigns.Contains(currentSign))
            .Shuffle() //This can be done with a simple Random function to have a better O(1) complexity
            .FirstOrDefault()!;
    }
}