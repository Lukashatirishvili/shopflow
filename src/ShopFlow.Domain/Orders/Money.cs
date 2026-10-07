using ShopFlow.Domain.Common;

namespace ShopFlow.Domain.Orders;

public sealed record Money
{
    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public decimal Amount { get; }

    public string Currency { get; }

    public static Money Create(decimal amount, string currency)
    {
        if (amount < 0)
        {
            throw new DomainException(
                "Money amount cannot be negative.");
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new DomainException(
                "Currency is required.");
        }

        currency = currency.Trim().ToUpperInvariant();

        if (currency.Length != 3)
        {
            throw new DomainException(
                "Currency must contain exactly 3 characters.");
        }

        return new Money(amount, currency);
    }

    public Money Multiply(int multiplier)
    {
        if (multiplier < 0)
        {
            throw new DomainException(
                "Multiplier cannot be negative.");
        }

        return Create(
            Amount * multiplier,
            Currency);
    }
}