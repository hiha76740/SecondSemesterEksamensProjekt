using BookRight.DomainLib.ValueObjects;
using BookRight.DomainLib.Exceptions;

namespace BookRight.DomainLib.Entities.Customers;

public class Customer : AggregateRoot
{
    public string Firstname { get; private set; } = null!;
    public string Lastname { get; private set; } = null!;
    public DateOnly Birthdate { get; init; }
    public string Note { get; private set; } = null!;
    public Address Address { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public Guid? PreferredTherapist { get; private set; }


    private Customer(
            string firstName,
            string lastName,
            DateOnly birthDate,
            Address address,
            Email email,
            PhoneNumber phoneNumber,
            string note,
            Guid? preferredTherapist)
    {
        EnsureValidFirstname(firstName);
        EnsureValidLastname(lastName);

        if (birthDate > DateOnly.FromDateTime(DateTime.Today))
            throw new DomainException("Birthdate cannot be in future");


        Id = Guid.NewGuid();
        Firstname = firstName;
        Lastname = lastName;
        Birthdate = birthDate;
        Note = note;
        Address = address;
        Email = email;
        PhoneNumber = phoneNumber;
        PreferredTherapist = preferredTherapist;
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


    public void ChangeFirstname(string newFirstname)
    {
        EnsureValidFirstname(newFirstname);

        if (Firstname == newFirstname)
            throw new DomainException("New firstname is the same as current firstname");

        Firstname = newFirstname;
    }

    public void ChangeLastname(string newLastname)
    {
        EnsureValidLastname(newLastname);

        if (Lastname == newLastname)
            throw new DomainException("New lastname is the same as current lastname");

        Lastname = newLastname;
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

    public void ChangePreferredTherapist(Guid? newPreferredTherapist)
    {
        if (newPreferredTherapist.HasValue)
        {
            if (PreferredTherapist == newPreferredTherapist.Value)
                throw new DomainException("New preferred therapist is the same as current therapist"); 
        }

        PreferredTherapist = newPreferredTherapist;
    }

    private static void EnsureValidFirstname(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("Customer must have a firstname");
    }

    private static void EnsureValidLastname(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Customer must have a lastname");
    }

    // EF Constructor
    private Customer() { }

}
