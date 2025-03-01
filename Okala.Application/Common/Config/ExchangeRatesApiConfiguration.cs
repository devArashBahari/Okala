namespace Okala.Application.Common.Config;

public record ExchangeRatesApiConfiguration(string Key = "", string Address = "", int Timeout = 30);
