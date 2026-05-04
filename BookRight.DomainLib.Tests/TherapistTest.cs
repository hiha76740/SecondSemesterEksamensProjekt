using BookRight.DomainLib.Entities.Therapists;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Tests.Entities.Therapists;

public class TherapistTests
{
  
    // PRIVATE STATIC FIELDS


    private static string Name => "John Doe";
    private static string AuthorizationNumber => "AUTH123";
    private static decimal HourlyRate => 550;

    private static Address Address(
        string street = "Testvej 1",
        string postalCode = "6700",
        string city = "Esbjerg")
        => new(street, postalCode, city);

    private static Email Email(
        string email = "test@test.dk")
        => new(email);

    private static PhoneNumber PhoneNumber(
        string number = "12345678")
        => new(number);


 
    // HELPER METHOD


    private static Therapist CreateWithValidData(
        string? name = null,
        string? authorizationNumber = null,
        decimal? hourlyRate = null,
        Address? address = null,
        PhoneNumber? phoneNumber = null,
        Email? email = null)
        => Therapist.Create(
            authorizationNumber ?? AuthorizationNumber,
            name ?? Name,
            hourlyRate ?? HourlyRate,
            address ?? Address(),
            email ?? Email(),
            phoneNumber ?? PhoneNumber());


    // CREATE VALIDATION TESTS
 

    [Theory]
    [InlineData("")]
    [InlineData(" ")]

    public void Create_GivenEmptyName_CastDomainException(string name)
    {
        // Act + Assert
        Assert.Throws<DomainException>(() =>
            CreateWithValidData(name: name));
    }


    [Fact]
    
    public void Create_GivenEmptyAuthorizationNumber_CastDomainException()
    {
        // Act + Assert
        Assert.Throws<DomainException>(() =>
            CreateWithValidData(authorizationNumber: ""));
    }


    [Fact]
    public void Create_GivenNegativeHourlyRate_CastDomainException()
    {
        // Act + Assert
        Assert.Throws<DomainException>(() =>
            CreateWithValidData(hourlyRate: -1));
    }


    // CHANGE NAME
   

    [Fact]
    public void ChangeName_GivenValidName_ChangesName()
    {
        // Arrange
        var therapist = CreateWithValidData();

        // Act
        therapist.ChangeName("Jane Doe");

        // Assert
        Assert.Equal("Jane Doe", therapist.Name);
    }


    [Fact]
    public void ChangeName_GivenEmptyName_CastDomainException()
    {
        // Arrange
        var therapist = CreateWithValidData();

        // Act + Assert
        Assert.Throws<DomainException>(() =>
            therapist.ChangeName(""));
    }


    // -----------------------------
    // CHANGE HOURLY RATE
    // -----------------------------

    [Fact]
    public void ChangeHourlyRate_GivenValidHourlyRate_ChangesHourlyRate()
    {
        // Arrange
        var therapist = CreateWithValidData();

        // Act
        therapist.ChangeHourlyRate(750);

        // Assert
        Assert.Equal(750, therapist.HourlyRate);
    }


    [Fact]
    public void ChangeHourlyRate_GivenNegativeHourlyRate_CastDomainException()
    {
        // Arrange
        var therapist = CreateWithValidData();

        // Act + Assert
        Assert.Throws<DomainException>(() =>
            therapist.ChangeHourlyRate(-1));
    }


    // -----------------------------
    // CHANGE ADDRESS
    // -----------------------------

    [Fact]
    public void ChangeAddress_GivenValidAddress_ChangesAddress()
    {
        // Arrange
        var therapist = CreateWithValidData();

        var newAddress = Address(
            "Nyvej 5",
            "6800",
            "Varde");

        // Act
        therapist.ChangeAddress(newAddress);

        // Assert
        Assert.Equal(newAddress, therapist.Address);
    }


    // -----------------------------
    // CHANGE EMAIL
    // -----------------------------

    [Fact]
    public void ChangeEmail_GivenValidEmail_ChangesEmail()
    {
        // Arrange
        var therapist = CreateWithValidData();

        var newEmail = Email("new@test.dk");

        // Act
        therapist.ChangeEmail(newEmail);

        // Assert
        Assert.Equal(newEmail, therapist.Email);
    }


    // -----------------------------
    // CHANGE PHONE NUMBER
    // -----------------------------

    [Fact]
    public void ChangePhoneNumber_GivenValidPhoneNumber_ChangesPhoneNumber()
    {
        // Arrange
        var therapist = CreateWithValidData();

        var newPhoneNumber = PhoneNumber("87654321");

        // Act
        therapist.ChangePhoneNumber(newPhoneNumber);

        // Assert
        Assert.Equal(newPhoneNumber, therapist.PhoneNumber);
    }
}