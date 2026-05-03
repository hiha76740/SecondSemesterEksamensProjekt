using BookRight.DomainLib.Exceptions;

namespace BookRight.DomainLib.ValueObjects;

public record Email
{
    public string EmailAddress { get; init;  }


    public Email (string emailAddress)
    {
        if (string.IsNullOrWhiteSpace(emailAddress))
            throw new DomainException("Email is required");

        if (emailAddress.Count(c => c == '@') != 1)
            throw new DomainException("Email must contain exactly one @");

        EmailAddress = emailAddress;
    }
}
