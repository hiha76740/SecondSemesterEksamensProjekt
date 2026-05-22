using BookRight.DomainLib.ValueObjects;
using BookRight.DomainLib.Exceptions;

namespace BookRight.DomainLib.Entities.Customers;

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

    // EF Constructor
    private Customer() { }

}
