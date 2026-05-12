namespace BookRight.FacadeLib.DTO;

public record CustomerDTO(
    Guid CustomerId,
    string Firstname,
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
