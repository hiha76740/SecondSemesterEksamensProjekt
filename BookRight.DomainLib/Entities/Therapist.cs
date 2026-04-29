using BookRight.DomainLib.ValueObjects;
using BookRight.DomainLib.Exceptions;

namespace BookRight.DomainLib.Entities.Therapists;

public class Therapist : AggregateRoot
{
    public Guid TherapistGuid { get; private set; }

    public string AuthorizationNumber { get; private set; }
    public string Name { get; private set; }
    public decimal HourlyRate { get; private set; }

    public Address Address { get; private set; }
    public Email Email { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }


    private Therapist(
        string authorizationNumber,
        string name,
        decimal hourlyRate,
        Address address,
        Email email,
        PhoneNumber phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(authorizationNumber))
            throw new DomainException("Therapist must have an authorization number");

        string normalisedAuthorizationNumber = authorizationNumber.Trim();


        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Therapist must have a name");

        string normalisedName = name.Trim();


        if (hourlyRate < 0)
            throw new DomainException("Hourly rate cannot be negative");


        Id = Guid.NewGuid();
        TherapistGuid = Guid.NewGuid();

        AuthorizationNumber = normalisedAuthorizationNumber;
        Name = normalisedName;
        HourlyRate = hourlyRate;

        Address = address ?? throw new DomainException("Therapist must have an address");
        Email = email ?? throw new DomainException("Therapist must have an email");
        PhoneNumber = phoneNumber ?? throw new DomainException("Therapist must have a phonenumber");
    }


    public static Therapist Create(
        string authorizationNumber,
        string name,
        decimal hourlyRate,
        Address address,
        Email email,
        PhoneNumber phoneNumber)
    {
        var therapist = new Therapist(
            authorizationNumber,
            name,
            hourlyRate,
            address,
            email,
            phoneNumber);

        return therapist;
    }


    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Therapist must have a name");

        string normalisedName = name.Trim();

        if (Name != normalisedName)
            Name = normalisedName;
    }


    public void ChangeAuthorizationNumber(string authorizationNumber)
    {
        if (string.IsNullOrWhiteSpace(authorizationNumber))
            throw new DomainException("Therapist must have an authorization number");

        string normalisedAuthorizationNumber = authorizationNumber.Trim();

        if (AuthorizationNumber != normalisedAuthorizationNumber)
            AuthorizationNumber = normalisedAuthorizationNumber;
    }


    public void ChangeHourlyRate(decimal hourlyRate)
    {
        if (hourlyRate < 0)
            throw new DomainException("Hourly rate cannot be negative");

        if (HourlyRate != hourlyRate)
            HourlyRate = hourlyRate;
    }


    public void ChangeAddress(Address address)
    {
        if (address == null)
            throw new DomainException("Therapist must have an address");

        if (Address != address)
            Address = address;
    }


    public void ChangeEmail(Email email)
    {
        if (email == null)
            throw new DomainException("Therapist must have an email");

        if (Email != email)
            Email = email;
    }


    public void ChangePhoneNumber(PhoneNumber phoneNumber)
    {
        if (phoneNumber == null)
            throw new DomainException("Therapist must have a phonenumber");

        if (PhoneNumber != phoneNumber)
            PhoneNumber = phoneNumber;
    }
}