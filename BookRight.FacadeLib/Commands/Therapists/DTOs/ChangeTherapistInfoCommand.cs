namespace BookRight.FacadeLib.Commands.Therapists.DTOs;

public sealed record ChangeTherapistInfoCommand(
    Guid TherapistId,
    string Name,
    decimal HourlyRate,
    string Street,
    string PostalCode,
    string City,
    string EmailAddress,
    string PhoneNumber,
    List<Guid> AssociatedClinics,
    List<string> Certifications
    );