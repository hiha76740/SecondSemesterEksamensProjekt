using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Tests.ValueObjects;

public class AddressTests
{
    private static string Street => "Test Allé";
    private static string PostalCode => "8000";
    private static string City => "Aarhus C";

    private static Address CreateAddressWithValidData(
        string? street = null,
        string? postalCode = null,
        string? city = null)
        => new Address(
            street ?? Street,
            postalCode ?? PostalCode,
            city ?? City);

    // ---------------------------------------------------------
    // 1. Create tests (Creating an Address)
    // ---------------------------------------------------------

    [Fact]
    public void Create_WithEmptyStreet_CastDomainException()
    {
        //Arrange
        string street = "";

        //Act & Assert
        Assert.Throws<DomainException>(() => CreateAddressWithValidData(street: street));
    }

    [Fact]
    public void Create_WithEmptyPostalCode_CastDomainException()
    {
        //Arrange
        string postalCode = "";

        //Act & Assert
        Assert.Throws<DomainException>(() => CreateAddressWithValidData(postalCode: postalCode));
    }

    [Fact]
    public void Create_WithEmptyCity_CastDomainException()
    {
        //Arrange
        string city = "";

        //Act & Assert
        Assert.Throws<DomainException>(() => CreateAddressWithValidData(city: city));
    }
}
