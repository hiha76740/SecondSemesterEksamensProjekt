using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Tests.ValueObjects;

public class PhoneNumberTests
{
    private static string Number => "98235467";

    private static PhoneNumber CreatePhoneNumberWithValidData(string? number = null) => new PhoneNumber(number ?? Number);


    // ---------------------------------------------------------
    // 1. Create tests (Creating a PhoneNumber)
    // ---------------------------------------------------------

    [Theory]
    [InlineData("923759023")]
    [InlineData("9450375")]
    public void Create_WithInvalidNoOfDigits_CastDomainException(string phoneNumber)
    {
        //Act & Assert
        Assert.Throws<DomainException>(() => CreatePhoneNumberWithValidData(phoneNumber));
    }

    [Fact]
    public void Create_WithNonDigits_CastDomainException()
    {
        //Arrange:
        string phoneNumber = "1234567#";

        //Act & Assert
        Assert.Throws<DomainException>(() => CreatePhoneNumberWithValidData(phoneNumber));
    }
}
