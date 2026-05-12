using BookRight.ApplicationLib.Repositories;
using BookRight.ApplicationLib.Services;
using BookRight.DomainLib.Discounts;
using BookRight.DomainLib.Discounts.DiscountStrategies;
using BookRight.DomainLib.Entities.Campaigns;
using BookRight.DomainLib.Entities.Customers;
using BookRight.DomainLib.Entities.Treatments;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Pricing.DTOs;
using BookRight.FacadeLib.Commands.Pricing.Interfaces;
using BookRight.FacadeLib.DTO;
using Moq;

namespace BookRight.ApplicationLib.Tests.Services;

public class PriceCalculatorServiceTests
{
    private static Campaign CreateCampaign(Treatment treatment) =>
        Campaign.Create(
            "Test kampagne",
            50,
            new CampaignPeriod(
                new DateOnly(2030,5,1),
                new DateOnly(2030,5,10)),
            new List<Guid>() { treatment.Id}
            );

    private static Customer CreateCustomer() =>
        Customer.Create(
            "Susan",
            "Bones",
            new DateOnly(1991, 1, 8),
            new Address("testvej 2", "4321", "FantasiBy2"),
            new Email("test2@test.dk"),
            new PhoneNumber("87654321"),
            "");

    private readonly static List<IDiscountStrategy> Strategies = new List<IDiscountStrategy> 
    { 
        new CampaignDiscountStrategy(), 
        new NoDiscountStrategy(), 
        new BirthDateDiscountStrategy(),
        new BronzeLoyalityDiscountStrategy(),
        new SilverLoyalityDiscountStrategy(), 
        new GoldLoyalityDiscountStrategy(),
    };

    private static Treatment CreateTreatment()
    {
        return Treatment.Create(
            "test", 
            550, 
            TimeSpan.FromMinutes(45), 
            CertificationTypes.Acupuncture,
            5);
    }


    [Theory]
    [InlineData(500, 550, DiscountTypes.None)]
    [InlineData(3001, 522.5, DiscountTypes.LoyaltyBronze)]
    [InlineData(10001, 495, DiscountTypes.LoyaltySilver)]
    [InlineData(25001, 467.5, DiscountTypes.LoyaltyGold)]

    public async Task Handle_GivenValidCommand_ShallSucceed(decimal bookingHistorySum, decimal finalPrice, DiscountTypes discountType)
    {
        // Arrange
        var Customer = CreateCustomer();
        var _strategies = Strategies.AsEnumerable();
        var bookingDate = new DateTime(2030, 5, 5);
        var treatment = CreateTreatment();

        var allTreatments = new List<Treatment>() { treatment };

        var mockCustomerRepo = new Mock<ICustomerRepository>();
        mockCustomerRepo.Setup(r => r.GetByIdAsync(Customer.Id))
            .ReturnsAsync(Customer);

        var mockBookingRepo = new Mock<IBookingRepository>();
        mockBookingRepo.Setup(r => r.GetBookingHistorySumAsync(Customer.Id, 12))
            .ReturnsAsync(bookingHistorySum);

        var mockTreatmentRepo = new Mock<ITreatmentRepository>();
        mockTreatmentRepo.Setup(r => r.GetByIdAsync(treatment.Id))
            .ReturnsAsync(treatment);

        mockTreatmentRepo.Setup(r => r.GetAllAsync())
            .ReturnsAsync(allTreatments);

        var mockCampaignRepo = new Mock<ICampaignRepository>();
        mockCampaignRepo.Setup(r => r.GetActiveCampaignsAsync())
            .ReturnsAsync(Array.Empty<Campaign>());


        var handler = new PriceCalculatorService(Strategies, mockCustomerRepo.Object, mockTreatmentRepo.Object, mockBookingRepo.Object, mockCampaignRepo.Object) as IPriceCalculatorService;

        var command = new PriceCalculatorCommand(Customer.Id, treatment.Id, bookingDate);

        var expected = new PriceCalculatorDTO(treatment.Price, finalPrice, discountType.ToString());

        // Act
        var result = await handler.CalculatePrice(command);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task Handle_GivenCampaignWithValidCommand_ReturnsCampaignDiscount()
    {
        // Arrange
        var Customer = CreateCustomer();
        var _strategies = Strategies.AsEnumerable();
        var bookingDate = new DateTime(2030, 5, 5);
        var treatment = CreateTreatment();
        var campaign = CreateCampaign(treatment);

        var activeCampaigns = new List<Campaign>() { campaign };
        var allTreatments = new List<Treatment>() { treatment };

        var mockCustomerRepo = new Mock<ICustomerRepository>();
        mockCustomerRepo.Setup(r => r.GetByIdAsync(Customer.Id))
            .ReturnsAsync(Customer);

        var mockBookingRepo = new Mock<IBookingRepository>();
        mockBookingRepo.Setup(r => r.GetBookingHistorySumAsync(Customer.Id, 12))
            .ReturnsAsync(0);

        var mockTreatmentRepo = new Mock<ITreatmentRepository>();
        mockTreatmentRepo.Setup(r => r.GetByIdAsync(treatment.Id))
            .ReturnsAsync(treatment);

        mockTreatmentRepo.Setup(r => r.GetAllAsync())
            .ReturnsAsync(allTreatments);

        var mockCampaignRepo = new Mock<ICampaignRepository>();
        mockCampaignRepo.Setup(r => r.GetActiveCampaignsAsync())
            .ReturnsAsync(activeCampaigns);


        var handler = new PriceCalculatorService(Strategies, mockCustomerRepo.Object, mockTreatmentRepo.Object, mockBookingRepo.Object, mockCampaignRepo.Object) as IPriceCalculatorService;

        var command = new PriceCalculatorCommand(Customer.Id, treatment.Id, bookingDate);

        var expected = new PriceCalculatorDTO(treatment.Price, 275, DiscountTypes.Campaign.ToString());

        // Act
        var result = await handler.CalculatePrice(command);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public async Task Handle_GivenBirthdayMonth_ReturnsBirthdayDiscount()
    {
        // Arrange
        var Customer = CreateCustomer();
        var _strategies = Strategies.AsEnumerable();
        var bookingDate = new DateTime(2026, 1, 17);
        var treatment = CreateTreatment();
        
        var allTreatments = new List<Treatment>() { treatment };

        var mockCustomerRepo = new Mock<ICustomerRepository>();
        mockCustomerRepo.Setup(r => r.GetByIdAsync(Customer.Id))
            .ReturnsAsync(Customer);

        var mockBookingRepo = new Mock<IBookingRepository>();
        mockBookingRepo.Setup(r => r.GetBookingHistorySumAsync(Customer.Id, 12))
            .ReturnsAsync(10000);

        var mockTreatmentRepo = new Mock<ITreatmentRepository>();
        mockTreatmentRepo.Setup(r => r.GetByIdAsync(treatment.Id))
            .ReturnsAsync(treatment);

        mockTreatmentRepo.Setup(r => r.GetAllAsync())
            .ReturnsAsync(allTreatments);

        var mockCampaignRepo = new Mock<ICampaignRepository>();
        mockCampaignRepo.Setup(r => r.GetActiveCampaignsAsync())
            .ReturnsAsync(Array.Empty<Campaign>());

        var handler = new PriceCalculatorService(Strategies, mockCustomerRepo.Object, mockTreatmentRepo.Object, mockBookingRepo.Object, mockCampaignRepo.Object) as IPriceCalculatorService;

        var calculatePriceCommand = new PriceCalculatorCommand(Customer.Id, treatment.Id, bookingDate);

        var expected = new PriceCalculatorDTO(treatment.Price, 412.5m, DiscountTypes.BirthdayMonth.ToString());

        // Act
        var result = await handler.CalculatePrice(calculatePriceCommand);

        // Assert
        Assert.Equal(expected, result);
    }
}
