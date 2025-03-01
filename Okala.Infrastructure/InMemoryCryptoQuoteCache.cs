using Okala.Application.Common;
using Okala.Application.Common.Interfaces.Services;
using Okala.Domain.Common;
using Microsoft.Extensions.Caching.Memory;

namespace Okala.Infrastructure;

//The name should be something more general because here is the infrastructure layer
public class InMemoryCryptoQuoteCache : ICryptoQuoteCache
{
    private readonly IMemoryCache _storage;

    public InMemoryCryptoQuoteCache(IMemoryCache memory)
    {
        _storage = memory;
    }

    public async Task<QuoteResult> Get(CryptoEnum cryptoSign)
    {
        await Task.CompletedTask;

        QuoteResult result;
        _storage.TryGetValue(cryptoSign, out result!);

        return result;
    }

    public async Task Set(CryptoEnum cryptoSign, QuoteResult quoteResult, TimeSpan expiration)
    {
        await Task.CompletedTask;
        _storage.Set(cryptoSign, quoteResult, expiration);
    }
}