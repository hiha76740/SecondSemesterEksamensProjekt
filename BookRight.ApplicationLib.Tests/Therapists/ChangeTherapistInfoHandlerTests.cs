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
    private static List<OpeningHourInput> OpeningHours = new()
    {
        new OpeningHourInput(
                Weekdays.Monday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourInput(
                Weekdays.Tuesday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourInput(
                Weekdays.Wednesday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourInput(
                Weekdays.Thursday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourInput(
                Weekdays.Friday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourInput(
                Weekdays.Saturday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourInput(
                Weekdays.Sunday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false)

    };


    private readonly static List<string> CertificationType = new() { CertificationTypes.Physiotherapy.ToString(), CertificationTypes.Acupuncture.ToString() };
    private static List<Guid> AssociatedClinics => new() { Guid.Parse("4504e50a-67a5-4cba-b029-8eb0b493c80d"), Guid.Parse("4515e50a-67a5-4cba-b029-8eb0b493c80d") };

    private static Clinic CreateClinic()
    {
        return Clinic.Create(
            "Test Klinik 1",
            5,
            OpeningHours,
            new Address("Testvej 21", "6000", "Kolding"));
    }

    private static Therapist CreateTherapist()
    {
        return Therapist.Create(
        "AUTH123",
        "Jane Doe",
        750,
        new Address("Nyvej 5", "6800", "Varde"),
        new Email("test@test.dk"),
        new PhoneNumber("12345678"),
        AssociatedClinics,
        new List<CertificationTypes> { CertificationTypes.Physiotherapy, CertificationTypes.Acupuncture }
        );
    }
    private static ChangeTherapistInfoCommand CreateCommand(
        Guid therapistId,
        string? name = null,
        decimal? hourlyRate = null,
        string? street = null,
        string? postalCode = null,
        string? city = null,
        string? email = null,
        string? phoneNumber = null,
        List<Guid>? associatedClinics = null,
        List<string>? certificationTypeString = null)
    {
        return new ChangeTherapistInfoCommand(
            therapistId,
            name ?? "Jane Doe",
            hourlyRate ?? 750,
            street ?? "Nyvej 5",
            postalCode ?? "6800",
            city ?? "Varde",
            email ?? "test@test.dk",
            phoneNumber ?? "12345678",
            associatedClinics ?? AssociatedClinics,
            certificationTypeString ?? CertificationType
            );
    }

    [Fact]
    public async Task Handle_GivenComandWithNewName_CallsSave()
    {
        // Arrange
        var therapist = CreateTherapist();
        var clinic = CreateClinic();

        var mockTherapistRepo = new Mock<ITherapistRepository>();
        var mockClinicRepo = new Mock<IClinicRepository>();

        mockTherapistRepo
            .Setup(r => r.GetByIdAsync(therapist.Id))
            .ReturnsAsync(therapist);

        mockClinicRepo
            .Setup(r => r.GetByIdAsync(clinic.Id))
            .ReturnsAsync(clinic);

        var command = CreateCommand(therapist.Id, name: "Susanne Madsen");

        var handler = new ChangeTherapistInfoHandler(mockTherapistRepo.Object, mockClinicRepo.Object) as IChangeTherapistInfoHandler;

        // Act
        await handler.Handle(command);

        // Assert
        mockTherapistRepo.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenCommandWithNewHourlyRate_CallsSave()
    {
        // Arrange
        var therapist = CreateTherapist();
        var clinic = CreateClinic();

        var mockTherapistRepo = new Mock<ITherapistRepository>();
        var mockClinicRepo = new Mock<IClinicRepository>();

        mockTherapistRepo
            .Setup(r => r.GetByIdAsync(therapist.Id))
            .ReturnsAsync(therapist);

        mockClinicRepo
            .Setup(r => r.GetByIdAsync(clinic.Id))
            .ReturnsAsync(clinic);

        var command = CreateCommand(therapist.Id, hourlyRate: 1000);

        var handler = new ChangeTherapistInfoHandler(mockTherapistRepo.Object, mockClinicRepo.Object) as IChangeTherapistInfoHandler;

        // Act
        await handler.Handle(command);

        // Assert
        mockTherapistRepo.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenCommandWithNewEmail_CallsSave()
    {
        // Arrange
        var therapist = CreateTherapist();
        var clinic = CreateClinic();

        var mockTherapistRepo = new Mock<ITherapistRepository>();
        var mockClinicRepo = new Mock<IClinicRepository>();

        mockTherapistRepo
            .Setup(r => r.GetByIdAsync(therapist.Id))
            .ReturnsAsync(therapist);

        mockClinicRepo
            .Setup(r => r.GetByIdAsync(clinic.Id))
            .ReturnsAsync(clinic);

        var command = CreateCommand(therapist.Id, email: "new@test.dk");

        var handler = new ChangeTherapistInfoHandler(mockTherapistRepo.Object, mockClinicRepo.Object) as IChangeTherapistInfoHandler;

        // Act
        await handler.Handle(command);

        // Assert

        mockTherapistRepo.Verify(r => r.SaveAsync(), Times.Once);
    }


    [Fact]
    public async Task Handle_GivenCommandWithNewPhoneNumber_CallsSave()
    {
        // Arrange
        var therapist = CreateTherapist();
        var clinic = CreateClinic();

        var mockTherapistRepo = new Mock<ITherapistRepository>();
        var mockClinicRepo = new Mock<IClinicRepository>();

        mockTherapistRepo
            .Setup(r => r.GetByIdAsync(therapist.Id))
            .ReturnsAsync(therapist);

        mockClinicRepo
            .Setup(r => r.GetByIdAsync(clinic.Id))
            .ReturnsAsync(clinic);

        var command = CreateCommand(therapist.Id, phoneNumber: "85245637");

        var handler = new ChangeTherapistInfoHandler(mockTherapistRepo.Object, mockClinicRepo.Object) as IChangeTherapistInfoHandler;

        // Act
        await handler.Handle(command);

        // Assert
        mockTherapistRepo.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenCommandAddCetification_CallsSave()
    {
        // Arrange
        var therapist = CreateTherapist();
        var clinic = CreateClinic();

        var mockTherapistRepo = new Mock<ITherapistRepository>();
        var mockClinicRepo = new Mock<IClinicRepository>();

        mockTherapistRepo
            .Setup(r => r.GetByIdAsync(therapist.Id))
            .ReturnsAsync(therapist);

        mockClinicRepo
            .Setup(r => r.GetByIdAsync(clinic.Id))
            .ReturnsAsync(clinic);

        var command = CreateCommand(
            therapist.Id,
            certificationTypeString: new List<string>()
            {
                CertificationTypes.Physiotherapy.ToString(),
                CertificationTypes.Acupuncture.ToString(),
                CertificationTypes.Message.ToString()
            });

        var handler = new ChangeTherapistInfoHandler(mockTherapistRepo.Object, mockClinicRepo.Object) as IChangeTherapistInfoHandler;

        // Act
        await handler.Handle(command);

        // Assert
        mockTherapistRepo.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenCommandRemoveCetification_CallsSave()
    {
        // Arrange
        var therapist = CreateTherapist();
        var clinic = CreateClinic();

        var mockTherapistRepo = new Mock<ITherapistRepository>();
        var mockClinicRepo = new Mock<IClinicRepository>();

        mockTherapistRepo
            .Setup(r => r.GetByIdAsync(therapist.Id))
            .ReturnsAsync(therapist);

        mockClinicRepo
            .Setup(r => r.GetByIdAsync(clinic.Id))
            .ReturnsAsync(clinic);

        var command = CreateCommand(
            therapist.Id,
            certificationTypeString: new List<string>()
            {
                CertificationTypes.Acupuncture.ToString()
            });

        var handler = new ChangeTherapistInfoHandler(mockTherapistRepo.Object, mockClinicRepo.Object) as IChangeTherapistInfoHandler;

        // Act
        await handler.Handle(command);

        // Assert
        mockTherapistRepo.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenCommandAddAssociatedClinic_CallsSave()
    {
        // Arrange
        var therapist = CreateTherapist();
        var clinic = CreateClinic();

        var mockTherapistRepo = new Mock<ITherapistRepository>();
        var mockClinicRepo = new Mock<IClinicRepository>();

        mockTherapistRepo
            .Setup(r => r.GetByIdAsync(therapist.Id))
            .ReturnsAsync(therapist);

        mockClinicRepo
            .Setup(r => r.GetByIdAsync(clinic.Id))
            .ReturnsAsync(clinic);

        var command = CreateCommand(
            therapist.Id,
            associatedClinics: new List<Guid>() { Guid.Parse("4504e50a-67a5-4cba-b029-8eb0b493c80d"), Guid.Parse("4504e52a-67a5-4cba-b029-8eb0b493c80d") });

        var handler = new ChangeTherapistInfoHandler(mockTherapistRepo.Object, mockClinicRepo.Object) as IChangeTherapistInfoHandler;

        // Act
        await handler.Handle(command);

        // Assert
        mockTherapistRepo.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenCommandRemoveAssociatedClinic_CallsSave()
    {
        // Arrange
        var therapist = CreateTherapist();
        var clinic = CreateClinic();

        var mockTherapistRepo = new Mock<ITherapistRepository>();
        var mockClinicRepo = new Mock<IClinicRepository>();

        mockTherapistRepo
            .Setup(r => r.GetByIdAsync(therapist.Id))
            .ReturnsAsync(therapist);

        mockClinicRepo
            .Setup(r => r.GetByIdAsync(clinic.Id))
            .ReturnsAsync(clinic);

        var command = CreateCommand(
            therapist.Id,
            associatedClinics: new List<Guid>() { Guid.Parse("4515e50a-67a5-4cba-b029-8eb0b493c80d") });

        var handler = new ChangeTherapistInfoHandler(mockTherapistRepo.Object, mockClinicRepo.Object) as IChangeTherapistInfoHandler;

        // Act
        await handler.Handle(command);

        // Assert
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
        .Setup(r => r.GetByIdAsync(therapist.Id))
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
            .Setup(r => r.GetByIdAsync(therapist.Id))
            .ReturnsAsync(therapist);

        mockClinicRepo
            .Setup(r => r.GetByIdAsync(clinic.Id))
            .ReturnsAsync(clinic);

        var command = CreateCommand(therapist.Id);

        var handler = new ChangeTherapistInfoHandler(mockTherapistRepo.Object, mockClinicRepo.Object) as IChangeTherapistInfoHandler;

        await handler.Handle(command);

        mockTherapistRepo.Verify(r => r.SaveAsync(), Times.Never);
    }
}