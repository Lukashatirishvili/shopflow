using ShopFlow.Domain.Common;
using ShopFlow.Domain.Orders;

namespace ShopFlow.Domain.Tests.Orders;

public sealed class MoneyTests
{
    [Fact]
    public void Create_ShouldCreateMoney_WhenValuesAreValid()
    {
        // Arrange
        const decimal amount = 25.50m;
        const string currency = "USD";

        // Act
        var money = Money.Create(amount, currency);

        // Assert
        Assert.Equal(amount, money.Amount);
        Assert.Equal(currency, money.Currency);
    }

    [Fact]
    public void Create_ShouldNormalizeCurrency()
    {
        // Act
        var money = Money.Create(10m, "usd");

        // Assert
        Assert.Equal("USD", money.Currency);
    }

    [Fact]
    public void Create_ShouldThrow_WhenAmountIsNegative()
    {
        // Act
        Action action = () =>
            Money.Create(-10m, "USD");

        // Assert
        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void Create_ShouldThrow_WhenCurrencyIsEmpty()
    {
        // Act
        Action action = () =>
            Money.Create(10m, "");

        // Assert
        Assert.Throws<DomainException>(action);
    }

    [Theory]
    [InlineData("US")]
    [InlineData("USDD")]
    [InlineData("A")]
    public void Create_ShouldThrow_WhenCurrencyDoesNotHaveThreeCharacters(
        string currency)
    {
        // Act
        Action action = () =>
            Money.Create(10m, currency);

        // Assert
        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void EqualMoney_ShouldBeEqual()
    {
        // Arrange
        var first = Money.Create(20m, "USD");
        var second = Money.Create(20m, "usd");

        // Assert
        Assert.Equal(first, second);
    }

    [Fact]
    public void Multiply_ShouldReturnCorrectMoney()
    {
        // Arrange
        var money = Money.Create(25m, "USD");

        // Act
        var result = money.Multiply(4);

        // Assert
        Assert.Equal(100m, result.Amount);
        Assert.Equal("USD", result.Currency);
    }
}