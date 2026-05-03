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


}
