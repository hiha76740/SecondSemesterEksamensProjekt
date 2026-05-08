using BookRight.ApplicationLib.Handlers.Therapists;
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Therapists;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Therapists.DTOs;
using BookRight.FacadeLib.Commands.Therapists.Interfaces;
using Moq;

namespace BookRight.ApplicationLib.Tests.Therapists;

public class RemoveClinicFromTherapistHandlerTests
{
    private static readonly Guid TherapistId = Guid.NewGuid();
    private static readonly Guid ClinicId = Guid.NewGuid();

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
            new List<Guid> { ClinicId },
            null);
    }

    [Fact]
    public async Task GivenValidIds_WhenRemovingClinic_ThenClinicIsRemoved()
    {

        var therapist = CreateTherapist();

        var therapistRepositoryMock =
            new Mock<ITherapistRepository>();

        var clinicRepositoryMock =
            new Mock<IClinicRepository>();

        therapistRepositoryMock
            .Setup(x => x.GetByIdAsync(TherapistId))
            .ReturnsAsync(therapist);

        clinicRepositoryMock
            .Setup(x => x.GetByIdAsync(ClinicId))
            .ReturnsAsync(Mock.Of<DomainLib.Entities.Clinics.Clinic>());

        IRemoveClinicFromTherapistHandler handler =
            new RemoveClinicFromTherapistHandler(
                therapistRepositoryMock.Object,
                clinicRepositoryMock.Object);

        var command = new RemoveClinicFromTherapistCommand(
            TherapistId,
            ClinicId);

        await handler.Handle(command);


        therapistRepositoryMock.Verify(
            x => x.SaveAsync(),
            Times.Once);
    }

    [Fact]
    public async Task GivenInvalidTherapistId_WhenRemovingClinic_ThenCastNotFoundException()
    {

        var therapistRepositoryMock =
            new Mock<ITherapistRepository>();

        var clinicRepositoryMock =
            new Mock<IClinicRepository>();

        therapistRepositoryMock
            .Setup(x => x.GetByIdAsync(TherapistId))
            .ReturnsAsync((Therapist?)null);

        IRemoveClinicFromTherapistHandler handler =
            new RemoveClinicFromTherapistHandler(
                therapistRepositoryMock.Object,
                clinicRepositoryMock.Object);

        var command = new RemoveClinicFromTherapistCommand(
            TherapistId,
            ClinicId);

        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(command));
    }

    [Fact]
    public async Task GivenInvalidClinicId_WhenRemovingClinic_ThenCastNotFoundException()
    {

        var therapist = CreateTherapist();

        var therapistRepositoryMock =
            new Mock<ITherapistRepository>();

        var clinicRepositoryMock =
            new Mock<IClinicRepository>();

        therapistRepositoryMock
            .Setup(x => x.GetByIdAsync(TherapistId))
            .ReturnsAsync(therapist);

        clinicRepositoryMock
            .Setup(x => x.GetByIdAsync(ClinicId))
            .ReturnsAsync((DomainLib.Entities.Clinics.Clinic?)null);

        IRemoveClinicFromTherapistHandler handler =
            new RemoveClinicFromTherapistHandler(
                therapistRepositoryMock.Object,
                clinicRepositoryMock.Object);

        var command = new RemoveClinicFromTherapistCommand(
            TherapistId,
            ClinicId);

        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(command));
    }
}