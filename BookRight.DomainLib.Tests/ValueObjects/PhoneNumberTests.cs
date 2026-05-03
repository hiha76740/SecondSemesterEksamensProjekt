using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Tests.ValueObjects;

public class PhoneNumberTests
{
    private static string Number => "98235467";

    private static PhoneNumber CreatePhoneNumberWithValidData(string? number = null) => new PhoneNumber(number ?? Number);
}
