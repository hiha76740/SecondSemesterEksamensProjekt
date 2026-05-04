using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Entities.Therapists;

public class Therapist : AggregateRoot
{
    public string AuthorizationNumber { get; init; }
    public string Name { get; private set; }
    public decimal HourlyRate { get; private set; }

    public Address Address { get; private set; }
    public Email Email { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }

    private readonly List<CertificationTypes> _certificationTypes = new();
    public IReadOnlyCollection<CertificationTypes> CertificationTypes => _certificationTypes.AsReadOnly();

    private Therapist(string authorizationNumber, string name, decimal hourlyRate, Address address, Email email, PhoneNumber phoneNumber)
    {
        // Pre-condition

        if (string.IsNullOrWhiteSpace(authorizationNumber))
            throw new DomainException("Therapist must have an authorization number");

        EnsureValidName(name);

        EnsureValidHourlyRate(hourlyRate);


        // Action

        Id = Guid.NewGuid();

        AuthorizationNumber = authorizationNumber;
        Name = name;
        HourlyRate = hourlyRate;

        Address = address;
        Email = email;
        PhoneNumber = phoneNumber;
    }


    public static Therapist Create(string authorizationNumber, string name, decimal hourlyRate, Address address, Email email, PhoneNumber phoneNumber)
        => new(
            authorizationNumber,
            name,
            hourlyRate,
            address,
            email,
            phoneNumber);


    public void ChangeName(string newName)
    {
        // Pre-condition
        EnsureValidName(newName);
        

        if (Name == newName)
            throw new DomainException("New name cannot be the same as current name");


        // Action

        Name = newName;
    }


    public void ChangeHourlyRate(decimal newHourlyRate)
    {
        // Pre-condition

        EnsureValidHourlyRate(newHourlyRate);

        if (HourlyRate == newHourlyRate)
            throw new DomainException("New hourly rate cannot be the same as current hourly rate");


        // Action

        HourlyRate = newHourlyRate;
    }


    public void ChangeAddress(Address newAddress)
    {
        // Pre-condition

        if (Address == newAddress)
            throw new DomainException("New address cannot be the same as current address");


        // Action

        Address = newAddress;
    }


    public void ChangeEmail(Email newEmail)
    {
        // Pre-condition

        if (Email == newEmail)
            throw new DomainException("New email cannot be the same as current email");


        // Action

        Email = newEmail;
    }


    public void ChangePhoneNumber(PhoneNumber newPhoneNumber)
    {
        // Pre-condition

        if (PhoneNumber == newPhoneNumber)
            throw new DomainException("New phonenumber cannot be the same as current phonenumber");


        // Action

        PhoneNumber = newPhoneNumber;
    }


    public void AddCertificationType(CertificationTypes certificationType)
    {
        // Pre-condition

        if (_certificationTypes.Contains(certificationType) == true)
            throw new DomainException("Therapist already has this certification type");


        // Action

        _certificationTypes.Add(certificationType);
    }


    public void RemoveCertificationType(CertificationTypes certificationType)
    {
        // Pre-condition

        if (_certificationTypes.Contains(certificationType) == false)
            throw new DomainException("Therapist does not have this certification type");


        // Action

        _certificationTypes.Remove(certificationType);
    }

    private void EnsureValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Therapist must have a name");
    }

    private void EnsureValidHourlyRate(decimal hourlyRate)
    {
        if (hourlyRate <= 0)
            throw new DomainException("Hourly rate cannot be negative");
    }
}