namespace BookRight.FacadeLib.Commands.Customers.DTOs;

// === Request DTOs for Handlers ===
public record CreateCustomerCommand(
    string FirstName,
    string LastName,
    DateOnly Birthdate,
    string Note,
    string Street,
    string PostalCode,
    string City,
    string EmailAddress,
    string PhoneNumber,
    Guid? PreferredTherapist
    );
