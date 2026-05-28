using BookRight.DomainLib.ValueObjects;
using BookRight.DomainLib.Exceptions;

namespace BookRight.DomainLib.Entities.Customers;

/// <summary>
/// 
/// </summary>
public class Customer : AggregateRoot
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public DateOnly BirthDate { get; init; }
    public string Note { get; private set; } = null!;
    public Address Address { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public Guid? PreferredTherapistId { get; private set; }

    /// <summary>
    /// Initializes a new instance of Customer with the specified personal, contact, and preference data.
    /// </summary>
    /// <remarks>Performs domain validation and assigns a new Id. The constructor is private to ensure
    /// instances are created with valid state.</remarks>
    /// <param name="firstName">The customer's first name. Validated by domain rules.</param>
    /// <param name="lastName">The customer's last name. Validated by domain rules.</param>
    /// <param name="birthDate">The customer's date of birth. Cannot be a future date.</param>
    /// <param name="address">The customer's postal address.</param>
    /// <param name="email">The customer's email address.</param>
    /// <param name="phoneNumber">The customer's phone number.</param>
    /// <param name="note">Optional free-text note about the customer.</param>
    /// <param name="preferredTherapistId">Optional identifier of the customer's preferred therapist.</param>
    /// <exception cref="DomainException">Thrown when a name fails domain validation or when the birth date is in the future.</exception>

    private Customer(
            string firstName,
            string lastName,
            DateOnly birthDate,
            Address address,
            Email email,
            PhoneNumber phoneNumber,
            string note,
            Guid? preferredTherapistId)
    {
        EnsureValidFirstName(firstName);
        EnsureValidLastName(lastName);

        if (birthDate > DateOnly.FromDateTime(DateTime.Today))
            throw new DomainException("Birth Date cannot be in future");


        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
        Note = note;
        Address = address;
        Email = email;
        PhoneNumber = phoneNumber;
        PreferredTherapistId = preferredTherapistId;
    }

    /// <summary>
    /// Creates a new Customer with the specified personal, contact, and optional preference information.
    /// </summary>
    /// <param name="firstName">Customer's first name.</param>
    /// <param name="lastName">Customer's last name.</param>
    /// <param name="birthDate">Customer's date of birth.</param>
    /// <param name="address">Customer's postal address.</param>
    /// <param name="email">Customer's email address.</param>
    /// <param name="phoneNumber">Customer's phone number.</param>
    /// <param name="note">Optional note associated with the customer.</param>
    /// <param name="PreferredTherapistId">Optional preferred therapist identifier.</param>
    /// <returns>A Customer initialized with the provided values.</returns>
    public static Customer Create(
            string firstName,
            string lastName,
            DateOnly birthDate,
            Address address,
            Email email,
            PhoneNumber phoneNumber,
            string note = "",
            Guid? PreferredTherapistId = null)
    {
        var customer = new Customer(firstName, lastName, birthDate, address, email, phoneNumber, note, PreferredTherapistId);
        return customer;
    }

    /// <summary>
    /// Assigns a validated new first name to the FirstName property.
    /// </summary>
    /// <param name="newFirstName">New first name to assign; must pass validation and be different from the current value.</param>
    /// <exception cref="DomainException">Thrown when the provided name is equal to the current FirstName.</exception>
    public void ChangeFirstName(string newFirstName)
    {
        EnsureValidFirstName(newFirstName);

        if (FirstName == newFirstName)
            throw new DomainException("New firstname is the same as current firstname");

        FirstName = newFirstName;
    }

    /// <summary>
    /// Updates the last name to the specified value.
    /// </summary>
    /// <remarks>Validation is performed by EnsureValidLastName.</remarks>
    /// <param name="newLastname">The new last name. Must be valid and different from the current last name.</param>
    /// <exception cref="DomainException">Thrown if newLastname is invalid or is the same as the current last name.</exception>
    public void ChangeLastName(string newLastname)
    {
        EnsureValidLastName(newLastname);

        if (LastName == newLastname)
            throw new DomainException("New lastname is the same as current lastname");

        LastName = newLastname;
    }

    /// <summary>
    /// Updates the current address to the specified new address.
    /// </summary>
    /// <param name="newAddress">The new address to assign.</param>
    /// <exception cref="DomainException">Thrown when the provided new address is equal to the current address.</exception>
    public void ChangeAddress(Address newAddress)
    {
        if (Address == newAddress)
            throw new DomainException("New address is the same as the current address");

        Address = newAddress;
    }

    /// <summary>
    /// Updates the entity's Email property to the specified value.
    /// </summary>
    /// <param name="newEmail">Email value to assign to the entity.</param>
    /// <exception cref="DomainException">Thrown when the provided email is equal to the current email.</exception>
    public void ChangeEmail(Email newEmail)
    {
        if (Email == newEmail)
            throw new DomainException("New email is the same as the current email");

        Email = newEmail;
    }

    /// <summary>
    /// Sets the current PhoneNumber to the specified PhoneNumber.
    /// </summary>
    /// <param name="newPhoneNumber">The new PhoneNumber to assign.</param>
    /// <exception cref="DomainException">Thrown when newPhoneNumber is the same as the current PhoneNumber.</exception>
    public void ChangePhoneNumber(PhoneNumber newPhoneNumber)
    {
        if (PhoneNumber == newPhoneNumber)
            throw new DomainException("New phonenumber is the same as the current phonenumber");

        PhoneNumber = newPhoneNumber;
    }

    /// <summary>
    /// Sets the Note property to the specified value.
    /// </summary>
    /// <param name="note">The value to assign to the Note property.</param>
    public void ChangeNote(string note)
    {
        Note = note;
    }

    /// <summary>
    /// Sets or clears the preferred therapist identifier for the entity.
    /// </summary>
    /// <remarks>Allows null to clear the preference; no additional validation is performed.</remarks>
    /// <param name="newPreferredTherapistId">Identifier of the new preferred therapist, or null to clear the preference.</param>
    /// <exception cref="DomainException">Thrown when newPreferredTherapistId has a value equal to the current PreferredTherapistId.</exception>
    public void ChangePreferredTherapist(Guid? newPreferredTherapistId)
    {
        if (newPreferredTherapistId.HasValue)
        {
            if (PreferredTherapistId == newPreferredTherapistId.Value)
                throw new DomainException("New preferred therapist is the same as current therapist");
        }

        PreferredTherapistId = newPreferredTherapistId;
    }

    /// <summary>
    /// Ensures the first name is not null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="firstName">First name to validate.</param>
    /// <exception cref="DomainException">Thrown when firstName is null, empty, or consists only of white-space characters.</exception>
    private static void EnsureValidFirstName(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("Customer must have a firstname");
    }

    /// <summary>
    /// Validates that the customer's last name is not null, empty, or consists only of white-space characters; throws
    /// DomainException if invalid.
    /// </summary>
    /// <param name="lastName">The customer's last name to validate.</param>
    /// <exception cref="DomainException">Thrown when lastName is null, empty, or consists only of white-space characters.</exception>
    private static void EnsureValidLastName(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Customer must have a lastname");
    }


    private Customer() { }

}
