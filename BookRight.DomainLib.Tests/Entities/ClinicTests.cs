using BookRight.DomainLib.Entities.Clinics;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Tests.Entities;

public class ClinicTests
{
    /*
    private static string Name => "Klinik Vejle";
    private static int TreatmentRoomLimit => 5;

    private static OpeningHours OpeningHours(DateTime open, DateTime close) => new(open, close);
    private static Address Address => new("Testvej 1", "1234", "FantasiBy");

    private static Clinic CreateWithValidData(
        string? name = null,
        int? treatmentRoomLimit = null,
        OpeningHours? openingHours = null,
        Address? address = null)
        => Clinic.Create(
            name ?? Name,
            treatmentRoomLimit ?? TreatmentRoomLimit,
            openingHours ?? OpeningHours(DateTime.Now.AddDays(1), DateTime.Now.AddDays(1)),
            address ?? Address
            );
    // ---------------------------------------------------------
    // 1. Create tests (Creating a Clinic)
    // ---------------------------------------------------------

    [Fact]
    public void Create_GivenInvalidName_CastDomainException()
    {
        // Arrange
        string name = "";

        // Act & Assert
        Assert.Throws<DomainException>(() => CreateWithValidData(name: name));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Create_GivenInvalidTreatmentRoomLimit_CastDomainException(int roomLimit)
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => CreateWithValidData(treatmentRoomLimit: roomLimit));
    }

    [Fact]
    public void Create_GivenOpeningHoursInPast_CastDomainException()
    {
        // Arrange
        var open = new DateTime(2024, 01, 01, 8, 0, 0);
        var close = new DateTime(2024, 01, 01, 16, 0, 0);
        var openingHours = OpeningHours(open, close);

        // Act & Assert
        Assert.Throws<DomainException>(() => CreateWithValidData(openingHours: openingHours));
    }


    // ---------------------------------------------------------
    // 2. ChangeOpeningHours tests (Changing a Clinic Opening hours)
    // ---------------------------------------------------------

    [Fact]
    public void ChangeOpeningHours_GivenValidData_ShallSucceed()
    {
        // Arrange
        var c = CreateWithValidData();
        var open = new DateTime(2030, 01, 01, 8, 0, 0);
        var close = new DateTime(2030, 01, 01, 16, 0, 0);
        var expected = OpeningHours(open, close);

        // Act
        c.ChangeOpeningHours(expected);

        // Assert
        Assert.Equal(expected, c.OpeningHours);
    }

    // ---------------------------------------------------------
    // 3. ChangeOpeningHours tests (Changing a Clinic Address)
    // ---------------------------------------------------------

    [Fact]
    public void ChangeAddress_GivenSameAddress_CastDomainException()
    {
        // Arrange
        var c = CreateWithValidData();

        // Act & Assert
        Assert.Throws<DomainException>(() => c.ChangeAddress(Address));
    }
    */
}
