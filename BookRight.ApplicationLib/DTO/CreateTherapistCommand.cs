namespace BookRight.FacadeLib.DTO;

public sealed record CreateTherapistCommand(
    string AuthorizationNumber,
    string Name,
    decimal HourlyRate,
    string Street,
    string PostalCode,
    string City,
    string EmailAddress,
    string PhoneNumber);