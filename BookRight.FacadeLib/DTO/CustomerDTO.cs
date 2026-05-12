namespace BookRight.FacadeLib.DTO;

public record CustomerDTO(
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
