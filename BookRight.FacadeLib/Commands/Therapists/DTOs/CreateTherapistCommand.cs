namespace BookRight.FacadeLib.Commands.Therapists.DTOs;

public sealed record CreateTherapistCommand(
 string AuthorizationNumber,
 string Name,
 decimal HourlyRate,
 string Street,
 string PostalCode,
 string City,
 string EmailAddress,
 string PhoneNumber,
 List<Guid> AssociatedClinicIds);
