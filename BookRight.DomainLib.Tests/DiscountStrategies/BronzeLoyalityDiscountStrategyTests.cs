using BookRight.DomainLib.Discounts;
using BookRight.DomainLib.Discounts.DiscountStrategies;
using BookRight.DomainLib.Entities.Campaigns;
using BookRight.DomainLib.Entities.Treatments;
using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Tests.DiscountStrategies;

public class BronzeLoyalityDiscountStrategyTests
{
    private static decimal Price => 500m;
    private static int NumberOfBirthdayDiscountUsed => 0;
    private static DateOnly CustomerBirthdate => new DateOnly(1990, 5, 5);
    private static DateTime BookingDate => new DateTime(2026, 6, 4);
    private static DiscountTypes DiscountTypes => DiscountTypes.LoyaltyBronze;
    private static BronzeLoyalityDiscountStrategy Strategy => new BronzeLoyalityDiscountStrategy();

    private static Treatment Treatment => Treatment.Create("test", 500, TimeSpan.FromMinutes(45), CertificationTypes.Acupuncture, 5);

    // ---------------------------------------------------------
    // 1. Create tests (Creating a Bronze Discount Strategy)
    // ---------------------------------------------------------

    [Theory]
    [InlineData(3000)]
    [InlineData(3001)]
    [InlineData(9999)]
    [InlineData(10000)]
    public void Create_GivenValidData_ShallReturnDiscount(decimal CustomerTotalPast12Months)
    {
        // Arrange
        decimal finalPrice = 475m;

        var request = new PriceCalculatorInput(Price, BookingDate, CustomerBirthdate, CustomerTotalPast12Months, NumberOfBirthdayDiscountUsed, new List<Treatment>() { Treatment }, new List<Campaign>());
        var expected = new PriceCalculatorResult(Price, finalPrice, DiscountTypes,true);

        // Act
        var result = Strategy.CalculatePrice(request);

        // Assert
        Assert.Equal(expected, result);
    }


    [Theory]
    [InlineData(10001)]
    [InlineData(2999)]
    public void Create_GivenDataOutOfRange_ShallReturnNormalPrice(decimal CustomerTotalPast12Months)
    {
        // Arrange
        var request = new PriceCalculatorInput(Price, BookingDate, CustomerBirthdate, CustomerTotalPast12Months, NumberOfBirthdayDiscountUsed, new List<Treatment>() { Treatment }, new List<Campaign>());
        var expected = new PriceCalculatorResult(Price, Price, DiscountTypes, false);

        // Act
        var result = Strategy.CalculatePrice(request);

        // Assert
        Assert.Equal(expected, result);
    }
}
