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
    public Guid? TherapistId { get; private set; }


    private Customer(
            string firstName,
            string lastName,
            DateTime birthDate,
            Address address,
            Email email,
            PhoneNumber phoneNumber,
            string note,
            Guid? therapistId)
    {
        EnsureValidFirstname(firstName);
        EnsureValidLastname(lastName);

        if (birthDate.Date > DateTime.Today)
            throw new DomainException("Birthdate cannot be in future");

        if (address == null)
            throw new DomainException("Customer must have an address");

        if (email == null)
            throw new DomainException("Customer must have an email");

        if (phoneNumber == null)
            throw new DomainException("Customer must have a phonenumber");

        if (therapistId == Guid.Empty)
            throw new DomainException("TherapistId cannot be empty");


        Id = Guid.NewGuid();
        Firstname = firstName;
        Lastname = lastName;
        Birthdate = birthDate.Date;
        Note = note;
        Address = address;
        Email = email;
        PhoneNumber = phoneNumber;
        TherapistId = therapistId;
    }

    public static Customer Create(
            string firstName,
            string lastName,
            DateTime birthDate,
            Address address,
            Email email,
            PhoneNumber phoneNumber,
            string note = "",
            Guid? therapistId = null)
    {
        var customer = new Customer(firstName, lastName, birthDate, address, email, phoneNumber, note, therapistId);
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

    public void ChangePreferredTherapist(Guid newTherapistId)
    {
        if (TherapistId == newTherapistId)
            throw new DomainException("New preferred therapist is the same as current therapist");

        TherapistId = newTherapistId;
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
