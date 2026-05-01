using BookRight.DomainLib.Entities.Therapists;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Tests.Entities.Therapists;

public class TherapistTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateTherapist()
    {
        // Arrange
        var address = new Address(
            "Testvej 1",
            "6700",
            "Esbjerg");

        var email = new Email("test@test.dk");

        var phoneNumber = new PhoneNumber("12345678");

        // Act
        var therapist = Therapist.Create(
            "AUTH123",
            "John Doe",
            550,
            address,
            email,
            phoneNumber);

        // Assert
        Assert.NotNull(therapist);

        Assert.Equal("AUTH123", therapist.AuthorizationNumber);
        Assert.Equal("John Doe", therapist.Name);
        Assert.Equal(550, therapist.HourlyRate);

        Assert.Equal(address, therapist.Address);
        Assert.Equal(email, therapist.Email);
        Assert.Equal(phoneNumber, therapist.PhoneNumber);

        Assert.NotEqual(Guid.Empty, therapist.Id);
        Assert.NotEqual(Guid.Empty, therapist.TherapistGuid);
    }


    [Fact]
    public void Create_WithoutName_ShouldThrowDomainException()
    {
        // Arrange
        var address = new Address(
            "Testvej 1",
            "6700",
            "Esbjerg");

        var email = new Email("test@test.dk");

        var phoneNumber = new PhoneNumber("12345678");

        // Act + Assert
        Assert.Throws<DomainException>(() =>
            Therapist.Create(
                "AUTH123",
                "",
                550,
                address,
                email,
                phoneNumber));
    }


    [Fact]
    public void Create_WithoutAuthorizationNumber_ShouldThrowDomainException()
    {
        // Arrange
        var address = new Address(
            "Testvej 1",
            "6700",
            "Esbjerg");

        var email = new Email("test@test.dk");

        var phoneNumber = new PhoneNumber("12345678");

        // Act + Assert
        Assert.Throws<DomainException>(() =>
            Therapist.Create(
                "",
                "John Doe",
                550,
                address,
                email,
                phoneNumber));
    }


    [Fact]
    public void Create_WithNegativeHourlyRate_ShouldThrowDomainException()
    {
        // Arrange
        var address = new Address(
            "Testvej 1",
            "6700",
            "Esbjerg");

        var email = new Email("test@test.dk");

        var phoneNumber = new PhoneNumber("12345678");

        // Act + Assert
        Assert.Throws<DomainException>(() =>
            Therapist.Create(
                "AUTH123",
                "John Doe",
                -1,
                address,
                email,
                phoneNumber));
    }


    [Fact]
    public void ChangeName_WithValidName_ShouldUpdateName()
    {
        // Arrange
        var therapist = CreateValidTherapist();

        // Act
        therapist.ChangeName("Jane Doe");

        // Assert
        Assert.Equal("Jane Doe", therapist.Name);
    }


    [Fact]
    public void ChangeAuthorizationNumber_WithValidAuthorizationNumber_ShouldUpdateAuthorizationNumber()
    {
        // Arrange
        var therapist = CreateValidTherapist();

        // Act
        therapist.ChangeAuthorizationNumber("AUTH999");

        // Assert
        Assert.Equal("AUTH999", therapist.AuthorizationNumber);
    }


    [Fact]
    public void ChangeHourlyRate_WithValidHourlyRate_ShouldUpdateHourlyRate()
    {
        // Arrange
        var therapist = CreateValidTherapist();

        // Act
        therapist.ChangeHourlyRate(750);

        // Assert
        Assert.Equal(750, therapist.HourlyRate);
    }


    [Fact]
    public void ChangeAddress_WithValidAddress_ShouldUpdateAddress()
    {
        // Arrange
        var therapist = CreateValidTherapist();

        var newAddress = new Address(
            "Nyvej 5",
            "6800",
            "Varde");

        // Act
        therapist.ChangeAddress(newAddress);

        // Assert
        Assert.Equal(newAddress, therapist.Address);
    }


    [Fact]
    public void ChangeEmail_WithValidEmail_ShouldUpdateEmail()
    {
        // Arrange
        var therapist = CreateValidTherapist();

        var newEmail = new Email("new@test.dk");

        // Act
        therapist.ChangeEmail(newEmail);

        // Assert
        Assert.Equal(newEmail, therapist.Email);
    }


    [Fact]
    public void ChangePhoneNumber_WithValidPhoneNumber_ShouldUpdatePhoneNumber()
    {
        // Arrange
        var therapist = CreateValidTherapist();

        var newPhoneNumber = new PhoneNumber("87654321");

        // Act
        therapist.ChangePhoneNumber(newPhoneNumber);

        // Assert
        Assert.Equal(newPhoneNumber, therapist.PhoneNumber);
    }


    private static Therapist CreateValidTherapist()
    {
        var address = new Address(
            "Testvej 1",
            "6700",
            "Esbjerg");

        var email = new Email("test@test.dk");

        var phoneNumber = new PhoneNumber("12345678");

        return Therapist.Create(
            "AUTH123",
            "John Doe",
            550,
            address,
            email,
            phoneNumber);
    }
}