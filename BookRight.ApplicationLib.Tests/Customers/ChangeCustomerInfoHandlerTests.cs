
using BookRight.ApplicationLib.Handlers.Customers;
using BookRight.ApplicationLib.Repositories;
using BookRight.FacadeLib.Commands.Customers.DTOs;
using BookRight.FacadeLib.Commands.Customers.Interfaces;
using Moq;

namespace BookRight.ApplicationLib.Tests.Customers;

public class ChangeCustomerInfoHandlerTests
{
    // Helpermethod for instatiating a ChangeCustomerInfoCommand Dto
    private static ChangeCustomerInfoCommand CreateChangeCustomerInfoCommandWithValidData(
        Guid customerId,
        string? firstName = null,
        string? lastName = null,
        DateTime? birthDate = null,
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
            birthDate ?? new DateTime(1956, 7, 16),
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


}
