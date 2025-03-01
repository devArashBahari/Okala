namespace Okala.Domain.Common;

public static partial class Errors
{

    public static class Exchange
    {
        public static ErrorOr.Error InvalidCurrency = ErrorOr.Error.Validation("Exchange.CurrencyError", "currency should not be empty or invalid");
        public static ErrorOr.Error NoValidProvidersFound = ErrorOr.Error.Validation("Exchange.InvalidProvider", "No valid exchange provider found");
        public static ErrorOr.Error CouldNotHandleRequestTechnicalError = ErrorOr.Error.Unexpected("Exchange.TechnicalError", "Could not fetch data from provider");
    }
}