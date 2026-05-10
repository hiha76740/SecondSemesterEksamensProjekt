
using BookRight.ApplicationLib.Handlers.Customers;
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Customers;
using BookRight.DomainLib.Entities.Therapists;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Customers.DTOs;
using BookRight.FacadeLib.Commands.Customers.Interfaces;
using Castle.Core.Resource;
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


    // ---------------------------------------------------------
    // 1. Handle tests (Changes a Customer)
    // ---------------------------------------------------------

    [Fact]
    public async Task Handle_GivenCommandWithNewFirstname_CallsSave()
    {
        // Arrange
        var newFirstname = "Thomas";
        var customer = CreateTestCustomerWithValidData();

        var mockCustomerRepo = new Mock<ICustomerRepository>();
        var mockTherapistRepo = new Mock<ITherapistRepository>();

        mockCustomerRepo.Setup(r => r.GetByIdAsync(customer.Id))
            .ReturnsAsync(customer);


        var command = CreateChangeCustomerInfoCommandWithValidData(customerId: customer.Id, firstName: newFirstname);
        var handler = new ChangeCustomerInfoHandler(mockCustomerRepo.Object, mockTherapistRepo.Object) as IChangeCustomerInfoHandler;

        // Act
        await handler.Handle(command);

        // Assert
        mockCustomerRepo.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenValidCommandNewLastname_CallsSave()
    {
        // Arrange
        var newLastname = "Jensen";
        var customer = CreateTestCustomerWithValidData();

        var mockCustomerRepo = new Mock<ICustomerRepository>();
        var mockTherapistRepo = new Mock<ITherapistRepository>();

        mockCustomerRepo.Setup(r => r.GetByIdAsync(customer.Id))
            .ReturnsAsync(customer);

        var command = CreateChangeCustomerInfoCommandWithValidData(customerId: customer.Id, lastName: newLastname);
        var handler = new ChangeCustomerInfoHandler(mockCustomerRepo.Object, mockTherapistRepo.Object) as IChangeCustomerInfoHandler;


        // Act
        await handler.Handle(command);

        // Assert
        mockCustomerRepo.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenValidCommandNewAddress_CallsSave()
    {
        // Arrange
        var newStreet = "Rapgade 24";
        var newPostalCode = "8945";
        var newCity = "Anderup";
        var customer = CreateTestCustomerWithValidData();

        var mockCustomerRepo = new Mock<ICustomerRepository>();
        var mockTherapistRepo = new Mock<ITherapistRepository>();

        mockCustomerRepo.Setup(r => r.GetByIdAsync(customer.Id))
            .ReturnsAsync(customer);

        var command = CreateChangeCustomerInfoCommandWithValidData(customerId: customer.Id, street: newStreet, postalCode: newPostalCode, city: newCity);
        var handler = new ChangeCustomerInfoHandler(mockCustomerRepo.Object, mockTherapistRepo.Object) as IChangeCustomerInfoHandler;

        // Act
        await handler.Handle(command);

        // Assert
        mockCustomerRepo.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenValidCommandNewEmail_CallsSave()
    {
        // Arrange
        var newEmail = "TSvendsen@alias.com";
        var customer = CreateTestCustomerWithValidData();

        var mockCustomerRepo = new Mock<ICustomerRepository>();
        var mockTherapistRepo = new Mock<ITherapistRepository>();

        mockCustomerRepo.Setup(r => r.GetByIdAsync(customer.Id))
            .ReturnsAsync(customer);

        var command = CreateChangeCustomerInfoCommandWithValidData(customerId: customer.Id, email: newEmail);
        var handler = new ChangeCustomerInfoHandler(mockCustomerRepo.Object, mockTherapistRepo.Object) as IChangeCustomerInfoHandler;

        // Act
        await handler.Handle(command);

        // Assert
        mockCustomerRepo.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenValidCommandNewPhoneNumber_CallsSave()
    {
        // Arrange
        var newPhoneNumber = "78329234";
        var customer = CreateTestCustomerWithValidData();

        var mockCustomerRepo = new Mock<ICustomerRepository>();
        var mockTherapistRepo = new Mock<ITherapistRepository>();

        mockCustomerRepo.Setup(r => r.GetByIdAsync(customer.Id))
            .ReturnsAsync(customer);

        var command = CreateChangeCustomerInfoCommandWithValidData(customerId: customer.Id, phoneNumber: newPhoneNumber);
        var handler = new ChangeCustomerInfoHandler(mockCustomerRepo.Object, mockTherapistRepo.Object) as IChangeCustomerInfoHandler;

        // Act
        await handler.Handle(command);

        // Assert
        mockCustomerRepo.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenValidCommandNoChanges_NeverCallsSave()
    {
        // Arrange
        var customer = CreateTestCustomerWithValidData();

        var mockCustomerRepo = new Mock<ICustomerRepository>();
        var mockTherapistRepo = new Mock<ITherapistRepository>();

        mockCustomerRepo.Setup(r => r.GetByIdAsync(customer.Id))
            .ReturnsAsync(customer);

        var command = CreateChangeCustomerInfoCommandWithValidData(customerId: customer.Id);
        var handler = new ChangeCustomerInfoHandler(mockCustomerRepo.Object, mockTherapistRepo.Object) as IChangeCustomerInfoHandler;


        // Act
        await handler.Handle(command);

        // Assert
        mockCustomerRepo.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenInvalidCommandNonExsistingPreferredTherapist_CastNotFoundException()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        var mockCustomerRepo = new Mock<ICustomerRepository>();
        var mockTherapistRepo = new Mock<ITherapistRepository>();

        mockTherapistRepo
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Therapist?)null);

        var command = CreateChangeCustomerInfoCommandWithValidData(customerId: customerId, preferredTherapist: It.IsAny<Guid>());
        var handler = new ChangeCustomerInfoHandler(mockCustomerRepo.Object, mockTherapistRepo.Object) as IChangeCustomerInfoHandler;


        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));
    }

    [Fact]
    public async Task Handle_GivenInvalidCommandNonExsistingCustomer_CastNotFoundException()
    {
        // Arrange
        var mockCustomerRepo = new Mock<ICustomerRepository>();
        var mockTherapistRepo = new Mock<ITherapistRepository>();

        mockCustomerRepo
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Customer?)null);

        var command = CreateChangeCustomerInfoCommandWithValidData(customerId: It.IsAny<Guid>());
        var handler = new ChangeCustomerInfoHandler(mockCustomerRepo.Object, mockTherapistRepo.Object) as IChangeCustomerInfoHandler;


        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));

    }

}
