using BookRight.ApplicationLib.Handlers.Customers;
using BookRight.ApplicationLib.Repositories;
using Moq;
using Xunit;

namespace BookRight.ApplicationLib.Tests.Customers;

public class CreateCustomerHandlerTests
{
    private readonly Mock<ICustomerRepository> _mockCustomerRepo = new();
    private readonly Mock<ITherapistRepository> _mockTherapistRepo = new();

    private CreateCustomerHandler CreateSut() => new(
        _mockCustomerRepo.Object,
        _mockTherapistRepo.Object
        );
}
