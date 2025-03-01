using ErrorOr;
using Okala.Domain.Common;

namespace Okala.Application.Common.Interfaces.Services;

public interface ICryptoQuoteService
{
    Task<ErrorOr<QuoteResult>> Get(CryptoEnum cryptoSign, CancellationToken cancellationToken);
}