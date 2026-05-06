using BookRight.ApplicationLib.Handlers.Customers;
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Customers;
using BookRight.FacadeLib.Handlers.Customers.DTOs;
using BookRight.FacadeLib.Handlers.Customers.Interfaces;
using Moq;
using Xunit;

namespace BookRight.ApplicationLib.Tests.Customers;

public class CreateCustomerHandlerTests
{
    //Testdata
    private static string Firstname => "Anne";
    private static string Lastname => "Andersen";
    private static DateTime Birthdate => new DateTime(1989, 2, 12);
    private static string Note => "Milk-allergy";
    private static string Street => "Test Allé 28";
    private static string PostalCode => "4321";
    private static string City => "Testby";
    private static string Email => "Anne.Andersen@testing.com";
    private static string PhoneNumber => "56781234";

    //Helpermethod to instatiate a CreateCustomerCommand Dto
    private static CreateCustomerCommand CreateCustomerCommandWithValidData(
        string? firstName = null,
        string? lastName = null,
        DateTime? birthDate = null,
        string? street = null,
        string? postalCode = null,
        string? city = null,
        string? email = null,
        string? phoneNumber = null,
        Guid? therapistId = null)
        => new CreateCustomerCommand(
            firstName ?? Firstname,
            lastName ?? Lastname,
            birthDate ?? Birthdate,
            Note,
            street ?? Street,
            postalCode ?? PostalCode,
            city ?? City,
            email ?? Email,
            phoneNumber ?? PhoneNumber,
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
    // 1. CreateCustomerHandler Tests
    // ---------------------------------------------------------

    [Fact]
    public async Task Handle_GivenValidCommandWithoutTherapist_CallAddAndSave()
    {
        //Arrange
        var command = CreateCustomerCommandWithValidData();

        //Act
        await CreateSut().Handle(command);

        //Assert
        _mockCustomerRepo.Verify(c => c.AddAsync(It.IsAny<Customer>()), Times.Once);
        _mockCustomerRepo.Verify(c => c.SaveAsync(), Times.Once);
    }
}
