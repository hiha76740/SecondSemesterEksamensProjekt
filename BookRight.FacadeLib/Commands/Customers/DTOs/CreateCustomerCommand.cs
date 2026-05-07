namespace BookRight.FacadeLib.Commands.Customers.DTOs;

// === Request DTOs for Handlers ===
public record CreateCustomerCommand(
    string Firstname,
    string Lastname,
    DateTime Birthdate,
    string Note,
    string Street,
    string PostalCode,
    string City,
    string EmailAddress,
    string PhoneNumber,
    Guid? PreferredTherapist
    );
