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

    [Fact]
    public async Task Handle_GivenValidCommand_AddsTherapistToRepository()
    {
        // Arrange

        var command = new CreateTherapistCommand(
            "AUTH123",
            "John Doe",
            550,
            "Testvej 1",
            "6700",
            "Esbjerg",
            "test@test.dk",
            "12345678");


        // Act

        await ((ICreateTherapistHandler)_handler)
            .Handle(command);


        // Assert

        _therapistRepositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<Therapist>()),
            Times.Once);
    }
    [Fact]
    public async Task Handle_GivenValidCommand_SavesRepository()
    {
        // Arrange

        var command = new CreateTherapistCommand(
            "AUTH123",
            "John Doe",
            550,
            "Testvej 1",
            "6700",
            "Esbjerg",
            "test@test.dk",
            "12345678");


        // Act

        await ((ICreateTherapistHandler)_handler)
            .Handle(command);


        // Assert

        _therapistRepositoryMock.Verify(
            x => x.SaveAsync(),
            Times.Once);
    }

