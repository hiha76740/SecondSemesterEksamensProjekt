namespace BookRight.FacadeLib.DTO;

public record CustomerDTO(
    Guid CustomerId,
    string Firstname,
    string Lastname,
    DateOnly Birthdate,
    string Note,
    string Street,
    string PostalCode,
    string City,
    string EmailAddress,
    string PhoneNumber,
    Guid? PreferredTherapist
    );
