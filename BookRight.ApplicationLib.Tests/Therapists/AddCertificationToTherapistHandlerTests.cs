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
    private static readonly Guid TherapistId = Guid.NewGuid();

    private static readonly Address Address =
        new("Testvej 1", "6700", "Esbjerg");

    private static readonly Email Email =
        new("test@test.dk");

    private static readonly PhoneNumber PhoneNumber =
        new("12345678");

    private static Therapist CreateTherapist()
    {
        return Therapist.Create(
            "AUTH123",
            "John Doe",
            550,
            Address,
            Email,
            PhoneNumber,
            new List<Guid>(),
            null);
    }

    [Fact]
    public async Task GivenValidCertification_WhenAddingCertification_ThenCertificationIsAdded()
    {

        var therapist = CreateTherapist();

        var therapistRepositoryMock =
            new Mock<ITherapistRepository>();

        therapistRepositoryMock
            .Setup(x => x.GetByIdAsync(TherapistId))
            .ReturnsAsync(therapist);

        IAddCertificationTypeHandler handler =
            new AddCertificationToTherapistHandler(
                therapistRepositoryMock.Object);

        var command = new AddCertificationTypeCommand(
            TherapistId,
            CertificationTypes.Acupuncture.ToString());

        await handler.Handle(command);

        Assert.Contains(
            CertificationTypes.Acupuncture,
            therapist.CertificationTypes);

        therapistRepositoryMock.Verify(
            x => x.SaveAsync(),
            Times.Once);
    }

    [Fact]
    public async Task GivenInvalidTherapistId_WhenAddingCertification_ThenCastNotFoundException()
    {

        var therapistRepositoryMock =
            new Mock<ITherapistRepository>();

        therapistRepositoryMock
            .Setup(x => x.GetByIdAsync(TherapistId))
            .ReturnsAsync((Therapist?)null);

        IAddCertificationTypeHandler handler =
            new AddCertificationToTherapistHandler(
                therapistRepositoryMock.Object);

        var command = new AddCertificationTypeCommand(
            TherapistId,
            CertificationTypes.Acupuncture.ToString());

        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(command));
    }

    [Fact]
    public async Task GivenInvalidCertification_WhenAddingCertification_ThenCastNotFoundException()
    {

        var therapist = CreateTherapist();

        var therapistRepositoryMock =
            new Mock<ITherapistRepository>();

        therapistRepositoryMock
            .Setup(x => x.GetByIdAsync(TherapistId))
            .ReturnsAsync(therapist);

        IAddCertificationTypeHandler handler =
            new AddCertificationToTherapistHandler(
                therapistRepositoryMock.Object);

        var command = new AddCertificationTypeCommand(
            TherapistId,
            "InvalidCertification");

        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(command));
    }
}