namespace BookRight.FacadeLib.DTO;

public record TherapistDTO(
    Guid Id,
    string AuthorizationNumber,
    string Name,
    decimal HourlyRate,
    string Street,
    string PostalCode,
    string City,
    string Email,
    string PhoneNumber,
    IReadOnlyList<string> CertificationTypes,
    IReadOnlyList<Guid> AssociatedClinics
    );
