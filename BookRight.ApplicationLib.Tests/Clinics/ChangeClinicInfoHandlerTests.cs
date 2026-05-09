using BookRight.ApplicationLib.Handlers.Clinics;
using BookRight.ApplicationLib.Repositories;
using BookRight.DomainLib.Entities.Clinics;
using BookRight.DomainLib.ValueObjects;
using BookRight.FacadeLib.Commands.Clinics.DTOs;
using BookRight.FacadeLib.Commands.Clinics.Interfaces;
using Moq;

namespace BookRight.ApplicationLib.Tests.Clinics;

public class ChangeClinicInfoHandlerTests
{
    private static Clinic CreateClinic()
    {
        DateTime open = DateTime.UtcNow.AddHours(1);
        DateTime close = open.AddHours(8);

        return Clinic.Create(
            "Ny Klinik Vejle",
            5,
            new OpeningHours(open, close),
            new Address("Testgade 42", "7100", "Vejle"));
    }

    private static ChangeClinicInfoCommand CreateCommand(
        Guid clinicId,
        string? street = null, 
        string? postalCode = null,
        string? city = null, 
        int? treatmentRoomLimit = null,
        DateTime? open = null, 
        DateTime? close = null)
    {
        return new ChangeClinicInfoCommand(
            clinicId,
            street ?? "NyGade 41",
            postalCode ?? "7000",
            city ?? "Fredericia",
            treatmentRoomLimit ?? 10,
            open ?? DateTime.UtcNow.AddDays(7),
            close ?? DateTime.UtcNow.AddDays(7).AddHours(8)
            );
    }

    [Fact]
    public void Handle_GivenDifferentInfo_CallsSave()
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
        handler.Handle(command);

        // Assert
        mockClinicRepo.Verify(r => r.SaveAsync(), Times.Once);
    }


    [Fact]
    public void Handle_GivenSameInfo_NeverCallsSave()
    {
        // Arrange
        var clinic = CreateClinic();

        var mockClinicRepo = new Mock<IClinicRepository>();

        mockClinicRepo
            .Setup(r => r.GetByIdAsync(clinic.Id))
            .ReturnsAsync(clinic);

        var handler = new ChangeClinicInfoHandler(mockClinicRepo.Object) as IChangeClinicInfoHandler;

        var command = CreateCommand(
            clinic.Id,
            clinic.Address.Street,
            clinic.Address.PostalCode,
            clinic.Address.City,
            clinic.TreatmentRoomLimit,
            clinic.OpeningHours.Open,
            clinic.OpeningHours.Close
            );

        // Act
        handler.Handle(command);

        // Assert
        mockClinicRepo.Verify(r => r.SaveAsync(), Times.Never);
    }
}


/*
 * feature-DevOps-324-ChangeClinicInfoHandlerTests-Added-Tests-For-GivenDifferentInfo
 * */