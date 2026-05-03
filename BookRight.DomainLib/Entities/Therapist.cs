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

    private readonly List<CertificationType> _certificationTypes = [];
    public IReadOnlyCollection<CertificationType> CertificationTypes => _certificationTypes.AsReadOnly();


    private Therapist(string authorizationNumber, string name, decimal hourlyRate, Address address, Email email, PhoneNumber phoneNumber)
    {
        // Pre-condition

        if (string.IsNullOrWhiteSpace(authorizationNumber))
            throw new DomainException("Therapist must have an authorization number");

        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Therapist must have a name");

        if (hourlyRate < 0)
            throw new DomainException("Hourly rate cannot be negative");

        if (address == null)
            throw new DomainException("Therapist must have an address");

        if (email == null)
            throw new DomainException("Therapist must have an email");

        if (phoneNumber == null)
            throw new DomainException("Therapist must have a phonenumber");


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

        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("Therapist must have a name");

        if (Name == newName)
            throw new DomainException("New name cannot be the same as current name");


        // Action

        Name = newName;
    }


    public void ChangeHourlyRate(decimal newHourlyRate)
    {
        // Pre-condition

        if (newHourlyRate < 0)
            throw new DomainException("Hourly rate cannot be negative");

        if (HourlyRate == newHourlyRate)
            throw new DomainException("New hourly rate cannot be the same as current hourly rate");


        // Action

        HourlyRate = newHourlyRate;
    }


    public void ChangeAddress(Address newAddress)
    {
        // Pre-condition

        if (newAddress == null)
            throw new DomainException("Therapist must have an address");

        if (Address == newAddress)
            throw new DomainException("New address cannot be the same as current address");


        // Action

        Address = newAddress;
    }


    public void ChangeEmail(Email newEmail)
    {
        // Pre-condition

        if (newEmail == null)
            throw new DomainException("Therapist must have an email");

        if (Email == newEmail)
            throw new DomainException("New email cannot be the same as current email");


        // Action

        Email = newEmail;
    }


    public void ChangePhoneNumber(PhoneNumber newPhoneNumber)
    {
        // Pre-condition

        if (newPhoneNumber == null)
            throw new DomainException("Therapist must have a phonenumber");

        if (PhoneNumber == newPhoneNumber)
            throw new DomainException("New phonenumber cannot be the same as current phonenumber");


        // Action

        PhoneNumber = newPhoneNumber;
    }


    public void AddCertificationType(CertificationType certificationType)
    {
        // Pre-condition

        if (certificationType == null)
            throw new DomainException("Certification type cannot be null");

        if (_certificationTypes.Contains(certificationType))
            throw new DomainException("Therapist already has this certification type");


        // Action

        _certificationTypes.Add(certificationType);
    }


    public void RemoveCertificationType(CertificationType certificationType)
    {
        // Pre-condition

        if (certificationType == null)
            throw new DomainException("Certification type cannot be null");

        if (!_certificationTypes.Contains(certificationType))
            throw new DomainException("Therapist does not have this certification type");


        // Action

        _certificationTypes.Remove(certificationType);
    }
}