using BookRight.ApplicationLib.Handlers.Therapists;
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Therapists;
using BookRight.FacadeLib.DTO;
using Moq;

namespace BookRight.ApplicationLib.Tests.Handlers.Therapists;

public class CreateTherapistHandlerTests
{
    private readonly Mock<ITherapistRepository> _therapistRepositoryMock;

    private readonly CreateTherapistHandler _handler;


    public CreateTherapistHandlerTests()
    {
        _therapistRepositoryMock = new Mock<ITherapistRepository>();

        _handler = new CreateTherapistHandler(
            _therapistRepositoryMock.Object);
    }
}
