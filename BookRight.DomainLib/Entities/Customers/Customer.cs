using BookRight.DomainLib.ValueObjects;

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

}
