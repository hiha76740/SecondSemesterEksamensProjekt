using BookRight.DomainLib.ValueObjects;
using BookRight.DomainLib.Exceptions;

namespace BookRight.DomainLib.Entities.Customers;

public class Customer : AggregateRoot
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateTime Birthdate { get; private set; }
    public string? Note {  get; private set; }
    public Address Address { get; private set; } 
    public Email Email { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }


    private Customer(
            string firstName,
            string lastName,
            DateTime birthdate,
            string? note, 
            Address address,
            Email email,
            PhoneNumber phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("Customer must have a firstname");

        string normalisedFirstName = firstName.Trim();


        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Customer must have a lastname");

        string normalisedLastName = lastName.Trim();


        DateTime normalisedBirthDate = birthdate.Date;
        if (normalisedBirthDate > DateTime.Today)
            throw new DomainException("Birthdate cannot be in future");
        if (normalisedBirthDate < new DateTime(1900, 1, 1))
            throw new DomainException("Birthdate cannot be this far back");


        Id = Guid.NewGuid();
        FirstName = normalisedFirstName
        LastName = normalisedLastName;
        Birthdate = normalisedBirthDate;
        Note = note;
        Address = address ?? throw new DomainException("Customer must have an address");
        Email = email ?? throw new DomainException("Customer must have an email");
        PhoneNumber = phoneNumber ?? throw new DomainException("Customer must have a phonenumber");
    }

    public static Customer Create(
            string firstName,
            string lastName,
            DateTime birthdate,
            string? note,
            Address address,
            Email email,
            PhoneNumber phoneNumber)
    {
        var customer = new Customer(firstName, lastName, birthdate, note, address, email, phoneNumber);
        return customer;
    }


    public void ChangeFirstName(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("Customer must have a firstname");

        string normalisedFirstName = firstName.Trim();

        if (FirstName != normalisedFirstName)
            FirstName = normalisedFirstName;
    }


    public void ChangeLastName(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Customer must have a lastname");

        string normalisedLastName = lastName.Trim();

        if (LastName != normalisedLastName)
            LastName = normalisedLastName;
    }
}
