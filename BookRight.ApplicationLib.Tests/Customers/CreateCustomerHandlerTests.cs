using BookRight.ApplicationLib.Handlers.Customers;
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Customers;
using BookRight.FacadeLib.Commands.Customers.DTOs;
using BookRight.FacadeLib.Commands.Customers.Interfaces;
using Moq;

namespace BookRight.ApplicationLib.Tests.Customers;

public class CreateCustomerHandlerTests
{
    //Helpermethod to instatiate a CreateCustomerCommand Dto
    private static CreateCustomerCommand CreateCustomerCommandWithValidData( Guid? therapistId = null)
        => new CreateCustomerCommand(
            "Anne",
            "Andersen",
            new DateTime(1989, 2, 12),
            "Milk-allergy",
            "Test Allé 28",
            "4321",
            "Testby",
            "Anne.Andersen@testing.com",
            "56781234",
            therapistId
            );

    //Mock of relevant repositories
    private readonly Mock<ICustomerRepository> _mockCustomerRepo = new();
    private readonly Mock<ITherapistRepository> _mockTherapistRepo = new();

    //SystemUnderTest
    private ICreateCustomerHandler CreateSut() => new CreateCustomerHandler(
        _mockCustomerRepo.Object,
        _mockTherapistRepo.Object
        );


    // ---------------------------------------------------------
    // 1. Handle tests (Creates a Customer)
    // ---------------------------------------------------------

    [Fact]
    public async Task Handle_GivenValidCommandWithoutTherapist_CallAddAndSave()
    {
        // Arrange
        var command = CreateCustomerCommandWithValidData();

        // Act
        await CreateSut().Handle(command);

        // Assert
        _mockCustomerRepo.Verify(c => c.AddAsync(It.IsAny<Customer>()), Times.Once);
        _mockCustomerRepo.Verify(c => c.SaveAsync(), Times.Once);
    }
}
