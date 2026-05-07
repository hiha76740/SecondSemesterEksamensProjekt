
namespace BookRight.FacadeLib.Commands.Customers.DTOs;

// === Request DTO for ChangeCustomerInfoHandler ===
public record ChangeCustomerInfoCommand(
    Guid CustomerId,
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
