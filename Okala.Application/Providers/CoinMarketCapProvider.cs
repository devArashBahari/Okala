using Okala.Application.Common;
using Okala.Application.Common.Interfaces.Services;
using Okala.Domain.Common;
using Newtonsoft.Json;

namespace Okala.Application.Providers;

public class CoinMarketCapProvider : ICryptoExchangeProvider
{
    #region props
    private readonly IHttpClientFactory _httpClientFactory;
    public string Name => nameof(CoinMarketCapProvider);
    public List<CryptoEnum> SupportedSigns => new() { CryptoEnum.BTC, CryptoEnum.ETH, CryptoEnum.XRP, CryptoEnum.ADA, CryptoEnum.SOL, CryptoEnum.BCH };
    #endregion

    public CoinMarketCapProvider(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<QuoteResult> GetQuotes(CryptoEnum cryptoSign, CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient(nameof(CoinMarketCapProvider));

        List<QuoteItem> quoteItems = new List<QuoteItem>();

        //It might be better if we sent the requests concurrently to the server (based on the rate limit)
        foreach (var supportedCurrency in ICryptoExchangeProvider.SupportedCurrencies)
        {
            var result = await client.GetAsync($"/v2/cryptocurrency/quotes/latest?symbol={cryptoSign}&convert={supportedCurrency}", cancellationToken);
            var stringResponse = await result.Content.ReadAsStringAsync();

            var coinMarketCapQuoteResponse = JsonConvert.DeserializeObject<CoinMarketCapResponse>(stringResponse);
            var price = ExtractPriceFromExchangeRateResponseOrDefault(cryptoSign, supportedCurrency, coinMarketCapQuoteResponse!);

            quoteItems.Add(new QuoteItem(supportedCurrency, price));
        }

        return new(quoteItems, Name);
    }

    private static decimal ExtractPriceFromExchangeRateResponseOrDefault(CryptoEnum cryptoSign, string supportedCurrency, CoinMarketCapResponse coinMarketCapQuoteResponse)
    {
        var specificSignResponse = coinMarketCapQuoteResponse
                                ?.Data?.Where(x => x.Key.ToLower() == cryptoSign.ToString().ToLower())?.FirstOrDefault().Value?.FirstOrDefault();
        var specificCurrenyQuote = specificSignResponse?.Quote.Where(x => x.Key.ToLower() == supportedCurrency.ToLower())?.FirstOrDefault().Value;

        return specificCurrenyQuote?.Price ?? 0;
    }

    #region DTO Models
    private record CoinMarketCapResponseStatus
    {
        [JsonProperty("error_code")]
        public int ErrorCode { set; get; }

        [JsonProperty("error_message")]
        public string ErrorMessage { set; get; }
    }

    private class CoinMarketCapResponseDataItem
    {
        public int Id { set; get; }

        public string Symbol { set; get; }

        public Dictionary<string, CoinMarketCapQuoteItem> Quote { set; get; }
    }

    private record CoinMarketCapQuoteItem(decimal Price);
    private record CoinMarketCapResponse(CoinMarketCapResponseStatus Status, Dictionary<string, List<CoinMarketCapResponseDataItem>> Data);
    #endregion
}