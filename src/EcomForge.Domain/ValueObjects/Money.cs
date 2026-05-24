namespace EcomForge.Domain.ValueObjects;

public sealed record Money(decimal Amount, string Currency = "BRL")
{
    public static Money From(decimal amount, string currency = "BRL")
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Money amount cannot be negative.");
        }

        return new Money(amount, currency);
    }
}
