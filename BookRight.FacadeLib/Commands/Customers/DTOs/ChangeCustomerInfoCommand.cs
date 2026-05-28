
namespace BookRight.FacadeLib.Commands.Customers.DTOs;

// === Request DTO for ChangeCustomerInfoHandler ===
public record ChangeCustomerInfoCommand(
    Guid CustomerId,
    string FirstName,
    string LastName,
    DateOnly BirthDate,
    string Note,
    string Street,
    string PostalCode,
    string City,
    string EmailAddress,
    string PhoneNumber,
    Guid? PreferredTherapistId
    );
