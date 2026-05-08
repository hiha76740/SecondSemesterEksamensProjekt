using BookRight.DomainLib.Entities.Therapists;
using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Tests.Entities;

public class TherapistTests
{
  
    // PRIVATE STATIC FIELDS


    private static string Name => "John Doe";
    private static string AuthorizationNumber => "AUTH123";
    private static decimal HourlyRate => 550;
    private static Guid AssociatedClinicId => Guid.Parse("d62f5c2d-a5e6-4523-902d-108acac956c8");
    private static List<Guid> AssociatedClinicIds => new() { AssociatedClinicId };
    private static CertificationTypes Certification => CertificationTypes.Acupuncture;
    private static List<CertificationTypes> Certifications => new() { Certification };

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
        Email? email = null,
        List<CertificationTypes>? certificationTypes = null,
        List<Guid>? associatedClinicIds = null)
        => Therapist.Create(
            authorizationNumber ?? AuthorizationNumber,
            name ?? Name,
            hourlyRate ?? HourlyRate,
            address ?? Address(),
            email ?? Email(),
            phoneNumber ?? PhoneNumber(),
            associatedClinicIds ?? AssociatedClinicIds,
            certificationTypes
            );


    // ---------------------------------------------------------
    // 1. Create tests (Creating a Therapist)
    // ---------------------------------------------------------

    [Theory]
    [InlineData("")]
    [InlineData(" ")]

    public void Create_GivenEmptyName_CastDomainException(string name)
    {
        // Act + Assert
        Assert.Throws<DomainException>(() =>
            CreateWithValidData(name: name));
    }


    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_GivenEmptyAuthorizationNumber_CastDomainException(string authorizationNumber)
    {
        // Act + Assert
        Assert.Throws<DomainException>(() =>
            CreateWithValidData(authorizationNumber: authorizationNumber));
    }


    [Fact]
    public void Create_GivenNegativeHourlyRate_CastDomainException()
    {
        decimal hourlyRate = -1;

        // Act + Assert
        Assert.Throws<DomainException>(() =>
            CreateWithValidData(hourlyRate: hourlyRate));
    }


    // ---------------------------------------------------------
    // 2. ChangeName tests (Changing a Therapist name) 
    // ---------------------------------------------------------


    [Fact]
    public void ChangeName_GivenValidName_ChangesName()
    {
        // Arrange
        var therapist = CreateWithValidData();
        string newName = "Jane Doe";

        // Act
        therapist.ChangeName(newName);

        // Assert
        Assert.Equal(newName, therapist.Name);
    }


    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void ChangeName_GivenEmptyName_CastDomainException(string name)
    {
        // Arrange
        var therapist = CreateWithValidData();

        // Act + Assert
        Assert.Throws<DomainException>(() =>
            therapist.ChangeName(name));
    }


    // ---------------------------------------------------------
    // 3. ChangeHourlyRate tests (Changing a Therapist hourly rate) 
    // ---------------------------------------------------------

    [Fact]
    public void ChangeHourlyRate_GivenValidHourlyRate_ChangesHourlyRate()
    {
        // Arrange
        var therapist = CreateWithValidData();
        decimal newHourlyRate = 750;

        // Act
        therapist.ChangeHourlyRate(newHourlyRate);

        // Assert
        Assert.Equal(newHourlyRate, therapist.HourlyRate);
    }


    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ChangeHourlyRate_GivenInvalidHourlyRate_CastDomainException(decimal newHourlyRate)
    {
        // Arrange
        var therapist = CreateWithValidData();

        // Act + Assert
        Assert.Throws<DomainException>(() =>
            therapist.ChangeHourlyRate(newHourlyRate));
    }


    // ---------------------------------------------------------
    // 4. ChangeAddress tests (Changing a Therapist Address) 
    // ---------------------------------------------------------

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


    // ---------------------------------------------------------
    // 5. ChangeEmail tests (Changing a Therapist Email) 
    // ---------------------------------------------------------

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


    // ---------------------------------------------------------
    // 6. ChangePhoneNumber tests (Changing a Therapist Phone Number) 
    // ---------------------------------------------------------

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

    // ---------------------------------------------------------
    // 8. AddCertificationType tests (Adding certifications to a Therapist) 
    // ---------------------------------------------------------

    [Fact]
    public void AddCertificationType_GivenAlreadyAddedCertificationType_CastDomainException()
    {
        // Arrange
        var therapist = CreateWithValidData(certificationTypes: Certifications);

        // Act & Assert
        Assert.Throws<DomainException>(() => therapist.AddCertificationType(Certification));
    }

    // ---------------------------------------------------------
    // 9. RemoveCertificationType tests (Removing certifications from a Therapist) 
    // ---------------------------------------------------------

    [Fact]
    public void RemoveCertificationType_GivenNotExistingCertificationType_CastDomainException()
    {
        // Arrange
        var therapist = CreateWithValidData();

        // Act & Assert
        Assert.Throws<DomainException>(() => therapist.RemoveCertificationType(Certification));
    }

    // ---------------------------------------------------------
    // 10. AddAssociatedClinic tests (Adding new associated clinic to a Therapist) 
    // ---------------------------------------------------------

    [Fact]
    public void AddAssociatedClinic_GivenAlreadyAddedAssociatedClinicType_CastDomainException()
    {
        // Arrange
        var therapist = CreateWithValidData();

        // Act & Assert
        Assert.Throws<DomainException>(() => therapist.AddAssociatedClinic(AssociatedClinicId));
    }

    // ---------------------------------------------------------
    // 11. RemoveAssociatedClinic tests (Removing associated clinic from a Therapist) 
    // ---------------------------------------------------------

    [Fact]
    public void RemoveAssociatedClinic_GivenNotExistingClinic_CastDomainException()
    {
        // Arrange
        var therapist = CreateWithValidData();

        therapist.RemoveAssociatedClinic(AssociatedClinicId);

        // Act & Assert
        Assert.Throws<DomainException>(() => therapist.RemoveAssociatedClinic(AssociatedClinicId));
    }
}
