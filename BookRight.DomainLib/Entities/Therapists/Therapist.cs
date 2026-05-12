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

    private readonly List<Guid> _associatedclinics = new();
    public IReadOnlyCollection<Guid> AssociatedClinics => _associatedclinics.AsReadOnly();

    private Therapist(string authorizationNumber, string name, decimal hourlyRate, Address address, Email email, PhoneNumber phoneNumber, List<Guid> associatedClinics, List<CertificationTypes>? certifications)
    {
        // Pre-condition
        if (associatedClinics.Count == 0)
            throw new DomainException("Therapist must be associated to atleast 1 clinic");

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
        _associatedclinics = associatedClinics;

        if (certifications != null)
            _certificationTypes = certifications;
    }


    public static Therapist Create(string authorizationNumber, string name, decimal hourlyRate, Address address, Email email, PhoneNumber phoneNumber, List<Guid> associatedClinics, List<CertificationTypes>? certifications = null)
        => new(
            authorizationNumber,
            name,
            hourlyRate,
            address,
            email,
            phoneNumber,
            associatedClinics,
            certifications);


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

    public void AddAssociatedClinic(Guid clinicId)
    {
        if (_associatedclinics.Contains(clinicId) == true)
            throw new DomainException($"Clinic is already associated with therapist {Name}");

        _associatedclinics.Add(clinicId);
    }

    public void RemoveAssociatedClinic(Guid clinicId)
    {
        if (_associatedclinics.Contains(clinicId) == false)
            throw new DomainException($"Clinic is not associated with therapist {Name}");

        if (_associatedclinics.Count == 1)
            throw new DomainException("Therapist must be associated to atleast 1 clinic");

        _associatedclinics.Remove(clinicId);
    }

    private static void EnsureValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Therapist must have a name");
    }

    private static void EnsureValidHourlyRate(decimal hourlyRate)
    {
        if (hourlyRate <= 0)
            throw new DomainException("Hourly rate cannot be negative or 0");
    }


    // EF Constructor
    private Therapist() { }

}