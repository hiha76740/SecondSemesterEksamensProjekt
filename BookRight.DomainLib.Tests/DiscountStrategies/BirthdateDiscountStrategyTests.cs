using BookRight.DomainLib.Discounts;
using BookRight.DomainLib.Discounts.DiscountStrategies;
using BookRight.DomainLib.Entities.Campaigns;
using BookRight.DomainLib.Entities.Treatments;
using BookRight.DomainLib.Enums;
using System.Globalization;

namespace BookRight.DomainLib.Tests.DiscountStrategies;

public class BirthdateDiscountStrategyTests
{
    private static decimal Price => 500m;
    private static DateOnly CustomerBirthdate => new DateOnly(1990, 6, 5);
    private static decimal CustomerTotalPast12Months => 0;

    private static DiscountTypes DiscountTypes => DiscountTypes.BirthdayMonth;

    private static BirthdateDiscountStrategy Strategy => new BirthdateDiscountStrategy();

    private static Treatment Treatment => Treatment.Create("test", 500, TimeSpan.FromMinutes(45), CertificationTypes.Acupuncture, 5);


    // ---------------------------------------------------------
    // 1. Create tests (Creating a Birthday Discount Strategy)
    // ---------------------------------------------------------

    [Theory]
    [InlineData("04-06-2026")]
    [InlineData("20-06-2026")]
    public void Create_GivenValidDate_ShallReturnFinalPrice(string stringBookingDate)
    {
        // Arrange
        DateTime.TryParseExact(stringBookingDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime bookingDate);
        decimal finalPrice = 375m;
        int NumberOfBirthdayDiscountUsed = 0;

        var request = new PriceCalculatorInput(Price, bookingDate, CustomerBirthdate, CustomerTotalPast12Months, NumberOfBirthdayDiscountUsed, new List<Treatment>() { Treatment }, new List<Campaign>());
        var expected = new PriceCalculatorResult(Price, finalPrice, DiscountTypes,true);

        // Act
        var result = Strategy.CalculatePrice(request);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("04-07-2026")]
    [InlineData("20-05-2026")]
    public void Create_GivenInvalidDate_ShallReturnNormalPrice(string stringBookingDate)
    {
        // Arrange
        DateTime.TryParseExact(stringBookingDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime bookingDate);
        int NumberOfBirthdayDiscountUsed = 0;

        var request = new PriceCalculatorInput(Price, bookingDate, CustomerBirthdate, CustomerTotalPast12Months, NumberOfBirthdayDiscountUsed, new List<Treatment>() { Treatment }, new List<Campaign>());
        var expected = new PriceCalculatorResult(Price, Price, DiscountTypes, false);

        // Act
        var result = Strategy.CalculatePrice(request);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Create_GivenLimitForUses_ShallReturnNormalPrice()
    {
        // Arrange
        DateTime bookingDate = new DateTime(2026, 06, 04);
        int NumberOfBirthdayDiscountUsed = 1;

        var request = new PriceCalculatorInput(Price, bookingDate, CustomerBirthdate, CustomerTotalPast12Months, NumberOfBirthdayDiscountUsed, new List<Treatment>() { Treatment }, new List<Campaign>());
        var expected = new PriceCalculatorResult(Price, Price, DiscountTypes, false);

        // Act
        var result = Strategy.CalculatePrice(request);

        Assert.Equal(expected, result);
    }
}
