using BookRight.ApplicationLib.Handlers.Therapists;
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Therapists;
using BookRight.FacadeLib.Commands.Therapists.DTOs;
using BookRight.FacadeLib.Commands.Therapists.Interfaces;
using Moq;

namespace BookRight.ApplicationLib.Tests.Therapists;

public class CreateTherapistHandlerTests
{
    // ---------------------------------------------------------
    // 1. Handle tests (Creates a therapist)
    // ---------------------------------------------------------

    [Fact]
    public async Task Handle_GivenValidCommand_CallsAddAndSave()
    {
        var mockTherapistRepo = new Mock<ITherapistRepository>();
        var mockClinicRepo = new Mock<IClinicRepository>();

        var handler = new CreateTherapistHandler(mockTherapistRepo.Object,mockClinicRepo.Object) as ICreateTherapistHandler;

        var command = new CreateTherapistCommand(
            "AUTH123",
            "John Doe",
            550,
            "Testvej 1",
            "6700",
            "Esbjerg",
            "test@test.dk",
            "12345678",
            new List<Guid> { Guid.Parse("d62f5c2d-a5e6-4523-902d-108acac956c8") },
            new List<string>());
        // Act
        await handler.Handle(command);


        // Assert
        mockTherapistRepo.Verify(r => r.AddAsync(It.IsAny<Therapist>()),Times.Once);
        mockTherapistRepo.Verify(r => r.SaveAsync(),Times.Once);
    }
}
