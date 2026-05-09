using BookRight.ApplicationLib.Handlers.Therapists;
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Therapists;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Therapists.DTOs;
using BookRight.FacadeLib.Commands.Therapists.Interfaces;
using Moq;

namespace BookRight.ApplicationLib.Tests.Therapists;

public class AddCertificationToTherapistHandlerTests
{

    private static Therapist CreateTherapist()
    {
        return Therapist.Create(
            "AUTH123",
            "John Doe",
            550,
            new Address("Testvej 1", "6700", "Esbjerg"),
            new Email("test@test.dk"),
            new PhoneNumber("12345678"),
            new List<Guid>() { Guid.NewGuid() },
            null);
    }

    [Fact]
    public async Task GivenValidCertification_WhenAddingCertification_ThenCertificationIsAdded()
    {
        // Arrange
        var therapist = CreateTherapist();

        var therapistRepositoryMock =
            new Mock<ITherapistRepository>();

        therapistRepositoryMock
            .Setup(x => x.GetByIdAsync(therapist.Id))
            .ReturnsAsync(therapist);

        var handler = new AddCertificationToTherapistHandler(therapistRepositoryMock.Object) as IAddCertificationTypeHandler;
        var command = new AddCertificationTypeCommand(therapist.Id, CertificationTypes.Acupuncture.ToString());

        // Act
        await handler.Handle(command);

        // Assert
        therapistRepositoryMock.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task GivenInvalidTherapistId_WhenAddingCertification_ThenCastNotFoundException()
    {
        // Arrange
        var therapistRepositoryMock =
            new Mock<ITherapistRepository>();

        therapistRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Therapist?)null);

        var handler = new AddCertificationToTherapistHandler(therapistRepositoryMock.Object) as IAddCertificationTypeHandler;
        var command = new AddCertificationTypeCommand(It.IsAny<Guid>(), CertificationTypes.Acupuncture.ToString());

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));
    }

    [Fact]
    public async Task GivenInvalidCertification_WhenAddingCertification_ThenCastNotFoundException()
    {
        // Arrange
        var certificationType = "InvalidCertification";
        var therapist = CreateTherapist();

        var therapistRepositoryMock =
            new Mock<ITherapistRepository>();

        therapistRepositoryMock
            .Setup(x => x.GetByIdAsync(therapist.Id))
            .ReturnsAsync(therapist);

        var handler = new AddCertificationToTherapistHandler(therapistRepositoryMock.Object) as IAddCertificationTypeHandler;
        var command = new AddCertificationTypeCommand(therapist.Id, certificationType);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));
    }
}