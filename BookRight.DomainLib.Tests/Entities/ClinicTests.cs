using BookRight.DomainLib.Entities.Clinics;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Tests.Entities;

public class ClinicTests
{
    private readonly static List<OpeningHourInput> OpeningHours = new()
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


    private static string Name => "Klinik Vejle";
    private static int TreatmentRoomLimit => 5;
    private static Address Address => new("Testvej 1", "1234", "FantasiBy");

    private static Clinic CreateWithValidData(
        string? name = null,
        int? treatmentRoomLimit = null,
        List<OpeningHourInput>? openingHours = null,
        Address? address = null)
        => Clinic.Create(
            name ?? Name,
            treatmentRoomLimit ?? TreatmentRoomLimit,
            openingHours ?? OpeningHours,
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

    // ---------------------------------------------------------
    // 2. ChangeOpeningHours tests (Changing a Clinic Opening hours)
    // ---------------------------------------------------------

    [Fact]
    public void ChangeOpeningHours_GivenValidData_ShallSucceed()
    {
        // Arrange
        var c = CreateWithValidData();
        var openingHour = c.OpeningHours.Where(oh => oh.WeekDay == WeekDays.Friday).First();
        var expected = true;


        var input = new OpeningHourInput(WeekDays.Friday, null, null, true);

        // Act
        c.ChangeOpeningHour(openingHour.Id,input);

        // Assert
        Assert.Equal(expected, openingHour.IsClosed);
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
}
