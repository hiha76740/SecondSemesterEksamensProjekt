using BookRight.ApplicationLib.Handlers.Customers;
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Customers;
using BookRight.DomainLib.Entities.Therapists;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
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
            new DateOnly(1989, 2, 12),
            "Milk-allergy",
            "Test Allé 28",
            "4321",
            "Testby",
            "Anne.Andersen@testing.com",
            "56781234",
            therapistId
            );

    // ---------------------------------------------------------
    // 1. Handle tests (Creates a Customer)
    // ---------------------------------------------------------

    [Fact]
    public async Task Handle_GivenValidCommandWithoutTherapist_CallAddAndSave()
    {
        // Arrange
        var mockCustomerRepo = new Mock<ICustomerRepository>();
        var mockTherapistRepo = new Mock<ITherapistRepository>();

        var command = CreateCustomerCommandWithValidData();
        var handler = new CreateCustomerHandler(mockCustomerRepo.Object, mockTherapistRepo.Object ) as ICreateCustomerHandler;

        // Act
        await handler.Handle(command);

        // Assert
        mockCustomerRepo.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Once);
        mockCustomerRepo.Verify(r => r.SaveAsync(), Times.Once);
        mockTherapistRepo.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenValidCommandWithTherapist_CallAddAndSave()
    {
        // Arrange
        var mockCustomerRepo = new Mock<ICustomerRepository>();
        var mockTherapistRepo = new Mock<ITherapistRepository>();

        var therapist = Therapist.Create(
                "AUTH246",
                "Pia Hansen",
                300,
                new Address("Vestergade 4", "8765", "Testby"),
                new Email("PiaH@test.dk"),
                new PhoneNumber("67328954"),
                associatedClinics: new List<Guid> { Guid.NewGuid() }
                );

        mockTherapistRepo.Setup(r => r.GetByIdAsync(therapist.Id))
            .ReturnsAsync(therapist);

        var command = CreateCustomerCommandWithValidData(therapist.Id);
        var handler = new CreateCustomerHandler(mockCustomerRepo.Object, mockTherapistRepo.Object) as ICreateCustomerHandler;

        // Act
        await handler.Handle(command);

        // Assert
        mockCustomerRepo.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Once);
        mockCustomerRepo.Verify(r => r.SaveAsync(), Times.Once);
        mockTherapistRepo.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenInvalidCommandWithEmptyTherapist_CastNotFoundException()
    {
        // Arrange
        var mockCustomerRepo = new Mock<ICustomerRepository>();
        var mockTherapistRepo = new Mock<ITherapistRepository>();

        mockTherapistRepo
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Therapist?)null);

        var command = CreateCustomerCommandWithValidData(It.IsAny<Guid>());
        var handler = new CreateCustomerHandler(mockCustomerRepo.Object, mockTherapistRepo.Object) as ICreateCustomerHandler;

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));
    }

}
