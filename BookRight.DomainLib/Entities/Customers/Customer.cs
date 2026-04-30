using BookRight.DomainLib.ValueObjects;
using BookRight.DomainLib.Exceptions;

namespace BookRight.DomainLib.Entities.Customers;

public class Customer : AggregateRoot
{
    public string Firstname { get; private set; }
    public string Lastname { get; private set; }
    public DateTime Birthdate { get; init; }
    public string Note { get; private set; }
    public Address Address { get; private set; }
    public Email Email { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public Guid TherapistId { get; private set; }
    //TODO: Add the functionality to assign an optional preferred Therapist to a Customer


    private Customer(
            string firstName,
            string lastName,
            DateTime birthDate,
            Address address,
            Email email,
            PhoneNumber phoneNumber,
            string note)
    {
        EnsureValidFirstname(firstName);
        EnsureValidLastname(lastName);

        string normalisedFirstName = firstName.Trim();
        string normalisedLastName = lastName.Trim();

        if (birthDate.Date > DateTime.Today)
            throw new DomainException("Birthdate cannot be in future");


        Id = Guid.NewGuid();
        Firstname = normalisedFirstName;
        Lastname = normalisedLastName;
        Birthdate = birthDate.Date;
        Note = note;
        Address = address ?? throw new DomainException("Customer must have an address");
        Email = email ?? throw new DomainException("Customer must have an email");
        PhoneNumber = phoneNumber ?? throw new DomainException("Customer must have a phonenumber");
    }

    public static Customer Create(
            string firstName,
            string lastName,
            DateTime birthDate,
            Address address,
            Email email,
            PhoneNumber phoneNumber,
            string note = "")
    {
        var customer = new Customer(firstName, lastName, birthDate, address, email, phoneNumber, note);
        return customer;
    }


    public void ChangeFirstname(string newFirstname)
    {
        EnsureValidFirstname(newFirstname);

        string normalisedNewFirstname = newFirstname.Trim();

        if (Firstname == normalisedNewFirstname)
            throw new DomainException("New firstname is the same as current firstname");

        Firstname = normalisedNewFirstname;
    }

    public void ChangeLastname(string newLastname)
    {
        EnsureValidLastname(newLastname);

        string normalisedNewLastname = newLastname.Trim();

        if (Lastname == normalisedNewLastname)
            throw new DomainException("New lastname is the same as current lastname");

        Lastname = normalisedNewLastname;
    }


    public void ChangeAddress(Address address)
    {
        if (address == null)
            throw new DomainException("Customer must have an address");

        if (Address != address)
            Address = address;
    }


    public void ChangeEmail(Email email)
    {
        if (Address == null)
            throw new DomainException("Customer must have an email");

        if (Email != email)
            Email = email;
    }


    public void ChangePhoneNumber(PhoneNumber phoneNumber)
    {
        if (PhoneNumber == null)
            throw new DomainException("Customer must have a phonenumber");

        if (PhoneNumber != phoneNumber)
            PhoneNumber = phoneNumber;
    }

    private void EnsureValidFirstname(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("Customer must have a firstname");
    }

    private void EnsureValidLastname(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Customer must have a lastname");
    }

}
