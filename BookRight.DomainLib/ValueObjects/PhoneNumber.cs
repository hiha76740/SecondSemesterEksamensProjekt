using BookRight.DomainLib.Exceptions;

namespace BookRight.DomainLib.ValueObjects;

public record PhoneNumber
{
    public string Number { get; init; }


    public PhoneNumber (string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new DomainException("Phonenumber is required");

        string normalisedNumber = number
            .Trim()
            .Replace(" ", "");

        if (normalisedNumber.StartsWith("+45"))
            normalisedNumber = normalisedNumber[3..];

        if (normalisedNumber.StartsWith("0045"))
            normalisedNumber = normalisedNumber[4..];

        if (normalisedNumber.Length != 8)
            throw new DomainException("Phonenumber must contain 8 digits");


        Number = normalisedNumber;
    }

}
