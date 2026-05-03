using BookRight.DomainLib.Entities.Customers;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;
using System.Net;
using Xunit;

namespace BookRight.DomainLib.Tests;

public class CustomerTests
{
    private static string Firstname => "Poul";
    private static string Lastname => "Pedersen";
    private static DateTime Birthdate => new DateTime(1965, 6, 18);
    private static string Note => "Peanutbutter-allergy";
    private static Address Address => new Address("Test Avenue 21", "1234", "Testville");
    private static Email Email => new Email("PoulP@testmail.com");
    private static PhoneNumber PhoneNumber => new PhoneNumber("87654321");

    private static Customer CreateCustomerWithValidData(
        string? firstName = null,
        string? lastName = null,
        DateTime? birthDate = null,
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
    public void Create_WithInvalidFirstName_ThrowDomainException()
    {
        //Arrange
        string firstName = "";

        //Act & Assert
        Assert.Throws<DomainException>(() => CreateCustomerWithValidData(firstName: firstName));
    }

    [Fact]
    public void Create_WithInvalidLastName_ThrowDomainException()
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
        DateTime birthdate = new DateTime(2028, 08, 14);

        //Act & Assert
        Assert.Throws<DomainException>(() => CreateCustomerWithValidData(birthDate: birthdate));
    }


    // ---------------------------------------------------------
    // 2. ChangeFirstname tests (Change Customer Firstname)
    // ---------------------------------------------------------

    [Fact]
    public void ChangeFirstname_GivenValidData_ShouldSucceed()
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
    public void ChangeFirstname_GivenEmptyFirstname_ThrowDomainException()
    {
        // Arrange
        var c = CreateCustomerWithValidData();
        string newFirstname = "";

        // Act & Assert
        Assert.Throws<DomainException>(() => c.ChangeFirstname(newFirstname));
    }


    // ---------------------------------------------------------
    // 3. ChangeLastname tests (Change Customer Lastname)
    // ---------------------------------------------------------

    [Fact]
    public void ChangeLastname_GivenValidData_ShouldSucceed()
    {
        // Arrange
        var c = CreateCustomerWithValidData();
        string newLastname = "Johnson";

        // Act
        c.ChangeLastname(newLastname);

        // Assert
        Assert.Equal(newLastname, c.Lastname);
    }

    [Fact]
    public void ChangeLastname_GivenEmptyLastname_ThrowDomainException()
    {
        // Arrange
        var c = CreateCustomerWithValidData();
        string newLastname = "";

        // Act & Assert
        Assert.Throws<DomainException>(() => c.ChangeLastname(newLastname));
    }

    //Arrange

    //Act

    //Assert
}
