using BookRight.ApplicationLib.Handlers.Clinics;
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Clinics;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Clinics.DTOs;
using BookRight.FacadeLib.Commands.Clinics.Interfaces;
using BookRight.FacadeLib.DTO;
using Moq;

namespace BookRight.ApplicationLib.Tests.Clinics;

public class ChangeClinicInfoHandlerTests
{

    private static List<OpeningHourDTO> OpeningHoursDTOChanged = new()
    {
        new OpeningHourDTO(
                "Monday",
                new TimeOnly(9, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourDTO(
                "Tuesday",
                new TimeOnly(9, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourDTO(
                "Wednesday",
                new TimeOnly(9, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourDTO(
                "Thursday",
                new TimeOnly(8, 0, 0),
                new TimeOnly(15, 0, 0),
                false),

        new OpeningHourDTO(
                "Friday",
                null,
                null,
                true),

        new OpeningHourDTO(
                "Saturday",
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                true),

        new OpeningHourDTO(
                "Sunday",
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                true)

    };

    private static List<OpeningHourDTO> OpeningHoursDTO = new()
    {
        new OpeningHourDTO(
                "Monday",
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourDTO(
                "Tuesday",
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourDTO(
                "Wednesday",
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourDTO(
                "Thursday",
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourDTO(
                "Friday",
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourDTO(
                "Saturday",
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourDTO(
                "Sunday",
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false)

    };

    private static List<OpeningHourInput> OpeningHours = new()
    {
        new OpeningHourInput(
                WeekDays.Monday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourInput(
                WeekDays.Tuesday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourInput(
                WeekDays.Wednesday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourInput(
                WeekDays.Thursday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourInput(
                WeekDays.Friday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourInput(
                WeekDays.Saturday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false),

        new OpeningHourInput(
                WeekDays.Sunday,
                new TimeOnly(8, 0, 0),
                new TimeOnly(16, 0, 0),
                false)

    };

    private static Clinic CreateClinic()
    {
        return Clinic.Create(
            "Ny Klinik Vejle",
            5,
            OpeningHours,
            new Address("Testgade 42", "7100", "Vejle"));
    }

    private static ChangeClinicInfoCommand CreateCommand(
        Guid clinicId,
        string? street = null,
        string? postalCode = null,
        string? city = null,
        int? treatmentRoomLimit = null,
        List<OpeningHourDTO>? openingHoursDTO = null)
    {
        return new ChangeClinicInfoCommand(
            clinicId,
            street ?? "Testgade 42",
            postalCode ?? "7100",
            city ?? "Vejle",
            treatmentRoomLimit ?? 5,
            openingHoursDTO ?? OpeningHoursDTO
            );
    }

    [Fact]
    public async Task Handle_GivenDifferentInfo_CallsSave()
    {
        // Arrange
        var clinic = CreateClinic();

        var mockClinicRepo = new Mock<IClinicRepository>();

        mockClinicRepo
            .Setup(r => r.GetByIdAsync(clinic.Id))
            .ReturnsAsync(clinic);

        var handler = new ChangeClinicInfoHandler(mockClinicRepo.Object) as IChangeClinicInfoHandler;

        var command = CreateCommand(clinic.Id,
            street: "NyGade 41",
            postalCode: "7000",
            city: "Fredericia",
            treatmentRoomLimit: 10,
            openingHoursDTO: OpeningHoursDTOChanged);

        // Act
        await handler.Handle(command);

        // Assert
        mockClinicRepo.Verify(r => r.SaveAsync(), Times.Once);
    }


    [Fact]
    public async Task Handle_GivenSameInfo_NeverCallsSave()
    {
        // Arrange
        var clinic = CreateClinic();

        var mockClinicRepo = new Mock<IClinicRepository>();

        mockClinicRepo
            .Setup(r => r.GetByIdAsync(clinic.Id))
            .ReturnsAsync(clinic);

        var handler = new ChangeClinicInfoHandler(mockClinicRepo.Object) as IChangeClinicInfoHandler;

        var command = CreateCommand(clinic.Id);

        // Act
        await handler.Handle(command);

        // Assert
        mockClinicRepo.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenUnknownClinic_CastNotFoundException()
    {
        // Arrange
        var mockClinicRepo = new Mock<IClinicRepository>();

        mockClinicRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
             .ReturnsAsync((Clinic?)null);

        var handler = new ChangeClinicInfoHandler(mockClinicRepo.Object) as IChangeClinicInfoHandler;

        var command = CreateCommand(It.IsAny<Guid>());

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));
    }

}