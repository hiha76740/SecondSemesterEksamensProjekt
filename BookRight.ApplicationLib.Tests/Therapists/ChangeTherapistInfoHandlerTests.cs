using BookRight.ApplicationLib.Handlers.Therapists;
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Clinics;
using BookRight.DomainLib.Entities.Therapists;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Therapists.DTOs;
using BookRight.FacadeLib.Commands.Therapists.Interfaces;
using Moq;

namespace BookRight.ApplicationLib.Tests.Therapists;

public class ChangeTherapistInfoHandlerTests
{
    private readonly static List<string> CertificationType = new() { CertificationTypes.Physiotherapy.ToString(), CertificationTypes.Acupuncture.ToString() };

    private static Guid ClinicId => Guid.NewGuid();
    private static List<Guid> AssociatedClinics => new() { Guid.Parse("4504e50a-67a5-4cba-b029-8eb0b493c80d") };

    private static Clinic CreateClinic()
    {
        DateTime openingHour = new DateTime(2030, 5, 1, 8, 0, 0);
        return Clinic.Create(
            "Test Klinik 1",
            5,
            new OpeningHours(openingHour, openingHour.AddHours(8)),
            new Address("Testvej 21", "6000", "Kolding"));
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
        new List<Guid>() { ClinicId },
        new List<CertificationTypes> { CertificationTypes.Acupuncture, CertificationTypes.Dietary }
        );
    }
    private static ChangeTherapistInfoCommand CreateCommand(Guid therapistId, List<Guid>? associatedClinics = null, List<string>? certificationTypeString = null)
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
            associatedClinics ?? AssociatedClinics,
            certificationTypeString ?? CertificationType
            );
    }

    [Fact]
    public async Task Handle_GivenChangedValues_CallsSave()
    {
        // Arrange
        var therapist = CreateTherapist();
        var clinic = CreateClinic();

        var mockTherapistRepo = new Mock<ITherapistRepository>();
        var mockClinicRepo = new Mock<IClinicRepository>();

        mockTherapistRepo
            .Setup(x => x.GetByIdAsync(therapist.Id))
            .ReturnsAsync(therapist);

        mockClinicRepo
            .Setup(r => r.GetByIdAsync(clinic.Id))
            .ReturnsAsync(clinic);

        var command = CreateCommand(therapist.Id, associatedClinics: new List<Guid>() { clinic.Id });

        var handler = new ChangeTherapistInfoHandler(mockTherapistRepo.Object, mockClinicRepo.Object) as IChangeTherapistInfoHandler;

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

        Assert.Contains(clinic.Id, therapist.AssociatedClinics);

        Assert.DoesNotContain(ClinicId, therapist.AssociatedClinics);

        mockTherapistRepo.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenInvalidCertification_CastNotFoundException()
    {
        // Arrange
        var therapist = CreateTherapist();

        var mockTherapistRepo = new Mock<ITherapistRepository>();
        var mockClinicRepo = new Mock<IClinicRepository>();

        mockTherapistRepo
        .Setup(x => x.GetByIdAsync(therapist.Id))
        .ReturnsAsync(therapist);

        var command = CreateCommand(therapist.Id, certificationTypeString: new List<string> { "InvalidCertification" });

        var handler = new ChangeTherapistInfoHandler(mockTherapistRepo.Object, mockClinicRepo.Object) as IChangeTherapistInfoHandler;

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));
    }


    [Fact]
    public async Task Handle_GivenNoChanges_NeverCallsSave()
    {
        // Arrange
        var therapist = CreateTherapist();
        var clinic = CreateClinic();

        var mockTherapistRepo = new Mock<ITherapistRepository>();
        var mockClinicRepo = new Mock<IClinicRepository>();

        mockTherapistRepo
            .Setup(x => x.GetByIdAsync(therapist.Id))
            .ReturnsAsync(therapist);

        mockClinicRepo
            .Setup(r => r.GetByIdAsync(clinic.Id))
            .ReturnsAsync(clinic);

        var command = new ChangeTherapistInfoCommand(
        therapist.Id,
        therapist.Name,
        therapist.HourlyRate,
        therapist.Address.Street,
        therapist.Address.PostalCode,
        therapist.Address.City,
        therapist.Email.EmailAddress,
        therapist.PhoneNumber.Number,
        therapist.AssociatedClinics.ToList(),
        therapist.CertificationTypes
        .Select(x => x.ToString())
        .ToList());

        var handler = new ChangeTherapistInfoHandler(mockTherapistRepo.Object, mockClinicRepo.Object) as IChangeTherapistInfoHandler;

        await handler.Handle(command);

        mockTherapistRepo.Verify(r => r.SaveAsync(), Times.Never);
    }
}