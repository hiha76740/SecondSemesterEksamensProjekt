using BookRight.DomainLib.Enums;
using BookRight.DomainLib.Exceptions;
using BookRight.DomainLib.ValueObjects;

namespace BookRight.DomainLib.Entities.Therapists;

/// <summary>
/// Represents a therapist identified by an authorization number, with contact details, an hourly rate, certification
/// types, and one or more associated clinics.
/// </summary>
/// <remarks>Create instances via the Create factory method. Enforces domain invariants: nonempty authorization
/// number and name, hourly rate > 0, and at least one associated clinic. Exposes methods to change name, hourly rate,
/// address, email, and phone, and to add or remove certification types and associated clinics; operations throw
/// DomainException for invalid or duplicate changes. AuthorizationNumber is immutable after creation.</remarks>
public class Therapist : AggregateRoot
{
    public string AuthorizationNumber { get; init; } = null!;
    public string Name { get; private set; } = null!;
    public decimal HourlyRate { get; private set; }

    public Address Address { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public PhoneNumber PhoneNumber { get; private set; } = null!;

    private readonly List<CertificationTypes> _certificationTypes = new();
    public IReadOnlyCollection<CertificationTypes> CertificationTypes => _certificationTypes.AsReadOnly();

    private readonly List<Guid> _associatedClinics = new();
    public IReadOnlyCollection<Guid> AssociatedClinics => _associatedClinics.AsReadOnly();

    
    private Therapist(string authorizationNumber, string name, decimal hourlyRate, Address address, Email email, PhoneNumber phoneNumber, List<Guid> associatedClinics, List<CertificationTypes>? certifications)
    {
        if (associatedClinics.Count == 0)
            throw new DomainException("Therapist must be associated to atleast 1 clinic");

        if (string.IsNullOrWhiteSpace(authorizationNumber))
            throw new DomainException("Therapist must have an authorization number");

        EnsureValidName(name);
        EnsureValidHourlyRate(hourlyRate);


        Id = Guid.NewGuid();
        AuthorizationNumber = authorizationNumber;
        Name = name;
        HourlyRate = hourlyRate;
        Address = address;
        Email = email;
        PhoneNumber = phoneNumber;
        _associatedClinics = associatedClinics;

        if (certifications != null)
            _certificationTypes = certifications;
    }

    /// <summary>
    /// Creates a Therapist with the specified authorization number, name, hourly rate, contact details, associated
    /// clinics, and optional certifications.
    /// </summary>
    /// <param name="authorizationNumber">Professional authorization number assigned to the therapist.</param>
    /// <param name="name">Therapist's full name.</param>
    /// <param name="hourlyRate">Hourly billing rate.</param>
    /// <param name="address">Postal address of the therapist.</param>
    /// <param name="email">Electronic mail address for contact.</param>
    /// <param name="phoneNumber">Telephone contact number.</param>
    /// <param name="associatedClinics">Identifiers of clinics with which the therapist is associated.</param>
    /// <param name="certifications">Optional collection of certifications held by the therapist; null if none.</param>
    /// <returns>A new Therapist instance initialized with the provided values.</returns>
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

    /// <summary>
    /// Changes the Name property to the specified value.
    /// </summary>
    /// <remarks>Input is validated by EnsureValidName; that validation may throw for invalid
    /// values.</remarks>
    /// <param name="newName">The new name to assign. It must be valid and different from the current Name.</param>
    /// <exception cref="DomainException">Thrown when the specified name is the same as the current Name.</exception>
    public void ChangeName(string newName)
    {
        EnsureValidName(newName);

        if (Name == newName)
            throw new DomainException("New name cannot be the same as current name");

        Name = newName;
    }

    /// <summary>
    /// Sets the entity's hourly rate to a validated value.
    /// </summary>
    /// <remarks>Validation is performed by EnsureValidHourlyRate; the rate is updated only if different from
    /// the current value.</remarks>
    /// <param name="newHourlyRate">The new hourly rate to apply; must be valid according to domain rules.</param>
    /// <exception cref="DomainException">Thrown if the new hourly rate is equal to the current hourly rate.</exception>
    public void ChangeHourlyRate(decimal newHourlyRate)
    {
        EnsureValidHourlyRate(newHourlyRate);

        if (HourlyRate == newHourlyRate)
            throw new DomainException("New hourly rate cannot be the same as current hourly rate");

        HourlyRate = newHourlyRate;
    }

    /// <summary>
    /// Updates the current Address to the specified newAddress.
    /// </summary>
    /// <remarks>Performs an equality check and rejects identical addresses.</remarks>
    /// <param name="newAddress">The new Address to assign.</param>
    /// <exception cref="DomainException">Thrown when newAddress is equal to the current Address.</exception>
    public void ChangeAddress(Address newAddress)
    {
        if (Address == newAddress)
            throw new DomainException("New address cannot be the same as current address");

        Address = newAddress;
    }

    /// <summary>
    /// Updates the current Email to the specified new email.
    /// </summary>
    /// <remarks>Uses Email's equality implementation to determine sameness.</remarks>
    /// <param name="newEmail">The new email value.</param>
    /// <exception cref="DomainException">Thrown when the provided newEmail is equal to the current Email.</exception>
    public void ChangeEmail(Email newEmail)
    {
        if (Email == newEmail)
            throw new DomainException("New email cannot be the same as current email");

        Email = newEmail;
    }

    /// <summary>
    /// Updates the current phone number to the specified value.
    /// </summary>
    /// <remarks>Performs an equality check and assigns the new value only if different.</remarks>
    /// <param name="newPhoneNumber">The phone number to set.</param>
    /// <exception cref="DomainException">Thrown when the specified phone number is the same as the current phone number.</exception>
    public void ChangePhoneNumber(PhoneNumber newPhoneNumber)
    {
        if (PhoneNumber == newPhoneNumber)
            throw new DomainException("New phonenumber cannot be the same as current phonenumber");

        PhoneNumber = newPhoneNumber;
    }

    /// <summary>
    /// Adds the specified certification type to the therapist's collection.
    /// </summary>
    /// <remarks>Duplicates are not permitted.</remarks>
    /// <param name="certificationType">The certification type to add.</param>
    /// <exception cref="DomainException">Thrown when the therapist already has the specified certification type.</exception>
    public void AddCertificationType(CertificationTypes certificationType)
    {
        if (_certificationTypes.Contains(certificationType) == true)
            throw new DomainException("Therapist already has this certification type");

        _certificationTypes.Add(certificationType);
    }

    /// <summary>
    /// Removes the specified certification type from the therapist's certification collection.
    /// </summary>
    /// <remarks>Mutates the internal certification collection; callers should handle DomainException if the
    /// type is not present.</remarks>
    /// <param name="certificationType">The certification type to remove from the therapist's certifications.</param>
    /// <exception cref="DomainException">Thrown when the therapist does not have the specified certification type.</exception>
    public void RemoveCertificationType(CertificationTypes certificationType)
    {
        if (_certificationTypes.Contains(certificationType) == false)
            throw new DomainException("Therapist does not have this certification type");

        _certificationTypes.Remove(certificationType);
    }

    /// <summary>
    /// Associate the specified clinic with the therapist.
    /// </summary>
    /// <remarks>Adds the clinic identifier to the therapist's collection of associated clinics.</remarks>
    /// <param name="clinicId">The identifier of the clinic to associate.</param>
    /// <exception cref="DomainException">Thrown when the clinic is already associated with the therapist.</exception>
    public void AddAssociatedClinic(Guid clinicId)
    {
        if (_associatedClinics.Contains(clinicId) == true)
            throw new DomainException($"Clinic is already associated with therapist {Name}");

        _associatedClinics.Add(clinicId);
    }

    /// <summary>
    /// Removes the association to the clinic identified by clinicId.
    /// </summary>
    /// <param name="clinicId">The identifier of the clinic to remove.</param>
    /// <exception cref="DomainException">Thrown if the specified clinic is not associated with the therapist, or if removal would leave the therapist
    /// with no associated clinics.</exception>
    public void RemoveAssociatedClinic(Guid clinicId)
    {
        if (_associatedClinics.Contains(clinicId) == false)
            throw new DomainException($"Clinic is not associated with therapist {Name}");

        if (_associatedClinics.Count == 1)
            throw new DomainException("Therapist must be associated to atleast 1 clinic");

        _associatedClinics.Remove(clinicId);
    }

    /// <summary>
    /// Validates that the provided name is not null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="name">The therapist's name to validate.</param>
    /// <exception cref="DomainException">Thrown when name is null, empty, or consists only of white-space characters.</exception>
    private static void EnsureValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Therapist must have a name");
    }

    /// <summary>
    /// Validates that the hourly rate is greater than zero.
    /// </summary>
    /// <param name="hourlyRate">Hourly rate to validate; must be greater than zero.</param>
    /// <exception cref="DomainException">Thrown if the hourly rate is less than or equal to zero.</exception>
    private static void EnsureValidHourlyRate(decimal hourlyRate)
    {
        if (hourlyRate <= 0)
            throw new DomainException("Hourly rate cannot be negative or 0");
    }


    private Therapist() { }

}