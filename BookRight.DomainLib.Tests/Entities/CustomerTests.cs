using BookRight.DomainLib.Entities.Customers;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Tests.Entities;

public class CustomerTests
{
    private static string Firstname => "Poul";
    private static string Lastname => "Pedersen";
    private static DateOnly Birthdate => new DateOnly(1965, 6, 18);
    private static string Note => "Peanutbutter-allergy";
    private static Address Address => new Address("Test Avenue 21", "1234", "Testville");
    private static Email Email => new Email("PoulP@testmail.com");
    private static PhoneNumber PhoneNumber => new PhoneNumber("87654321");

    private static Customer CreateCustomerWithValidData(
        string? firstName = null,
        string? lastName = null,
        DateOnly? birthDate = null,
        Address? address = null,
        Email? email = null,
        PhoneNumber? phoneNumber = null,
        Guid? therapistId = null)
        => Customer.Create(
            firstName ?? Firstname,
            lastName ?? Lastname,
            birthDate ?? Birthdate,
            address ?? Address,
            email ?? Email,
            phoneNumber ?? PhoneNumber,
            Note,
            therapistId);


    // ---------------------------------------------------------
    // 1. Create tests (Creating a Customer)
    // ---------------------------------------------------------

    [Fact]
    public void Create_WithInvalidFirstName_CastDomainException()
    {
        //Arrange
        string firstName = "";

        //Act & Assert
        Assert.Throws<DomainException>(() => CreateCustomerWithValidData(firstName: firstName));
    }

    [Fact]
    public void Create_WithInvalidLastName_CastDomainException()
    {
        //Arrange
        string lastName = "";

        //Act & Assert
        Assert.Throws<DomainException>(() => CreateCustomerWithValidData(lastName: lastName));
    }

    [Fact]
    public void Create_WithInvalidBirthdate_ThrowDomainException()
    {
        //Arrange
        DateOnly birthdate = new DateOnly(2028, 08, 14);

        //Act & Assert
        Assert.Throws<DomainException>(() => CreateCustomerWithValidData(birthDate: birthdate));
    }


    // ---------------------------------------------------------
    // 2. ChangeFirstname tests (Change Customer Firstname)
    // ---------------------------------------------------------

    [Fact]
    public void ChangeFirstname_GivenValidData_ShallSucceed()
    {
        //Arrange
        var c = CreateCustomerWithValidData();
        string newFirstname = "Ian";

        //Act
        c.ChangeFirstname(newFirstname);

        //Assert
        Assert.Equal(newFirstname, c.Firstname);
    }

    [Fact]
    public void ChangeFirstname_GivenEmptyFirstname_CastDomainException()
    {
        //Arrange
        var c = CreateCustomerWithValidData();
        string newFirstname = "";

        //Act & Assert
        Assert.Throws<DomainException>(() => c.ChangeFirstname(newFirstname));
    }


    // ---------------------------------------------------------
    // 3. ChangeLastname tests (Change Customer Lastname)
    // ---------------------------------------------------------

    [Fact]
    public void ChangeLastname_GivenValidData_ShouldSucceed()
    {
        //Arrange
        var c = CreateCustomerWithValidData();
        string newLastname = "Johnson";

        //Act
        c.ChangeLastname(newLastname);

        //Assert
        Assert.Equal(newLastname, c.Lastname);
    }

    [Fact]
    public void ChangeLastname_GivenEmptyLastname_CastDomainException()
    {
        //Arrange
        var c = CreateCustomerWithValidData();
        string newLastname = "";

        //Act & Assert
        Assert.Throws<DomainException>(() => c.ChangeLastname(newLastname));
    }


    // ---------------------------------------------------------
    // 4. ChangeAddress tests (Change Customer Address)
    // ---------------------------------------------------------

    [Fact]
    public void ChangeAddress_GivenValidData_ShallSucceed()
    {
        //Arrange
        var c = CreateCustomerWithValidData();
        var expected = new Address("Testgade 8", "8765", "Testrup");

        //Act
        c.ChangeAddress(expected);

        //Assert
        Assert.Equal(expected, c.Address);
    }

    [Fact]
    public void ChangeAddress_GivenSameAddress_CastDomainException()
    {
        //Arrange
        var c = CreateCustomerWithValidData();
        var address = new Address("Test Avenue 21", "1234", "Testville");

        //Act & Assert
        Assert.Throws<DomainException>(() => c.ChangeAddress(address));
    }


    // ---------------------------------------------------------
    // 5. ChangeEmail tests (Change Customer Email)
    // ---------------------------------------------------------

    [Fact]
    public void ChangeEmail_GivenValidData_ShallSucceed()
    {
        //Arrange
        var c = CreateCustomerWithValidData();
        var expected = new Email("mytestmail@testcloud.com");

        //Act
        c.ChangeEmail(expected);

        //Assert
        Assert.Equal(expected, c.Email);
    }

    [Fact]
    public void ChangeEmail_GivenSameEmail_CastDomainException()
    {
        //Arrange
        var c = CreateCustomerWithValidData();
        var email = new Email("PoulP@testmail.com");

        //Act & Assert
        Assert.Throws<DomainException>(() => c.ChangeEmail(email));
    }


    // ---------------------------------------------------------
    // 6. ChangePhoneNumber tests (Change Customer PhoneNumber)
    // ---------------------------------------------------------

    [Fact]
    public void ChangePhoneNumber_GivenValidData_ShallSucceed()
    {
        //Arrange
        var c = CreateCustomerWithValidData();
        var expected = new PhoneNumber("43218765");

        //Act
        c.ChangePhoneNumber(expected);

        //Assert
        Assert.Equal(expected, c.PhoneNumber);
    }

    [Fact]
    public void ChangePhoneNumber_GivenSamePhoneNumber_CastDomainException()
    {
        //Arrange
        var c = CreateCustomerWithValidData();
        var phoneNumber = new PhoneNumber("87654321");

        //Act & Assert
        Assert.Throws<DomainException>(() => c.ChangePhoneNumber(phoneNumber));
    }


    // ---------------------------------------------------------
    // 7. ChangeNote tests (Change Customer Note)
    // ---------------------------------------------------------

    [Fact]
    public void ChangeNote_GivenValidData_ShallSucceed()
    {
        //Arrange
        var c = CreateCustomerWithValidData();
        var expected = "This is a new note";

        //Act
        c.ChangeNote(expected);

        //Assert
        Assert.Equal(expected, c.Note);
    }


    // ---------------------------------------------------------
    // 8. ChangePreferredTherapist tests (Change Customer Preffered Therapist)
    // ---------------------------------------------------------

    [Fact]
    public void ChangePreferredTherapist_GivenValidData_ShallSucceed()
    {
        //Arrange
        var therapistId = Guid.NewGuid();
        var c = CreateCustomerWithValidData(therapistId: therapistId);
        var expected = Guid.NewGuid();

        //Act
        c.ChangePreferredTherapist(expected);

        //Assert
        Assert.Equal(expected, c.PreferredTherapist);
    }

    [Fact]
    public void ChangePreferredTherapist_GivenSameId_CastDomainException()
    {
        //Arrange
        var therapistId = Guid.NewGuid();
        var c = CreateCustomerWithValidData(therapistId: therapistId);

        //Act & Assert
        Assert.Throws<DomainException>(() => c.ChangePreferredTherapist(therapistId));
    }
}
