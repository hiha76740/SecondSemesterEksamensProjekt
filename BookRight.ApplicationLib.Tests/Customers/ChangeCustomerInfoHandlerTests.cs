
using BookRight.FacadeLib.Commands.Customers.DTOs;

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

}
