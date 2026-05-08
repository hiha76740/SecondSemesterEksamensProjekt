using BookRight.ApplicationLib.Handlers.Therapists;
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Therapists;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Therapists.DTOs;
using BookRight.FacadeLib.Commands.Therapists.Interfaces;
using Moq;

namespace BookRight.ApplicationLib.Tests.Handlers.Therapists;

public class ChangeTherapistInfoHandlerTests
{
    private static List<string> certificationType = new() { CertificationTypes.Physiotherapy.ToString(), CertificationTypes.Acupuncture.ToString() };

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
        new List<CertificationTypes> { CertificationTypes.Acupuncture, CertificationTypes.Dietary }
        );
    }
    private static ChangeTherapistInfoCommand CreateCommand(Guid therapistId, List<string>? certificationTypeString = null)
    {
        return new ChangeTherapistInfoCommand(
            therapistId,
            "Jane Doe",
            750,
            "Nyvej 5",
            "6800",
            "Varde",
            "new@test.dk",
            "87654321",
            certificationTypeString ?? certificationType
            );
    }

    [Fact]
    public async Task GivenChangedValues_WhenChangingTherapistInfo_CallsSave()
    {
        // Arrange
        var therapist = CreateTherapist();

        var mockTherapistRepo = new Mock<ITherapistRepository>();

        mockTherapistRepo
        .Setup(x => x.GetByIdAsync(therapist.Id))
        .ReturnsAsync(therapist);

        var command = CreateCommand(therapist.Id);

        var handler = new ChangeTherapistInfoHandler(mockTherapistRepo.Object) as IChangeTherapistInfoHandler;

        // Act
        await handler.Handle(command);

        // Assert
        Assert.Equal(command.Name, therapist.Name);

        Assert.Equal(command.HourlyRate, therapist.HourlyRate);

        Assert.Equal(command.EmailAddress, therapist.Email.EmailAddress);

        Assert.Equal(command.PhoneNumber, therapist.PhoneNumber.Number);

        Assert.Contains(CertificationTypes.Physiotherapy, therapist.CertificationTypes);

        Assert.Contains(CertificationTypes.Acupuncture, therapist.CertificationTypes);

        Assert.DoesNotContain(CertificationTypes.Dietary, therapist.CertificationTypes);

        mockTherapistRepo.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task GivenInvalidCertification_WhenChangingTherapistInfo_CastNotFoundException()
    {
        // Arrange
        var therapist = CreateTherapist();

        var mockTherapistRepo = new Mock<ITherapistRepository>();

        mockTherapistRepo
        .Setup(x => x.GetByIdAsync(therapist.Id))
        .ReturnsAsync(therapist);

        var command = CreateCommand(therapist.Id, new List<string> { "InvalidCertification" });

        var handler = new ChangeTherapistInfoHandler(mockTherapistRepo.Object) as IChangeTherapistInfoHandler;

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));
    }


    [Fact]
    public async Task GivenNoChanges_WhenChangingTherapistInfo_NeverCallsSave()
    {
        // Arrange
        var therapist = CreateTherapist();

        var mockTherapistRepo = new Mock<ITherapistRepository>();

        mockTherapistRepo
        .Setup(x => x.GetByIdAsync(therapist.Id))
        .ReturnsAsync(therapist);

        var command = new ChangeTherapistInfoCommand(
        therapist.Id,
        therapist.Name,
        therapist.HourlyRate,
        therapist.Address.Street,
        therapist.Address.PostalCode,
        therapist.Address.City,
        therapist.Email.EmailAddress,
        therapist.PhoneNumber.Number,
        therapist.CertificationTypes
        .Select(x => x.ToString())
        .ToList());

        var handler = new ChangeTherapistInfoHandler(mockTherapistRepo.Object) as IChangeTherapistInfoHandler;

        await handler.Handle(command);

        mockTherapistRepo.Verify(r => r.SaveAsync(), Times.Never);
    }
}