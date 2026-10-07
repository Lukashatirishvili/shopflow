using ShopFlow.Domain.Common;

namespace ShopFlow.Domain.Orders;

public sealed class OrderItem
{
    internal OrderItem(
        ProductId productId,
        Money unitPrice,
        int quantity)
    {
        ValidateQuantity(quantity);

        ProductId = productId;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    public ProductId ProductId { get; }

    public Money UnitPrice { get; }

    public int Quantity { get; private set; }

    public Money Total => UnitPrice.Multiply(Quantity);

    internal void ChangeQuantity(int quantity)
    {
        ValidateQuantity(quantity);

        Quantity = quantity;
    }

    private static void ValidateQuantity(int quantity)
    {
        if (quantity is < 1 or > 20)
        {
            throw new DomainException(
                "Order item quantity must be between 1 and 20.");
        }
    }
}