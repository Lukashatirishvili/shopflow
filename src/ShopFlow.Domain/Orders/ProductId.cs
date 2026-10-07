namespace ShopFlow.Domain.Orders;

public readonly record struct ProductId(Guid Value)
{
    public static ProductId New()
    {
        return new ProductId(Guid.NewGuid());
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}