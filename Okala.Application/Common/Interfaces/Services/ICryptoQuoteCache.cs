using Okala.Domain.Common;

namespace Okala.Application.Common.Interfaces.Services;

public interface ICryptoQuoteCache
{
    Task<QuoteResult> Get(CryptoEnum cryptoSign);
    Task Set(CryptoEnum cryptoSign, QuoteResult quoteResult, TimeSpan expiration);


}