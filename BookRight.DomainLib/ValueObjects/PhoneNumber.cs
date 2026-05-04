using BookRight.DomainLib.Exceptions;

namespace BookRight.DomainLib.ValueObjects;

public record PhoneNumber
{
    public string Number { get; init; }


    public PhoneNumber (string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new DomainException("Phonenumber is required");

        if (number.All(char.IsDigit) == false)
            throw new DomainException("Phonenumber must contain only digits.");

        if (number.Length != 8)
            throw new DomainException("Phonenumber must contain 8 digits");

        Number = number;
    }

}
