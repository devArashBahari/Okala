using Okala.Domain.Common;

namespace Okala.Application.Common.Interfaces.Services;

public interface ICryptoExchangeProvider
{
    //This should be loaded from appSettings
    public static List<string> SupportedCurrencies = new List<string>() { "USD", "EUR", "BRL", "GBP", "AUD" };
    public List<CryptoEnum> SupportedSigns { get; }
    
    string Name
    {
        get;
    }

    Task<QuoteResult> GetQuotes(CryptoEnum cryptoSign, CancellationToken cancellationToken);
}