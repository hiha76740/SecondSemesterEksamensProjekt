using BookRight.DomainLib.Exceptions;

namespace BookRight.DomainLib.ValueObjects;

public record Email
{
    public string EmailAddress { get; init;  }


    public Email (string emailAddress)
    {
        if (string.IsNullOrWhiteSpace(emailAddress))
            throw new DomainException("Email is required");
        
        string normalisedEmailAddress = emailAddress.Trim();

        if (normalisedEmailAddress.Contains('@') == false || normalisedEmailAddress[^2].Equals('.') == false)
            throw new DomainException("Email must have a domain");


        EmailAddress = normalisedEmailAddress;
    }
}
