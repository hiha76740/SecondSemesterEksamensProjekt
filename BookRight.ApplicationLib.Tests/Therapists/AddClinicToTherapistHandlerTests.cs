using BookRight.ApplicationLib.Handlers.Therapists;
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Clinics;
using BookRight.DomainLib.Entities.Therapists;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Therapists.DTOs;
using BookRight.FacadeLib.Commands.Therapists.Interfaces;
using Moq;

namespace BookRight.ApplicationLib.Tests.Therapists;

public class AddClinicToTherapistHandlerTests
{
    private static Clinic CreateClinic()
    {
        DateTime openingHour = new DateTime(2030, 5, 1, 8, 0, 0);
        return Clinic.Create(
            "Test Klinik 1",
            5,
            new OpeningHours(openingHour, openingHour.AddHours(8)),
            new Address("Testvej 21","6000", "Kolding"));
    }

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
    public async Task Handler_GivenValidIds_CallsSave()
    {
        // Arrange
        var therapist = CreateTherapist();
        var clinic = CreateClinic();

        var therapistRepositoryMock = new Mock<ITherapistRepository>();
        var clinicRepositoryMock = new Mock<IClinicRepository>();

        therapistRepositoryMock
            .Setup(x => x.GetByIdAsync(therapist.Id))
            .ReturnsAsync(therapist);

        clinicRepositoryMock
            .Setup(x => x.GetByIdAsync(clinic.Id))
            .ReturnsAsync(clinic);

        var handler = new AddClinicToTherapistHandler(therapistRepositoryMock.Object, clinicRepositoryMock.Object) as IAddClinicToTherapistHandler;
        var command = new AddClinicToTherapistCommand(therapist.Id, clinic.Id);

        // Act
        await handler.Handle(command);

        // Assert
        therapistRepositoryMock.Verify(x => x.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handler_GivenInvalidTherapistId_CastNotFoundException()
    {
        // Arrange
        var clinic = CreateClinic();

        var therapistRepositoryMock = new Mock<ITherapistRepository>();
        var clinicRepositoryMock = new Mock<IClinicRepository>();

        therapistRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Therapist?)null);

        clinicRepositoryMock
            .Setup(r => r.GetByIdAsync(clinic.Id))
            .ReturnsAsync(clinic);

        var handler = new AddClinicToTherapistHandler(therapistRepositoryMock.Object, clinicRepositoryMock.Object) as IAddClinicToTherapistHandler;
        var command = new AddClinicToTherapistCommand(It.IsAny<Guid>(), clinic.Id);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));
    }

    [Fact]
    public async Task Handler_GivenInvalidClinicId_CastNotFoundException()
    {
        // Arrange
        var therapist = CreateTherapist();

        var therapistRepositoryMock = new Mock<ITherapistRepository>();
        var clinicRepositoryMock = new Mock<IClinicRepository>();

        therapistRepositoryMock
            .Setup(x => x.GetByIdAsync(therapist.Id))
            .ReturnsAsync(therapist);

        clinicRepositoryMock
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Clinic?)null);

        var handler = new AddClinicToTherapistHandler(therapistRepositoryMock.Object, clinicRepositoryMock.Object) as IAddClinicToTherapistHandler;
        var command = new AddClinicToTherapistCommand(therapist.Id, It.IsAny<Guid>());

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));
    }
}