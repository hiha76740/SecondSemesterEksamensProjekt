using BookRight.DomainLib.Discounts;
using BookRight.DomainLib.Discounts.DiscountStrategies;
using BookRight.DomainLib.Entities.Campaigns;
using BookRight.DomainLib.Entities.Treatments;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Tests.DiscountStrategies;

public class CampaignDiscountStrategyTests
{
    private static DateOnly CustomerBirthdate => new DateOnly(1990, 5, 5);
    private static DateTime BookingDate => DateTime.Today.AddDays(3);
    private static decimal Price => 30000;
    private static decimal CustomerBookingLast12Months => 0;
    private static int NumberOfBirthdayDiscountUsed => 0;
    private static DiscountTypes DiscountTypes => DiscountTypes.Campaign;
    private static CampaignDiscountStrategy strategy => new();

    private static Treatment CreateTreatment() => 
        Treatment.Create(
        "test",
        500, 
        TimeSpan.FromMinutes(45),
        CertificationTypes.Acupuncture,
        5);


    private static Campaign CreateCampaign(Treatment treatment)
    {
        return Campaign.Create(
            "Test Kampagne",
            50,
            new CampaignPeriod(
                DateOnly.FromDateTime(DateTime.Today.AddDays(1)), 
                DateOnly.FromDateTime(DateTime.Today.AddDays(7))),
            new List<Guid>() { treatment.Id });
    }


    // ---------------------------------------------------------
    // 1. Create tests (Creating a Campain Discount Strategy)
    // ---------------------------------------------------------

    [Fact]
    public void Create_GivenValidData_ShallSucceed()
    {
        // Arrange
        decimal finalPrice = 15000;
        var treatment = CreateTreatment();
        var campaign = CreateCampaign(treatment);

        List<Campaign> campaigns = new() { campaign };

        var request = new PriceCalculatorInput(
            Price, 
            BookingDate,
            CustomerBirthdate,
            CustomerBookingLast12Months,
            NumberOfBirthdayDiscountUsed,
            new List<Treatment>() { treatment },
            campaigns);

        var expected = new PriceCalculatorResult(Price, finalPrice, DiscountTypes,true);


        // Act
        var result = strategy.CalculatePrice(request);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Create_GivenInValidData_ShallReturnNormalPrice()
    {
        // Arrange
        var treatment = CreateTreatment();
       
        var request = new PriceCalculatorInput(
            Price,
            BookingDate,
            CustomerBirthdate,
            CustomerBookingLast12Months,
            NumberOfBirthdayDiscountUsed,
            new List<Treatment>() { treatment },
            new List<Campaign>());

        var expected = new PriceCalculatorResult(Price, Price, DiscountTypes,false);



        // Act
        var result = strategy.CalculatePrice(request);

        // Assert
        Assert.Equal(expected, result);
    }
}
