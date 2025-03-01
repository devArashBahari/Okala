using Okala.Application.Common;
using Okala.Application.Common.Interfaces.Services;
using Okala.Domain.Common;
using Newtonsoft.Json;

namespace Okala.Application.Providers;

public class ExchangeRatesApiProvider : ICryptoExchangeProvider
{
    #region props
    private readonly IHttpClientFactory _httpClientFactory;
    public string Name => nameof(ExchangeRatesApiProvider);
    public List<CryptoEnum> SupportedSigns => new() { CryptoEnum.BTC };
    #endregion

    public ExchangeRatesApiProvider(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<QuoteResult> GetQuotes(CryptoEnum cryptoSign, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        var client = _httpClientFactory.CreateClient(nameof(ExchangeRatesApiProvider));

        var result = await client.GetAsync($"/currency_data/live?source={cryptoSign}&currencies={string.Join(",", ICryptoExchangeProvider.SupportedCurrencies)}", cancellationToken);
        var stringResponse = await result.Content.ReadAsStringAsync();

        var exchangeRateLiveResponse = JsonConvert.DeserializeObject<ExchangeRateLiveResposne>(stringResponse);

        List<QuoteItem> quoteItems = new List<QuoteItem>();

        if (exchangeRateLiveResponse?.Success is true)
        {
            foreach (var kvp in exchangeRateLiveResponse.Quotes)
            {
                string key = kvp.Key.Replace(cryptoSign.ToString(), "");
                decimal value = kvp.Value;

                quoteItems.Add(new QuoteItem(key, value));
            }
        }

        return new QuoteResult(quoteItems, Name);
    }

    private record ExchangeRateLiveResposne(Dictionary<string, decimal> Quotes, bool Success);
}