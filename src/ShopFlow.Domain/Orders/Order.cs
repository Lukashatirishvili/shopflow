using ShopFlow.Domain.Common;

namespace ShopFlow.Domain.Orders;

public sealed class Order
{
    private readonly List<OrderItem> _items = [];

    private Order(OrderId id)
    {
        Id = id;
        Status = OrderStatus.Draft;
    }

    public OrderId Id { get; }

    public OrderStatus Status { get; private set; }

    public IReadOnlyCollection<OrderItem> Items =>
        _items.AsReadOnly();

    public static Order Create()
    {
        return new Order(OrderId.New());
    }

    public void AddProduct(
        ProductId productId,
        Money unitPrice,
        int quantity)
    {
        EnsureDraft();

        bool productAlreadyExists =
            _items.Any(item =>
                item.ProductId == productId);

        if (productAlreadyExists)
        {
            throw new DomainException(
                "Product already exists in the order.");
        }

        var orderItem = new OrderItem(
            productId,
            unitPrice,
            quantity);

        _items.Add(orderItem);
    }

    public void RemoveProduct(ProductId productId)
    {
        EnsureDraft();

        var orderItem = _items.FirstOrDefault(
            item => item.ProductId == productId);

        if (orderItem is null)
        {
            throw new DomainException(
                "Product does not exist in the order.");
        }

        _items.Remove(orderItem);
    }

    public void ChangeProductQuantity(
        ProductId productId,
        int quantity)
    {
        EnsureDraft();

        var orderItem = _items.FirstOrDefault(
            item => item.ProductId == productId);

        if (orderItem is null)
        {
            throw new DomainException(
                "Product does not exist in the order.");
        }

        orderItem.ChangeQuantity(quantity);
    }

    public void Confirm()
    {
        EnsureDraft();

        if (_items.Count == 0)
        {
            throw new DomainException(
                "An empty order cannot be confirmed.");
        }

        Status = OrderStatus.Confirmed;
    }

    public void Ship()
    {
        if (Status != OrderStatus.Confirmed)
        {
            throw new DomainException(
                "Only confirmed orders can be shipped.");
        }

        Status = OrderStatus.Shipped;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Shipped)
        {
            throw new DomainException(
                "A shipped order cannot be cancelled.");
        }

        if (Status == OrderStatus.Cancelled)
        {
            throw new DomainException(
                "Order is already cancelled.");
        }

        Status = OrderStatus.Cancelled;
    }

    private void EnsureDraft()
    {
        if (Status != OrderStatus.Draft)
        {
            throw new DomainException(
                "Order can only be modified while it is in draft status.");
        }
    }
}