
using BookRight.ApplicationLib.Handlers.Customers;
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Customers;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Customers.DTOs;
using BookRight.FacadeLib.Commands.Customers.Interfaces;
using Moq;

namespace BookRight.ApplicationLib.Tests.Customers;

public class ChangeCustomerInfoHandlerTests
{
    // Helpermethod for instantiating a Customer-object for Mock-tests
    private static Customer CreateTestCustomerWithValidData(Guid? preferredTherapist = null)
        => Customer.Create(
    "Torben",
    "Svendsen",
    new DateTime(1956, 7, 16),
    new Address("Niels Bohrs Gade 43", "6534", "Testlev"),
    new Email("TorbenS@testing.com"),
    new PhoneNumber("96538562"),
    "Gets easily confused",
    preferredTherapist);

    // Helpermethod for instatiating a ChangeCustomerInfoCommand Dto
    private static ChangeCustomerInfoCommand CreateChangeCustomerInfoCommandWithValidData(
        Guid customerId,
        string? firstName = null,
        string? lastName = null,
        string? note = null,
        string? street = null,
        string? postalCode = null,
        string? city = null,
        string? email = null,
        string? phoneNumber = null,
        Guid? preferredTherapist = null
        )
        => new ChangeCustomerInfoCommand(
            customerId,
            firstName ?? "Torben",
            lastName ?? "Svendsen",
            new DateTime(1956, 7, 16),
            note ?? "Gets easily confused",
            street ?? "Niels Bohrs Gade 43",
            postalCode ?? "6534",
            city ?? "Testlev",
            email ?? "TorbenS@testing.com",
            phoneNumber ?? "96538562",
            preferredTherapist
            );

    // Mock of relevant repositories
    private readonly Mock<ICustomerRepository> _mockCustomerRepo = new();
    private readonly Mock<ITherapistRepository> _mockTherapistRepo = new();

    // SystemUnderTest
    private IChangeCustomerInfoHandler CreateSut() => new ChangeCustomerInfoHandler(_mockCustomerRepo.Object, _mockTherapistRepo.Object);


    // ---------------------------------------------------------
    // 1. Handle tests (Changes a Customer)
    // ---------------------------------------------------------

    [Fact]
    public async Task Handle_GivenValidCommandNewFirstname_ShallCallSaveAsync()
    {
        // Arrange
        var newFirstname = "Thomas";
        var customer = CreateTestCustomerWithValidData();
        var command = CreateChangeCustomerInfoCommandWithValidData(customerId: customer.Id, firstName: newFirstname);

        _mockCustomerRepo.Setup(c => c.GetByIdAsync(customer.Id))
            .ReturnsAsync(customer);

        // Act
        await CreateSut().Handle(command);

        // Assert
        _mockCustomerRepo.Verify(c => c.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenValidCommandNoChanges_ShallNotCallSaveAsync()
    {
        // Arrange
        var customer = CreateTestCustomerWithValidData();
        var command = CreateChangeCustomerInfoCommandWithValidData(customerId: customer.Id);
        _mockCustomerRepo.Setup(c => c.GetByIdAsync(customer.Id))
            .ReturnsAsync(customer);


        // Act
        await CreateSut().Handle(command);

        // Assert
        _mockCustomerRepo.Verify(c => c.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenInvalidCommandEmptyPreferredTherapist_CastNotFoundException()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var preferredTherapistId = Guid.Empty;
        var command = CreateChangeCustomerInfoCommandWithValidData(customerId: customerId, preferredTherapist: preferredTherapistId);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => CreateSut().Handle(command));
    }

    [Fact]
    public async Task Handle_GivenInvalidCommandNoCustomer_CastNotFoundException()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var command = CreateChangeCustomerInfoCommandWithValidData(customerId: customerId);
        _mockCustomerRepo.Setup(c => c.GetByIdAsync(customerId))
            .ReturnsAsync((Customer?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => CreateSut().Handle(command));

    }
    // TODO: Reevaluate current tests and their naming, consider more tests and extra check to see if the appropriate Change-method is called.

}
