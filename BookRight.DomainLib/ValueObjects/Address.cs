using BookRight.DomainLib.Exceptions;

namespace BookRight.DomainLib.ValueObjects;

public record Address
{
    public string Street { get; init; }
    public string PostalCode { get; init; }
    public string City { get; init; }


    public Address (string street, string postalCode, string city)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new DomainException("Streetname is required");

        string normalisedStreet = street.Trim();


        if (string.IsNullOrWhiteSpace(postalCode))
            throw new DomainException("Postalcode is required");

        string normalisedPostalCode = postalCode.Trim();


        if (string.IsNullOrWhiteSpace(city))
            throw new DomainException("City is required");
        
        string normalisedCity = city.Trim();


        Street = normalisedStreet;
        PostalCode = normalisedPostalCode;
        City = normalisedCity;
    }
}
