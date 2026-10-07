namespace ShopFlow.Domain.Orders;

public readonly record struct OrderId(Guid Value)
{
    public static OrderId New()
    {
        return new OrderId(Guid.NewGuid());
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}