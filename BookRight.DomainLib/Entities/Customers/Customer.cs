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


    public void ChangeLastName(string newLastname)
    {
        EnsureValidLastName(newLastname);

        if (LastName == newLastname)
            throw new DomainException("New lastname is the same as current lastname");

        LastName = newLastname;
    }


    public void ChangeAddress(Address newAddress)
    {
        if (Address == newAddress)
            throw new DomainException("New address is the same as the current address");

        Address = newAddress;
    }


    public void ChangeEmail(Email newEmail)
    {
        if (Email == newEmail)
            throw new DomainException("New email is the same as the current email");

        Email = newEmail;
    }


    public void ChangePhoneNumber(PhoneNumber newPhoneNumber)
    {
        if (PhoneNumber == newPhoneNumber)
            throw new DomainException("New phonenumber is the same as the current phonenumber");

        PhoneNumber = newPhoneNumber;
    }

    public void ChangeNote(string note)
    {
        Note = note;
    }

    public void ChangePreferredTherapist(Guid? newPreferredTherapistId)
    {
        if (newPreferredTherapistId.HasValue)
        {
            if (PreferredTherapistId == newPreferredTherapistId.Value)
                throw new DomainException("New preferred therapist is the same as current therapist");
        }

        PreferredTherapistId = newPreferredTherapistId;
    }

    private static void EnsureValidFirstName(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("Customer must have a firstname");
    }

    private static void EnsureValidLastName(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Customer must have a lastname");
    }


    private Customer() { }

}
