namespace Bookify.Domain.Shared;

public record Currency
{
    internal static readonly Currency None = new("");
    public static readonly Currency USD = new("USD");
    public static readonly Currency EUR = new("EUR");
    public static readonly Currency CFA = new("CFA");
    public Currency(string code) => Code = code;
    public string Code { get; init; }

    public static Currency FromCode(string code)
    {
        return All.FirstOrDefault(x => x.Code == code) ??
               throw new Exception($"The currency code is invalid");
    }

    public static IReadOnlyCollection<Currency> All = new[]
    {
        USD,
        EUR,
        CFA
    };
}