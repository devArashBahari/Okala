using Okala.Application.Common.Interfaces.Services;
using Okala.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Okala.Api.Controllers;

[Route("api/[controller]")]
//I used exchangeController because in this simple problem, I assumed that the exchange is AggregateRoot.
public class ExchangeController : ApiController
{
    private readonly ILogger<ExchangeController> _logger;
    private readonly ICryptoQuoteService _cryptoQuoteService;

    public ExchangeController(ILogger<ExchangeController> logger, ICryptoQuoteService cryptoQuoteService)
    {
        _logger = logger;
        _cryptoQuoteService = cryptoQuoteService;
    }

    /// <summary>
    /// Crypto quote API
    /// </summary>
    /// <returns>A list of quotes for a specific Crypto sign</returns>
    /// <remarks>
    /// Sample request:
    ///     GET /api/exchange?sign=BTC
    /// </remarks>
    /// <response code="400">If the sign is invalid</response>
    /// <response code="500">If technical issue</response>
    /// <response code="200">If success</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet("quotes")]
    public async Task<IActionResult> Get([FromQuery] CryptoEnum sign, [FromServices] ICryptoQuoteService cryptoQuoteService, CancellationToken cancellationToken)
    {
        var result = await cryptoQuoteService.Get(sign, cancellationToken);

        if (result.IsError is false)
        {
            return Ok(result.Value);
        }

        return Problem(result.FirstError);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("supported-currencies")]
    public IActionResult SupportedCurrencies()
    {
        return Ok(ICryptoExchangeProvider.SupportedCurrencies);
    }
}