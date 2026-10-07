using ShopFlow.Domain.Common;
using ShopFlow.Domain.Orders;

namespace ShopFlow.Domain.Tests.Orders;

public sealed class OrderTests
{
    [Fact]
    public void Create_ShouldCreateDraftOrder()
    {
        // Act
        var order = Order.Create();

        // Assert
        Assert.Equal(OrderStatus.Draft, order.Status);
        Assert.Empty(order.Items);
    }
    
    [Fact]
    public void AddProduct_ShouldAddProduct_WhenOrderIsDraft()
    {
        // Arrange
        var order = Order.Create();

        var productId = ProductId.New();

        var price = Money.Create(20m, "USD");

        // Act
        order.AddProduct(productId, price, 2);

        // Assert
        var item = Assert.Single(order.Items);

        Assert.Equal(productId, item.ProductId);
        Assert.Equal(2, item.Quantity);
        Assert.Equal(price, item.UnitPrice);
    }
    
    [Fact]
    public void AddProduct_ShouldThrow_WhenProductAlreadyExists()
    {
        // Arrange
        var order = Order.Create();

        var productId = ProductId.New();
        var price = Money.Create(20m, "USD");

        order.AddProduct(
            productId,
            price,
            1);

        // Act
        Action action = () =>
            order.AddProduct(
                productId,
                price,
                2);

        // Assert
        Assert.Throws<DomainException>(action);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(21)]
    [InlineData(100)]
    public void AddProduct_ShouldThrow_WhenQuantityIsInvalid(
        int quantity)
    {
        // Arrange
        var order = Order.Create();

        // Act
        Action action = () => order.AddProduct(ProductId.New(), Money.Create(20m, "USD"), quantity);

        // Assert
        Assert.Throws<DomainException>(action);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(20)]
    public void AddProduct_ShouldAcceptBoundaryQuantities(
        int quantity)
    {
        // Arrange
        var order = Order.Create();

        // Act
        order.AddProduct(
            ProductId.New(),
            Money.Create(20m, "USD"),
            quantity);

        // Assert
        var item = Assert.Single(order.Items);

        Assert.Equal(quantity, item.Quantity);
    }
    
    [Fact]
    public void Confirm_ShouldSetStatusToConfirmed_WhenOrderContainsItems()
    {
        // Arrange
        var order = CreateOrderWithOneProduct();

        // Act
        order.Confirm();

        // Assert
        Assert.Equal(
            OrderStatus.Confirmed,
            order.Status);
    }
    
    [Fact]
    public void Confirm_ShouldThrow_WhenOrderIsEmpty()
    {
        // Arrange
        var order = Order.Create();

        // Act
        Action action = order.Confirm;

        // Assert
        Assert.Throws<DomainException>(action);
    }
    
    [Fact]
    public void AddProduct_ShouldThrow_WhenOrderIsConfirmed()
    {
        // Arrange
        var order = CreateOrderWithOneProduct();

        order.Confirm();

        // Act
        Action action = () =>
            order.AddProduct(
                ProductId.New(),
                Money.Create(10m, "USD"),
                1);

        // Assert
        Assert.Throws<DomainException>(action);
    }
    
    [Fact]
    public void RemoveProduct_ShouldThrow_WhenOrderIsConfirmed()
    {
        // Arrange
        var order = Order.Create();

        var productId = ProductId.New();

        order.AddProduct(
            productId,
            Money.Create(10m, "USD"),
            1);

        order.Confirm();

        // Act
        Action action = () =>
            order.RemoveProduct(productId);

        // Assert
        Assert.Throws<DomainException>(action);
    }
    
    [Fact]
    public void ChangeProductQuantity_ShouldThrow_WhenOrderIsConfirmed()
    {
        // Arrange
        var order = Order.Create();

        var productId = ProductId.New();

        order.AddProduct(
            productId,
            Money.Create(10m, "USD"),
            1);

        order.Confirm();

        // Act
        Action action = () =>
            order.ChangeProductQuantity(
                productId,
                5);

        // Assert
        Assert.Throws<DomainException>(action);
    }
    
    [Fact]
    public void Ship_ShouldSetStatusToShipped_WhenOrderIsConfirmed()
    {
        // Arrange
        var order = CreateOrderWithOneProduct();

        order.Confirm();

        // Act
        order.Ship();

        // Assert
        Assert.Equal(
            OrderStatus.Shipped,
            order.Status);
    }
    
    [Fact]
    public void Cancel_ShouldCancelDraftOrder()
    {
        // Arrange
        var order = Order.Create();

        // Act
        order.Cancel();

        // Assert
        Assert.Equal(
            OrderStatus.Cancelled,
            order.Status);
    }
    
    [Fact]
    public void Cancel_ShouldCancelConfirmedOrder()
    {
        // Arrange
        var order = CreateOrderWithOneProduct();

        order.Confirm();

        // Act
        order.Cancel();

        // Assert
        Assert.Equal(
            OrderStatus.Cancelled,
            order.Status);
    }
    
    [Fact]
    public void Cancel_ShouldThrow_WhenOrderIsShipped()
    {
        // Arrange
        var order = CreateOrderWithOneProduct();

        order.Confirm();
        order.Ship();

        // Act
        Action action = order.Cancel;

        // Assert
        Assert.Throws<DomainException>(action);
    }
    
    [Fact]
    public void Cancel_ShouldThrow_WhenOrderIsAlreadyCancelled()
    {
        // Arrange
        var order = Order.Create();

        order.Cancel();

        // Act
        Action action = order.Cancel;

        // Assert
        Assert.Throws<DomainException>(action);
    }
    
    
    private static Order CreateOrderWithOneProduct()
    {
        var order = Order.Create();

        order.AddProduct(
            ProductId.New(),
            Money.Create(10m, "USD"),
            1);

        return order;
    }
}

