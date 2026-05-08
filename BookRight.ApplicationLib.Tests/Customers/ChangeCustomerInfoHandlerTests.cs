
using BookRight.ApplicationLib.Handlers.Customers;
using BookRight.ApplicationLib.Repositories;
using BookRight.FacadeLib.Commands.Customers.DTOs;
using BookRight.FacadeLib.Commands.Customers.Interfaces;
using Moq;

namespace BookRight.ApplicationLib.Tests.Customers;

public class ChangeCustomerInfoHandlerTests
{
    //Helpermethod to instatiate a CreateCustomerCommand Dto
    private static ChangeCustomerInfoCommand ChangeCustomerInfoCommandWithValidData(Guid? preferredTherapist = null)
        => new ChangeCustomerInfoCommand(
            CustomerId: Guid.NewGuid(),
            "Torben",
            "Svendsen",
            new DateTime(1956, 7, 16),
            "Gets easily confused",
            "Niels Bohrs Gade 43",
            "6534",
            "Testlev",
            "TorbenS@testing.com",
            "96538562",
            preferredTherapist
            );

    // Mock of relevant repositories
    private readonly Mock<ICustomerRepository> _mockCustomerRepo = new();
    private readonly Mock<ITherapistRepository> _mockTherapistRepo = new();

    // SystemUnderTest
    private IChangeCustomerInfoHandler CreateSut() => new ChangeCustomerInfoHandler(_mockCustomerRepo.Object, _mockTherapistRepo.Object);


}
