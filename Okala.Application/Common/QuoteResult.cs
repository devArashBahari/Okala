namespace Okala.Application.Common;

public record QuoteItem(string Currency, decimal Value);
public record QuoteResult(List<QuoteItem> Items, string Provider);

