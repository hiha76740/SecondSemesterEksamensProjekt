using BookRight.DomainLib.Discounts;
using BookRight.DomainLib.Discounts.DiscountStrategies;
using BookRight.DomainLib.Entities.Campaigns;
using BookRight.DomainLib.Entities.Treatments;
using BookRight.DomainLib.Enums;

namespace BookRight.DomainLib.Tests.DiscountStrategies;

public class SilverLoyalityDiscountStrategyTests
{
    private static decimal Price => 500m;
    private static int NumberOfBirthdayDiscountUsed => 0;
    private static DateTime CustomerBirthdate => new DateTime(1990, 5, 5);
    private static DateTime BookingDate => new DateTime(2026, 6, 4);
    private static DiscountTypes DiscountTypes => DiscountTypes.LoyaltySilver;
    private static SilverLoyalityDiscountStrategy Strategy => new();
    private static Treatment Treatment => Treatment.Create("test", 500, TimeSpan.FromMinutes(45), CertificationTypes.Acupuncture, 5);


    // ---------------------------------------------------------
    // 1. Create tests (Creating a Silver Discount Strategy)
    // ---------------------------------------------------------

    [Theory]
    [InlineData(10001)]
    [InlineData(25000)]
    public void Create_GivenValidData_ShallReturnDiscount(decimal CustomerTotalPast12Months)
    {
        // Arrange
        decimal finalPrice = 450m;

        var request = new PriceCalculatorInput(Price, BookingDate, CustomerBirthdate, CustomerTotalPast12Months, NumberOfBirthdayDiscountUsed, new List<Treatment>() { Treatment }, new List<Campaign>());
        var expected = new PriceCalculatorResult(Price, finalPrice, DiscountTypes, true);

        // Act
        var result = Strategy.CalculatePrice(request);

        // Assert
        Assert.Equal(expected, result);
    }


    [Theory]
    [InlineData(10000)]
    [InlineData(25001)]
    public void Create_GivenDataOutOfRange_ShallReturnNormalPrice(decimal CustomerTotalPast12Months)
    {
        // Arrange
        decimal finalPrice = 0;

        var request = new PriceCalculatorInput(Price, BookingDate, CustomerBirthdate, CustomerTotalPast12Months, NumberOfBirthdayDiscountUsed, new List<Treatment>() { Treatment }, new List<Campaign>());
        var expected = new PriceCalculatorResult(Price, Price, DiscountTypes, false);

        // Act
        var result = Strategy.CalculatePrice(request);

        // Assert
        Assert.Equal(expected, result);
    }
}
